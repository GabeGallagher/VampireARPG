using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour, IDamage, IDamageable, ILootable
{
    public enum EEnemyState
    {
        Idle,
        Moving, // Enemy is moving to attack range of target
        Roaming, // Enemy is wandering aimlessley looking for targets
        Attacking
    }

    public event Action<EEnemyState> OnEnemyStateChange;

    public Transform AttackPoint => attackPoint;

    [SerializeField] private ItemSO itemSO;
    [SerializeField] private EnemySO enemySO;
    [SerializeField] private GameObject damageTextPrefab;
    [SerializeField] private Transform attackPoint;

    private Canvas canvas;

    private NavMeshAgent agent;

    private Vector3 startingPos, targetPos;

    private Coroutine roamingCoroutine;

    private Transform player;

    public EEnemyState state = EEnemyState.Roaming;

    private float idleTime = 5f; // seconds enemies should stay idle before roaming
    private float countingTime = 0f;
    private float roamingRange;

    private int health, maxHealth;

    public EEnemyState State
    {
        get => state;

        set
        {
            if (state != value)
            {
                state = value;
                OnEnemyStateChange?.Invoke(state);
            }
        }
    }

    private void Start()
    {
        canvas = FindObjectOfType<Canvas>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = enemySO.Speed;
        health = 100;
        roamingRange = enemySO.RoamingRange;
        startingPos = transform.position;

        roamingCoroutine = StartCoroutine(GetRoamingTargetCoroutine(roamingRange, position => targetPos = position));
    }

    private void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            if (distanceToPlayer <= enemySO.AggroRange)
            {
                State = EEnemyState.Moving;
                targetPos = player.position;
                agent.SetDestination(targetPos);
            }
        }
        else
        {
            player = GameObject.FindWithTag("Player")?.transform;
        }

        if (State == EEnemyState.Roaming)
        {
            agent.SetDestination(targetPos);

            if (Vector3.Distance(transform.position, targetPos) < 0.5f)
            {
                State = EEnemyState.Idle;
            }
        }
        else if (State == EEnemyState.Idle)
        {
            countingTime += Time.deltaTime;
            if (countingTime >= idleTime)
            {
                countingTime = 0;

                if (roamingCoroutine != null) StopCoroutine(roamingCoroutine);
                roamingCoroutine = StartCoroutine(GetRoamingTargetCoroutine(roamingRange, position => targetPos = position));
            }
        }
        else if (State == EEnemyState.Moving)
        {
            countingTime = -idleTime; // When aggro is broken, enemy idle should double before it starts roam

            if (Vector3.Distance(transform.position, player.position) < enemySO.AttackSO.AttackRange)
            {
                agent.isStopped = true;
                State = EEnemyState.Attacking;
            }
            else
            {
                agent.isStopped = false;
                State = EEnemyState.Moving;
            }

            if (Vector3.Distance(transform.position, player.position) > enemySO.AggroRange)
            {
                startingPos = transform.position;
                agent.isStopped = true;
                State = EEnemyState.Idle;
            }
        }
    }

    private IEnumerator GetRoamingTargetCoroutine(float range, System.Action<Vector3> onTargetFound)
    {
        while (true)
        {
            Vector2 randomPoint = Random.insideUnitSphere * range;

            Vector3 roamingTarget = new Vector3(randomPoint.x, 0, randomPoint.y) + startingPos;

            if (NavMesh.SamplePosition(roamingTarget, out NavMeshHit target, 2f, NavMesh.AllAreas))
            {
                onTargetFound?.Invoke(target.position);
                State = EEnemyState.Roaming;
                agent.isStopped = false;
                yield break;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void DamageReceived(int damageReceived, GameObject damageFrom)
    {
        health -= damageReceived;

        ShowDamageText(damageReceived);

        if (health <= 0)
        {
            DropLoot();

            if (damageFrom.GetComponent<PlayerController>())
            {
                damageFrom.GetComponent<PlayerController>().TargetKilled(gameObject);
            }
            Destroy(gameObject);
        }
    }

    public void ShowDamageText(int damageAmount)
    {
        GameObject damageText = Instantiate(damageTextPrefab, transform.position, Quaternion.identity, canvas.transform);

        Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);

        RectTransform rectTransform = damageText.GetComponent<RectTransform>();

        rectTransform.position = screenPosition;

        TextMeshProUGUI textMesh = damageText.GetComponent<TextMeshProUGUI>();

        if (textMesh != null)
        {
            textMesh.text = damageAmount.ToString();
        }
    }

    public void DropLoot()
    {
        GameObject item = Instantiate(itemSO.Prefab);

        item.transform.position = transform.position;
    }

    public void DealDamage(int damage, List<Transform> targetList)
    {
        foreach (Transform target in targetList)
        {
            if (target.GetComponent<IDamageable>() != null)
            {
                target.GetComponent<IDamageable>().DamageReceived(damage, gameObject);
            }
        }
    }

    public void SpawnDamageCollider()
    {
        GameObject attackCollider = Instantiate(enemySO.AttackSO.AttackObjectPrefab);
        attackCollider.GetComponent<AttackController>().Origin = attackPoint;
        attackCollider.GetComponent<AttackController>().Start();
    }

    public int CalcDamage(SkillSO damageSkill)
    {
        return (int)damageSkill.Damage;
    }
}
