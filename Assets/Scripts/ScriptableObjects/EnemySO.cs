using UnityEngine;

[CreateAssetMenu]
public class EnemySO : ScriptableObject
{
    [SerializeField] private float speed, roamingRange, aggroRange, attackRange;

    public float Speed { get => speed; }
    public float RoamingRange { get => roamingRange; }
    public float AggroRange {  get => aggroRange; }
    public float AttackRange { get => attackRange; }
}
