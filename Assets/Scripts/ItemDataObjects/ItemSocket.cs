using UnityEngine;

#nullable enable

public class ItemSocket : MonoBehaviour
{
    [SerializeField] private EWeaponType weaponType;

    public EWeaponType WeaponType => weaponType;
}
