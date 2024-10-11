using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class LootSO : ScriptableObject
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private string lootName;

    public GameObject Prefab { get => prefab; }

    public string LootName { get => lootName; }
}
