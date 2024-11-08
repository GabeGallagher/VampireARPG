using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private GameObject itemStash;

    private List<ItemSO> itemsList = new List<ItemSO>();
        
    public void AddItem(ItemSO item)
    {
        itemsList.Add(item);

        UpdateStash();
    }

    private void UpdateStash()
    {
        foreach (ItemSO item in itemsList)
        {
            for (int i = 0; i < itemStash.transform.childCount; i++)
            {
                for (int j= 0; j < itemStash.transform.GetChild(i).childCount; j++)
                {
                    Transform itemSlotTransform = itemStash.transform.GetChild(i).GetChild(j);

                    ItemSlot itemSlot = itemSlotTransform.GetComponent<ItemSlot>();

                    if (itemSlot.InventorySprite.GetComponent<Image>().sprite == null)
                    {
                        itemSlot.SetSprite(item.Sprite);
                        i = itemStash.transform.childCount;
                        break;
                    }
                }
            }
        }
    }
}
