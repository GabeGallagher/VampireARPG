using UnityEngine;

public class AvailableSkillsButtonController : MonoBehaviour
{
    private GameObject availableSkillsPanel;
    private SkillSlotController skillSlot;
    private SkillSO skill;

    public SkillSO Skill { set => skill = value; }

    private void Awake()
    {
        availableSkillsPanel = transform.parent.parent.gameObject;
        skillSlot = availableSkillsPanel.transform.parent.GetComponent<SkillSlotController>();
    }

    public void OnClick()
    {
        skillSlot.skill = skill;
        skillSlot.img.gameObject.SetActive(true);
        skillSlot.img.sprite = skill.Icon;
        availableSkillsPanel.SetActive( false );
    }
}
