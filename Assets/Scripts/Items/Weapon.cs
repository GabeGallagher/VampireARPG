using UnityEngine;

#nullable enable

public class Weapon : Item
{
    private WeaponData weaponData;

    public ItemSO WeaponSO => ItemSO;
    public ItemData WeaponData => weaponData;

    protected override ItemData SetData()
    {
        if (ItemData == null)
        {
            return new WeaponData((WeaponSO)ItemSO);
        }
        return weaponData;
    }
}
