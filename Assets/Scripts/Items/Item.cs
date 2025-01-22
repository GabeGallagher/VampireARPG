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
        itemData = SetData();
    }

    protected virtual ItemData SetData()
    {
        if (itemData == null)
        {
            return new ItemData(itemSO);
        }
        return itemData;
    }

    public void ResetTransform()
    {
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }
}
