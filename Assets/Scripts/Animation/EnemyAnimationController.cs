using UnityEngine;

#nullable enable

public class EnemyAnimationController : MonoBehaviour
{
    private Animator animator;
    private EnemyController enemy;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        enemy = transform.parent.GetComponent<EnemyController>();
    }

    private void Start()
    {
        enemy.OnEnemyStateChange += Enemy_OnEnemyStateChange;
    }

    private void Enemy_OnEnemyStateChange(EnemyController.EEnemyState obj)
    {
        SetAnimationState(obj);
    }

    private void SetAnimationState(EnemyController.EEnemyState state)
    {
        switch (state)
        {
            case EnemyController.EEnemyState.Idle:
                animator.SetBool("IsIdle", true);
                animator.SetBool("IsWalking", false);
                animator.SetBool("IsAttacking", false);
                break;

            case EnemyController.EEnemyState.Moving:
            case EnemyController.EEnemyState.Roaming:
                animator.SetBool("IsIdle", false);
                animator.SetBool("IsWalking", true);
                animator.SetBool("IsAttacking", false);
                break;

            case EnemyController.EEnemyState.Attacking:
                animator.SetBool("IsIdle", false);
                animator.SetBool("IsWalking", false);
                animator.SetBool("IsAttacking", true);
                break;

            default:
                Debug.Log($"Invalid state selected: {enemy.state}");
                break;
        }
    }

    private void SpawnDamageCollider()
    {
        enemy.SpawnDamageCollider();
    }
}
