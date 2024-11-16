using UnityEngine;

[CreateAssetMenu()]
public class BuildableSO : ScriptableObject
{
    public enum EBuildableType
    {
        Floor,
        Wall,
        Desk
    }

    [SerializeField] private GameObject prefab;
    [SerializeField] private Sprite sprite;
    [SerializeField] private string buildableName;
    [SerializeField] private EBuildableType buildableType;

    public GameObject Prefab { get => prefab; }

    public Sprite Sprite { get => sprite; }

    public string BuildableName {  get => buildableName; }

    public EBuildableType BuildableType { get => buildableType; }
}
