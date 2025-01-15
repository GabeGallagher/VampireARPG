using UnityEngine;

[CreateAssetMenu]
public class PhysicalAttackSO : SkillSO
{
    [SerializeField] private float attackAngle;

    public override ESkillType SkillType => ESkillType.Physical;
    public float AttackAngle => attackAngle;
}
