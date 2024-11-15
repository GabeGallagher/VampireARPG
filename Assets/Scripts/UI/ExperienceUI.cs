using UnityEngine;
using UnityEngine.UI;

public class ExperienceUI : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    private void Update()
    {
        if (slider != null && playerController != null)
        {
            slider.value = GetExperience();
        }
    }

    private float GetExperience()
    {
        return (float)playerController.Experience / (float)playerController.LevelUpExperience;
    }
}
