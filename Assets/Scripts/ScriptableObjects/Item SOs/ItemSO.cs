using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu]
public class ItemSO : ScriptableObject
{
    public enum EItemType
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
    [SerializeField] private EItemType itemType;
    [SerializeField] private string itemName;

    public GameObject Prefab { get => prefab; }
    public Sprite Sprite { get => sprite; }
    public virtual EItemType ItemType { get => itemType; }
    public string ItemName { get => itemName; }
}
