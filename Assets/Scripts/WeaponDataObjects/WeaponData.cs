using UnityEngine;

#nullable enable

public class WeaponData : ItemData
{
    readonly EWeaponType weaponType;
    readonly bool isTwoHanded;
    readonly int range;
    readonly int minDamage, maxDamage;
    readonly bool isRanged;

    public WeaponData(WeaponSO weaponSO) : base(weaponSO)
    {
        this.weaponType = weaponSO.WeaponType;
        this.isTwoHanded = weaponSO.IsTwoHanded;
        this.range = weaponSO.Range;
        this.minDamage = weaponSO.MinDamage;
        this.maxDamage = weaponSO.MaxDamage;
        this.isRanged = weaponSO.IsRanged;
    }

    public WeaponSO WeaponSO => (WeaponSO)ItemSO;
}
