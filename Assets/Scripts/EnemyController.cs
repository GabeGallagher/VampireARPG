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
    [SerializeField] private ItemSO itemSO;
    [SerializeField] private EnemySO enemySO;
    [SerializeField] private GameObject damageTextPrefab;

    private Canvas canvas;

    private NavMeshAgent agent;

    private Vector3 startingPos, targetPos;

    private Coroutine roamingCoroutine;

    private Transform player;

    private float idleTime = 5f; // seconds enemies should stay idle before roaming

    private float countingTime = 0f;

    private float roamingRange;

    private int health, maxHealth;

    private bool isRoaming = true;

    private bool isAttacking = false;

    private bool isIdle = false;

    public bool IsRoaming
    {
        get => isRoaming;

        set
        {
            isRoaming = value;
            if (isRoaming)
            {
                isAttacking = false;
                isIdle = false;
            }
        }
    }

    public bool IsAttacking
    {
        get => isAttacking;

        set
        {
            isAttacking = value;
            if (isAttacking)
            {
                isRoaming = false;
                isIdle = false;
            }
        }
    }

    public bool IsIdle
    {
        get => isIdle;

        set
        {
            isIdle = value;
            if (isIdle)
            {
                isAttacking = false;
                isRoaming = false;
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
                IsAttacking = true;
                targetPos = player.position;
                agent.SetDestination(targetPos);
            }
        }
        else
        {
            player = GameObject.FindWithTag("Player")?.transform;
        }

        if (isRoaming)
        {
            agent.SetDestination(targetPos);

            if (Vector3.Distance(transform.position, targetPos) < 0.5f)
            {
                IsIdle = true;
            }
        }
        else if (isIdle)
        {
            countingTime += Time.deltaTime;
            if (countingTime >= idleTime)
            {
                countingTime = 0;

                if (roamingCoroutine != null) StopCoroutine(roamingCoroutine);
                roamingCoroutine = StartCoroutine(GetRoamingTargetCoroutine(roamingRange, position => targetPos = position));
            }
        }
        else if (isAttacking)
        {
            countingTime = -idleTime; // When aggro is broken, enemy idle should double before it starts roam

            if (Vector3.Distance(transform.position, player.position) < enemySO.AttackRange)
            {
                agent.isStopped = true;
                Debug.Log($"{transform.name} attacking player");
                List<Transform> targets = new List<Transform>();
                targets.Add(player);
                DealDamage(enemySO.Damage, targets);
            }
            else
            {
                agent.isStopped = false;
                Debug.Log($"{transform.name} moving to attack player");
            }

            if (Vector3.Distance(transform.position, player.position) > enemySO.AggroRange)
            {
                startingPos = transform.position;
                IsIdle = true;
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
                IsRoaming = true;
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
}
