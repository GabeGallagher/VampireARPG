using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyController : MonoBehaviour, IDamageable, ILootable
{
    [SerializeField] private LootSO lootSO;

    [SerializeField] private GameObject damageTextPrefab;

    private Canvas canvas;

    private int health, maxHealth;

    private void Start()
    {
        canvas = FindObjectOfType<Canvas>();

        health = 100;
    }

    public void DamageReceived(int damageReceived, GameObject damageFrom)
    {
        health -= damageReceived;

        ShowDamageText(damageReceived);

        if (health <= 0)
        {
            DropLoot();

            Destroy(gameObject);
        }
    }

    private void ShowDamageText(int damageAmount)
    {
        GameObject damageText = Instantiate(damageTextPrefab, transform.position, Quaternion.identity, canvas.transform);

        Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);

        RectTransform rectTransform = damageText.GetComponent<RectTransform>();

        rectTransform.position = screenPosition;

        TextMeshProUGUI textMesh = damageText.GetComponent<TextMeshProUGUI>();

        if (textMesh != null)
        {
            textMesh.text = damageAmount.ToString();
        }
    }

    public void DropLoot()
    {
        GameObject loot = Instantiate(lootSO.Prefab);

        loot.transform.position = transform.position;
    }
}
