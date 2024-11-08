using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] private GameObject inventorySprite;

    public GameObject InventorySprite { get => inventorySprite;}

    public void SetSprite(Sprite newItemSprite)
    {
        Image sprite = inventorySprite.GetComponent<Image>();

        sprite.sprite = newItemSprite;

        Color color = sprite.color;

        color.a = Mathf.Clamp01(1.0f);

        sprite.color = color;
    }
}
