using UnityEngine;

[CreateAssetMenu]
public class SkillSO : ScriptableObject
{
    public enum ESkillType
    {
        Physical,
        Ranged,
        Magical,
        Passive,
        Null
    }

    [SerializeField] private GameObject atkObjPrefab;
    [SerializeField] private Sprite icon;
    [SerializeField] private string skillName, description;
    [SerializeField] private float damage;
    [SerializeField] private float attackRange;

    public virtual ESkillType SkillType => ESkillType.Null;
    public GameObject AttackObjectPrefab => atkObjPrefab;
    public Sprite Icon => icon;
    public string SkillName => skillName;
    public string Description => description;
    public float Damage => damage;
    public virtual float AttackRange => attackRange;
}
