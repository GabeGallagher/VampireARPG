using UnityEngine;

public class SkillTabController : MonoBehaviour
{
    [SerializeField] private InputController inputController;

    private void Start()
    {
        inputController = GetComponent<InputController>();
    }
}
