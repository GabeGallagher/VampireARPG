using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] private GameObject inventorySprite;
    [SerializeField] private ItemData itemData;

    public GameObject InventorySprite { get => inventorySprite;}
    public ItemData ItemData { get => itemData; set => SetItem(value); }

    private void SetItem(ItemData itemData)
    {
        this.itemData = itemData;

        SetSprite(itemData.Sprite);
    }

    public void SetSprite(Sprite newItemSprite)
    {
        Image sprite = inventorySprite.GetComponent<Image>();

        sprite.sprite = newItemSprite;

        Color color = sprite.color;

        color.a = Mathf.Clamp01(1.0f);

        sprite.color = color;
    }

    public void ClearSprite()
    {
        Image sprite = inventorySprite.GetComponent<Image>();

        if (sprite != null)
        {
            sprite.sprite = null;

            Color color = sprite.color;

            color.a = Mathf.Clamp01(0f);

            sprite.color = color;
        }
    }
}
