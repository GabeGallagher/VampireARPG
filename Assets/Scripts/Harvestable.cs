using System;
using TMPro;
using UnityEngine;

#nullable enable

public class Harvestable : MonoBehaviour, IDamageable
{
    [SerializeField] private GameObject damageTextPrefab;
    [SerializeField] private HarvestableSO harvestableSO;
    [SerializeField] private int health = 100;

    private Canvas canvas;

    private void Start()
    {
        try
        {
            canvas = FindObjectOfType<Canvas>();
        }
        catch (NullReferenceException e)
        {
            Debug.LogError($"{gameObject.name} could not find canvas: {e.ToString()}");
        }
    }

    public void DamageReceived(int damageReceived, GameObject damageFrom)
    {
        health -= damageReceived;

        ShowDamageText(damageReceived);

        if (damageFrom.GetComponent<PlayerController>() != null)
        {
            ReturnResource(damageReceived, damageFrom.GetComponent<PlayerController>());
        }
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void ShowDamageText(int damageAmount)
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

    private void ReturnResource(int resourceReturn, PlayerController player)
    {
        player.HarvestResource(resourceReturn, harvestableSO);
    }
}
