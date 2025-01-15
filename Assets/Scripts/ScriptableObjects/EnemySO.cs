using UnityEngine;

[CreateAssetMenu]
public class EnemySO : ScriptableObject
{
    [SerializeField] private SkillSO attackSO;
    [SerializeField] private float speed, roamingRange, aggroRange;

    public float Speed => speed;
    public float RoamingRange => roamingRange;
    public float AggroRange => aggroRange;
    public SkillSO AttackSO => attackSO;
}
