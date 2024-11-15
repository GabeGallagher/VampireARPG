using System.Collections.Generic;
using UnityEngine;

public class BuildMenuUI : MonoBehaviour
{
    public List<BuildableSO> buildablesList;

    [SerializeField] private GameObject buildButtonPrefab;

    private InputController inputController;

    private Transform buildButtonsParent;

    private string buildButtonsParentName = "BuildingButtons";

    private void Start()
    {
        inputController = FindAnyObjectByType<InputController>();

        inputController.OnToggleBuildMode += InputController_OnToggleBuildMode;

        buildButtonsParent = transform.Find(buildButtonsParentName);

        gameObject.SetActive(false);
    }

    private void InputController_OnToggleBuildMode(object sender, System.EventArgs e)
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);

            if (buildButtonsParent == null)
            {
                Debug.LogError($"Cannot find object: {buildButtonsParentName}");
            }
            foreach (BuildableSO buildable in buildablesList)
            {
                GameObject buttonGameObject = Instantiate(buildButtonPrefab, buildButtonsParent);

                BuildButton button = buttonGameObject.GetComponent<BuildButton>();

                button.SO = buildable;
            }
        }
        else
        {
            foreach (Transform button in buildButtonsParent)
            {
                Destroy(button.gameObject);
            }
            gameObject.SetActive(false);
        }
    }
}
