using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private int range;
    [SerializeField] private int minDamage, maxDamage;
    [SerializeField] private bool isRanged;

    public GameObject Prefab { get => prefab; }
    public Sprite Sprite { get => sprite; }
    public EItemType ItemType {  get => itemType; }
    public string ItemName { get => itemName; }
    public int Range { get => range; }
    public int MinDamage { get => minDamage; }
    public int MaxDamage { get => maxDamage; }
    public bool IsRanged { get => isRanged; }
}
