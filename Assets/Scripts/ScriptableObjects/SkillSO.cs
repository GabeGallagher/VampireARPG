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

    [SerializeField] private Sprite icon;
    [SerializeField] private string skillName, description;
    [SerializeField] private float damage;
    [SerializeField] private int range;

    public virtual ESkillType SkillType => ESkillType.Null;
    public Sprite Icon => icon;
    public string SkillName => skillName;
    public string Description => description;
    public float Damage => damage;
    public virtual int Range => range;
}
