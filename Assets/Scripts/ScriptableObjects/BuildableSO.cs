using UnityEngine;

[CreateAssetMenu()]
public class BuildableSO : ScriptableObject
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private Sprite sprite;
    [SerializeField] private string buildableName;

    public GameObject Prefab { get => prefab; }

    public Sprite Sprite { get => sprite; }

    public string BuildableName {  get => buildableName; }
}
