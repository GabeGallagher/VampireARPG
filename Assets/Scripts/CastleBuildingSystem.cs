using System.Runtime.CompilerServices;
using UnityEngine;

public class CastleBuildingSystem : MonoBehaviour
{
    [SerializeField] private InputController inputController;

    [SerializeField] private GameObject floor, wall;

    private Grid<GridObject> grid;

    private float cellSize;

    private bool inBuildMode = false;

    private void Awake()
    {
        int gridWidth = 10;
        int gridHeight = 10;
        cellSize = floor.transform.localScale.x;
        grid = new Grid<GridObject>(gridWidth, gridHeight, 10, cellSize, FloorGridPosition(), (Grid<GridObject> g, int x, int z) => new GridObject(g, x, z), true, Color.black);
    }

    private void Start()
    {
        inputController.OnToggleBuildMode += InputController_OnToggleBuildMode;
    }

    private void InputController_OnToggleBuildMode(object sender, System.EventArgs e)
    {
        inBuildMode = !inBuildMode;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && inBuildMode)
        {
            GetFloorPosition(GetMouseWorldPosition(), out float x, out float z);

            Instantiate(wall, new Vector3(x, 0, z), Quaternion.identity);
        }
    }
    private void GetFloorPosition(Vector3 worldPosition, out float x, out float z)
    {
        GetVisualOffset(wall.transform, out float offsetX, out float offsetZ);

        float cellSnap = cellSize / 2;
        Vector3 snapWorldPosition = worldPosition + new Vector3(-cellSnap, 0f, -cellSnap);

        grid.GetRoundedFloatCoordinates(snapWorldPosition, out x, out z);

        x = x * cellSize + cellSnap + offsetX;
        z = z * cellSize + cellSnap + offsetZ;
    }

    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out  RaycastHit hitInfo))
        {
            return hitInfo.point;
        }
        else
        {
            return Vector3.zero;
        }
    }

    private Vector3 FloorGridPosition()
    {
        Vector3 floorCenter = floor.transform.position;

        float halfWidth = floor.transform.localScale.x * 5;
        float halfHeight = floor.transform.localScale.z * 5;

        return floorCenter - new Vector3(halfWidth, 0, halfHeight);
    }

    private void GetVisualOffset(Transform placeable, out float x, out float z)
    {
        x = -placeable.GetChild(0).position.x;
        z = -placeable.GetChild(0).position.z;
    }

    public class GridObject
    {
        private Grid<GridObject> grid;
        private int x;
        private int z;

        public GridObject(Grid<GridObject> grid, int x, int z)
        {
            this.grid = grid;
            this.x = x;
            this.z = z;
        }

        public override string ToString()
        {
            return $"{x}, {z}";
        }
    }
}
