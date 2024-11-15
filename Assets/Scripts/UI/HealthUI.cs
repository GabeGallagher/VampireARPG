using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    private Image uiElement;

    private void Awake()
    {
        uiElement = GetComponent<Image>();
    }

    private void Update()
    {
        if (playerController != null && uiElement != null)
        {
            uiElement.fillAmount = GetFillAmount();
        }
    }

    private float GetFillAmount()
    {
        return (float)playerController.CurrentHealth / (float)playerController.MaxHealth;
    }
}
