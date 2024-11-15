using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMesh;
    [SerializeField] private Image image;
    private BuildableSO so;

    public BuildableSO SO { get => so; set => SetBuildable(value); }

    public void SetBuildable(BuildableSO buildableSO)
    {
        so = buildableSO;

        textMesh.text = so.name;

        image.sprite = so.Sprite;
    }
}
