using UnityEngine;

public class SkillTabController : MonoBehaviour
{
    private InputController inputController;

    private void Start()
    {
        inputController = GetComponent<InputController>();
        gameObject.SetActive(false);
    }
}
