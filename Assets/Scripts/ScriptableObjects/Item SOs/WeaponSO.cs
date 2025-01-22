using UnityEngine;

#nullable enable

[CreateAssetMenu]
public class WeaponSO : ItemSO
{
    [SerializeField] private EWeaponType weaponType;
    [SerializeField] private bool isTwoHanded;
    [SerializeField] private int range;
    [SerializeField] private int minDamage, maxDamage;
    [SerializeField] private bool isRanged;

    public override EItemType ItemType => EItemType.Weapon;
    public EWeaponType WeaponType => weaponType;
    public int Range => GetRange();
    public int MinDamage => minDamage;
    public int MaxDamage => maxDamage;
    public bool IsRanged => isRanged;
    public bool IsTwoHanded => isTwoHanded;

    private int GetRange()
    {
        if (isRanged) return int.MaxValue;
        else return range;
    }
}
