using UnityEngine;

#nullable enable

public class Item : MonoBehaviour
{
    public enum ERarity
    {
        Common,
        Commodity,
        Uncommon,
        Rare,
        Legendary,
        Unique
    }

    [SerializeField] private ItemSO itemSO;

    private ItemData itemData;

    public ItemSO ItemSO => itemSO;
    public ItemData ItemData => itemData;

    private void Awake()
    {
        SetData();
    }

    protected virtual ItemData SetData()
    {
        if (itemData == null)
        {
            return new ItemData(itemSO);
        }
        return itemData;
    }
}
