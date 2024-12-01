using UnityEngine;

#nullable enable

[CreateAssetMenu]
public class WeaponSO : ItemSO
{
    [SerializeField] private bool isTwoHanded;
    [SerializeField] private int range;
    [SerializeField] private int minDamage, maxDamage;
    [SerializeField] private bool isRanged;

    public override EItemType ItemType => EItemType.Weapon;
    public int Range => GetRange();
    public int MinDamage { get => minDamage; }
    public int MaxDamage { get => maxDamage; }
    public bool IsRanged { get => isRanged; }
    public bool IsTwoHanded => isTwoHanded;

    private int GetRange()
    {
        if (isRanged) return int.MaxValue;
        else return range;
    }
}
