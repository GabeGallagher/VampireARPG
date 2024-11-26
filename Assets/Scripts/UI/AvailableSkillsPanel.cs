using UnityEngine;
using UnityEngine.UI;

public class AvailableSkillsPanel : MonoBehaviour
{
    [SerializeField] private Transform skillsContainer;

    public void BuildSkillsPanel(PlayerController player)
    {
        ClearSkills();

        foreach (SkillSO skill in player.learnedSkills)
        {
            GameObject imgObj = new GameObject($"{skill.name}_icon");
            Image img = imgObj.AddComponent<Image>();
            imgObj.transform.SetParent(skillsContainer.transform, false);
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
