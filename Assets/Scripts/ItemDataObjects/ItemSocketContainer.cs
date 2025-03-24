using System;
using System.Linq;
using UnityEngine;

#nullable enable

public class ItemSocketContainer : MonoBehaviour
{
    public void EquipItem(GameObject item)
    {
        ItemData itemData = item.GetComponent<Item>().ItemData;
        if (itemData == null)
        {
            Debug.LogError($"{gameObject.name} could not find item data on {item.name}");
        }
        else
        {
            if (itemData.ItemType == EItemType.Weapon)
            {
                Weapon weapon = item.GetComponent<Weapon>();
                WeaponSO weaponSO = (WeaponSO)itemData.ItemSO;
                Transform weaponSocket = GetComponentsInChildren<ItemSocket>()
                    .FirstOrDefault(socket => socket.WeaponType == weaponSO.WeaponType)?
                    .transform;

                if (weaponSocket == null)
                {
                    Debug.LogError($"No socket found for weapon type {weaponSO.WeaponType}"); return;
                }
                weapon.transform.SetParent( weaponSocket );
                weapon.GetComponent<Item>().ResetTransform();

                /*This is a super dirty way to expose the origin point of of a
                melee weapon attack to the player controller. Needs refactor
                for my own sanity but it's the best way to implement given the
                current codebase :( */
                try
                {
                    PlayerController player = FindAnyObjectByType<PlayerController>();
                    player.MainHand = weapon;
                }
                catch (NullReferenceException ex)
                {
                    Debug.LogError("Player not found: " + ex.Message);
                }
            }
        }
    }
}
