using System;
using UnityEngine;

#nullable enable

public class AttackController : MonoBehaviour
{
    private Transform origin, attacker;
    private PlayerController? playerController;
    private EnemyController? enemyController;
    private SphereCollider collider;
    private SkillSO skill;

    private float radius, timeLive;
    private int damage;

    public Transform Origin { set => origin = value; }
    public Transform Attacker { set => attacker = value; }
    public PlayerController Player { set => playerController = value; }
    public SkillSO Skill { get => skill; set => skill = value; }
    public float Radius { set => radius = SetRadius(value); }
    public int Damage { set =>  damage = value; }

    private void Awake()
    {
        collider = GetComponent<SphereCollider>();
    }

    public void Start()
    {
        if (origin.position == null)
        {
            Debug.LogError($"Origin of {gameObject.name} not found");
        }
        else
        {
            transform.position = origin.position;
        }
        SetAttackerController();
    }

    private void SetAttackerController()
    {
        if (attacker.GetComponent<EnemyController>() != null)
        {
            enemyController = attacker.GetComponent<EnemyController>();
        }
        else if (attacker.GetComponent<PlayerController>() != null)
        {
            playerController = attacker.GetComponent<PlayerController>();
        }
        else
        {
            Debug.LogError($"Invalid attacker: {attacker.name}");
        }
    }

    private void Update()
    {
        timeLive += Time.deltaTime;
        if (timeLive >= 0.01f)
        {
            Destroy(gameObject);
        }
    }

    private float SetRadius(float radius)
    {
        collider.radius = radius;
        return radius;
    }

    private void DealDamage(Collider target)
    {
        IDamageable damageableTarget = target.GetComponent<IDamageable>();
        if (damageableTarget == null)
        {
            Debug.Log($"No IDamageable component retrieved: {target.name}"); return;
        }

        IDealDamage attacker = enemyController != null ?
            (IDealDamage)enemyController :
            (IDealDamage)playerController;

        Debug.Log($"{target.name} damaged");
        int damage = attacker.CalcDamage(skill);
        damageableTarget.DamageReceived(damage, attacker is EnemyController ?
            enemyController.gameObject :
            playerController.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"{attacker.name} Attacked: {other.name}");

        if (other.name != attacker.name)
        {
            DealDamage(other);
        }
    }
}
