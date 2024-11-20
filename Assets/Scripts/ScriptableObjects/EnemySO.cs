using UnityEngine;

[CreateAssetMenu]
public class EnemySO : ScriptableObject
{
    [SerializeField] private float speed, roamingRange, aggroRange, attackRange;
    [SerializeField] private int damage;

    public float Speed { get => speed; }
    public float RoamingRange { get => roamingRange; }
    public float AggroRange {  get => aggroRange; }
    public float AttackRange { get => attackRange; }
    public int Damage { get => damage; }
}
