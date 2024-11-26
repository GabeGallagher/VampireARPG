using UnityEngine;
using UnityEngine.UI;

public class AvailableSkillsPanel : MonoBehaviour
{
    [SerializeField] private Transform skillsContainer;
    [SerializeField] private GameObject availableSkillsButtonPrefab;

    public void BuildSkillsPanel(PlayerController player)
    {
        ClearSkills();

        foreach (SkillSO skill in player.learnedSkills)
        {
            GameObject skillBtn = Instantiate(availableSkillsButtonPrefab, skillsContainer, false);
            AvailableSkillsButtonController skillBtnController = skillBtn.GetComponent<AvailableSkillsButtonController>();
            skillBtnController.Skill = skill;
            skillBtn.name = $"{skill.name}_button";
            Image img = skillBtn.GetComponent<Image>();
            img.sprite = skill.Icon;
        }
    }

    private void ClearSkills()
    {
        for (int i = skillsContainer.childCount - 1; i >= 0; i--)
        {
            GameObject.Destroy(skillsContainer.GetChild(i).gameObject);
        }
    }
}
