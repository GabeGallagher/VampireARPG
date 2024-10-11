using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootController : MonoBehaviour
{
    public void PickUp(PlayerController player)
    {
        player.Inventory.Add(gameObject);

        player.StopMoving();

        Destroy(gameObject);
    }
}
