using Unity.AI.Navigation;
using UnityEngine;

public class DynamicNavMesh : MonoBehaviour
{
    public void UpdateNavMesh()
    {
        NavMeshSurface surface = GetComponent<NavMeshSurface>();
        surface.BuildNavMesh();
    }
}
