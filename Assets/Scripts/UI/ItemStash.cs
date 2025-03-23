using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemStash : MonoBehaviour
{
    public List<GameObject> stashRowList = new List<GameObject>();

    public void Awake()
    {
        if (stashRowList.Count == 0)
        {
            throw new ArgumentException("No stash rows found in item stash");
        }

        for (int i = 0; i < stashRowList.Count; i++)
        {
            GameObject gameObject = stashRowList[i];
            if (gameObject.tag != "StashRow")
            {
                throw new ArgumentException($"Game object at index {i} in the stash row list does not have the 'StashRow' tag! Object name: {gameObject.name}");
            }
        }
    }
}
