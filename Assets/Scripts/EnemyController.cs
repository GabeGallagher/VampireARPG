using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, IDamageable, ILootable
{
    [SerializeField] private LootSO lootSO;

    private int health, maxHealth;

    private void Start()
    {
        health = 100;
    }

    public void DamageReceived(int damageReceived, GameObject damageFrom)
    {
        health -= damageReceived;

        if (health <= 0)
        {
            DropLoot();

            Destroy(gameObject);
        }
    }

    public void DropLoot()
    {
        GameObject loot = Instantiate(lootSO.Prefab);

        loot.transform.position = transform.position;
    }
}
