using UnityEngine;

#nullable enable

public class AttackController : MonoBehaviour
{
    private PlayerController playerController;
    private SphereCollider collider;
    private PhysicalAttackSO skill;
    private float radius, timeLive;

    public PlayerController Player { set => playerController = value; }
    public PhysicalAttackSO Skill { get => skill; set => skill = value; }
    public float Radius { set => radius = SetRadius(value); }

    private void Awake()
    {
        collider = GetComponent<SphereCollider>();
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Vector3 directionToEnemy = (other.transform.position - transform.position).normalized;
            float dotProduct = Vector3.Dot(transform.forward, directionToEnemy);
            float minDotProduct = Mathf.Cos(skill.AttackAngle * 0.5f * Mathf.Deg2Rad);

            if (dotProduct >= minDotProduct)
            {
                EnemyController enemy = other.GetComponent<EnemyController>();
                if (enemy != null)
                {
                    Debug.Log($"{enemy.name} damaged");
                    int damage = playerController.CalcDamage(skill);
                    enemy.DamageReceived(damage, playerController.gameObject);
                }
            }
        }
    }
}
