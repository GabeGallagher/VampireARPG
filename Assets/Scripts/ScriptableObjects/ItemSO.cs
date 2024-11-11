using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ItemSO : ScriptableObject
{
    public enum ItemType
    {
        Weapon,
        OffHand,
        Helm,
        Chest,
        Boots,
        Gloves,
        Ring,
        Amulet,
        Belt,
    }

    [SerializeField] private GameObject prefab;
    [SerializeField] private Sprite sprite;
    [SerializeField] private ItemType type;
    [SerializeField] private string itemName;

    public GameObject Prefab { get => prefab; }

    public Sprite Sprite { get => sprite; }

    public ItemType Type {  get => type; }

    public string ItemName { get => itemName; }
}
