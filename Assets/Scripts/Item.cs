using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType
    {
        Weapon,
        Helm,
        Chest,
        Boots,
        Gloves,
        Ring,
        Amulet,
        Belt,
    }

    [SerializeField] private ItemSO itemSO;

    public ItemSO ItemSO { get => itemSO; }
}
