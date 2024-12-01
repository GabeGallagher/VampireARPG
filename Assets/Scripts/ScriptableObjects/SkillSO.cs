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
        Default
    }

    [SerializeField] private Sprite icon;
    [SerializeField] private string skillName, description;
    [SerializeField] private float damage;
    [SerializeField] private int range;

    public virtual ESkillType SkillType => ESkillType.Default;
    public Sprite Icon { get => icon; }
    public string SkillName { get => skillName; }
    public string Description { get => description; }
    public float Damage { get => damage; }
    public virtual int Range => range;
}
