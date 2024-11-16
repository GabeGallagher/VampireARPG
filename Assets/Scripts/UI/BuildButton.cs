using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMesh;
    [SerializeField] private Image image;

    private BuildableSO so;
    private CastleBuildingSystem castleBuildingSystem;

    public BuildableSO SO { get => so; set => SetBuildable(value); }

    private void Start()
    {
        castleBuildingSystem = FindAnyObjectByType<CastleBuildingSystem>();
    }

    public void SetBuildable(BuildableSO buildableSO)
    {
        so = buildableSO;

        textMesh.text = so.name;

        image.sprite = so.Sprite;
    }

    public void OnClick()
    {
        castleBuildingSystem.PlaceablePrefab = so.Prefab;

        Debug.Log($"{so.name} clicked");
    }
}
