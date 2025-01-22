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
                WeaponSO weaponSO = (WeaponSO)itemData.ItemSO;
                Transform weaponSocket = GetComponentsInChildren<ItemSocket>()
                    .FirstOrDefault(socket => socket.WeaponType == weaponSO.WeaponType)?
                    .transform;

                if (weaponSocket == null)
                {
                    Debug.LogError($"No socket found for weapon type {weaponSO.WeaponType}"); return;
                }
                item.transform.SetParent( weaponSocket );
                item.GetComponent<Item>().ResetTransform();
            }
        }
    }
}
