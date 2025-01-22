using UnityEngine;

#nullable enable

public class Weapon : Item
{
    [SerializeField] private Transform damageOrigin;

    private WeaponData weaponData;

    public WeaponSO WeaponSO => (WeaponSO)ItemSO;
    public ItemData WeaponData => weaponData;
    public Transform DamageOrigin => damageOrigin;

    protected override ItemData SetData()
    {
        if (ItemData == null)
        {
            return new WeaponData((WeaponSO)ItemSO);
        }
        return weaponData;
    }
}
