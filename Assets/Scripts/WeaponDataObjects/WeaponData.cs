using UnityEngine;

#nullable enable

public class WeaponData : ItemData
{
    public WeaponData(WeaponSO weaponSO) : base(weaponSO) { }

    public WeaponSO WeaponSO => (WeaponSO)ItemSO;
}
