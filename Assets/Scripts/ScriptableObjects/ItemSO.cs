using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ItemSO : ScriptableObject
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private Sprite sprite;
    [SerializeField] private string itemName;

    public GameObject Prefab { get => prefab; }

    public Sprite Sprite { get => sprite; }

    public string ItemName { get => itemName; }
}
