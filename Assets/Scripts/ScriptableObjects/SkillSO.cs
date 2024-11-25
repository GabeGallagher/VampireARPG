using UnityEngine;

[CreateAssetMenu]
public class SkillSO : ScriptableObject
{
    public enum ESkillType
    {
        Physical,
        Ranged,
        Magical,
        Passive
    }

    [SerializeField] private Sprite icon;
    [SerializeField] private string skillName, description;
    [SerializeField] private float damage;
    [SerializeField] private int range;

    public Sprite Icon { get => icon; }
    public string SkillName { get => skillName; }
    public string Description { get => description; }
    public float Damage { get => damage; }
}
