using UnityEngine;

public class CastleBuildingSystem : MonoBehaviour
{
    [SerializeField] private GameObject floor;

    private Grid<GridObject> grid;

    private void Awake()
    {
        int gridWidth = 10;
        int gridHeight = 10;
        float cellSize = floor.transform.localScale.x;
        grid = new Grid<GridObject>(gridWidth, gridHeight, 10, cellSize, FloorGridPosition(), (Grid<GridObject> g, int x, int z) => new GridObject(g, x, z), true, Color.black);
    }

    private Vector3 FloorGridPosition()
    {
        Vector3 floorCenter = floor.transform.position;

        float halfWidth = floor.transform.localScale.x * 5;
        float halfHeight = floor.transform.localScale.z * 5;

        return floorCenter - new Vector3(halfWidth, 0, halfHeight);
    }

    private float CellSize(int width, int height)
    {
        float floorScale = floor.transform.localScale.x;

        if (width > height)
        {
            return floorScale / width;
        }
        else
        {
            return floorScale / height;
        }
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
