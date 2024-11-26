using UnityEngine;
using UnityEngine.UI;

public class SkillTreeButtonController : MonoBehaviour
{
    [SerializeField] private SkillSO skillSO;

    private PlayerController player;
    private bool isLearned = false;
    private bool isEmpowered = false;

    private void Awake()
    {
        if (skillSO != null)
        {
            GetComponent<Button>().image.sprite = skillSO.Icon;
        }
        else
        {
            Debug.Log($"{gameObject.name} does not have an attached Skill");
        }
    }

    private void Start()
    {
        player = GetComponent<PlayerController>();
    }

    public void OnClick()
    {
        if (!isLearned)
        {
            player.learnedSkills.Add(skillSO);
        }
    }
}
