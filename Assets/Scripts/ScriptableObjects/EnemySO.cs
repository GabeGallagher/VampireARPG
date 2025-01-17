using UnityEngine;

[CreateAssetMenu]
public class EnemySO : ScriptableObject
{
    [SerializeField] private SkillSO attackSO;
    [SerializeField] private float speed, roamingRange, aggroRange;
    [SerializeField] private int damage;

    public float Speed => speed;
    public float RoamingRange => roamingRange;
    public float AggroRange => aggroRange;
    public int Damage => damage;
    public SkillSO AttackSO => attackSO;
}
