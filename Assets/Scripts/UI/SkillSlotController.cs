using UnityEngine;
using UnityEngine.UI;

public class SkillSlotController : MonoBehaviour
{
    public SkillSO skill;
    public Image img;

    [SerializeField] private AvailableSkillsPanel availableSkillsPanel;

    private PlayerController player;

    private void Start()
    {
        player = FindAnyObjectByType<PlayerController>();
    }

    public void OnClick()
    {
        availableSkillsPanel.gameObject.SetActive(!availableSkillsPanel.gameObject.activeSelf);
        if (availableSkillsPanel.gameObject.activeSelf == true)
        {
            availableSkillsPanel.BuildSkillsPanel(player);
        }
    }
}
