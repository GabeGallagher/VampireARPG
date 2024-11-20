using UnityEngine;

public class Buildable : MonoBehaviour
{
    [SerializeField] private BuildableSO so;

    public BuildableSO So { get => so; }
}
