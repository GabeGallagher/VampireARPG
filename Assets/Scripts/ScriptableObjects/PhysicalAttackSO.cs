using UnityEngine;

[CreateAssetMenu]
public class PhysicalAttackSO : SkillSO
{
    [SerializeField] private GameObject atkObjPrefab;
    [SerializeField] private float attackAngle;

    public override ESkillType SkillType => ESkillType.Physical;
    public GameObject AttackObjectPrefab => atkObjPrefab;
    public float AttackAngle => attackAngle;
}
