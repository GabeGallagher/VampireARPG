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
    private int[,] stashLocation; // [row, column]
    public ItemSO ItemSO => itemSO;
    public ItemData ItemData => itemData;

    private void Awake()
    {
        itemData = SetData();
        stashLocation = new int[4, 10];
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
