using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] private GameObject inventorySprite;

    [SerializeField] private ItemSO itemSO;

    public GameObject InventorySprite { get => inventorySprite;}

    public ItemSO ItemSO { get => itemSO; set => SetItem(value); }

    private void SetItem(ItemSO itemSO)
    {
        this.itemSO = itemSO;

        SetSprite(itemSO.Sprite);
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
