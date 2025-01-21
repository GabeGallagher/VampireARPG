using UnityEngine;

#nullable enable

public class ItemData
{
    readonly string name;
    readonly Sprite sprite;
    readonly EItemType itemType;
    readonly ItemSO itemSO;

    public ItemData(ItemSO itemSO)
    {
        this.name = itemSO.ItemName;
        this.sprite = itemSO.Sprite;
        this.itemType = itemSO.ItemType;
        this.itemSO = itemSO;
    }

    public string Name => name;
    public Sprite Sprite => sprite;
    public EItemType ItemType => itemType;
    public ItemSO ItemSO => itemSO;
}
