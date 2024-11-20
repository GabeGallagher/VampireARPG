using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static CastleBuildingSystem;

public class CastleBuildingSystem : MonoBehaviour
{
    [SerializeField] private InputController inputController;

    [SerializeField] private GameObject floor;

    [SerializeField] private Material canPlaceMaterial, cantPlaceMaterial;

    private Grid<GridObject> grid;

    private GridObject gridObject;

    private GameObject previewObject, placeablePrefab;

    private Renderer previewRenderer;

    private float cellSize;

    private bool inBuildMode = false;

    private void Awake()
    {
        int gridWidth = 10;
        int gridHeight = 10;
        cellSize = floor.transform.localScale.x;
        grid = new Grid<GridObject>(gridWidth, gridHeight, 10, cellSize, FloorGridPosition(), (Grid<GridObject> g, int x, int z) => new GridObject(g, x, z), false, Color.black);
    }

    private void Start()
    {
        inputController.OnToggleBuildMode += InputController_OnToggleBuildMode;
    }

    public GameObject PlaceablePrefab { get => placeablePrefab; set => SetPlaceablePrefab(value); }

    private void InputController_OnToggleBuildMode(object sender, System.EventArgs e)
    {
        inBuildMode = !inBuildMode;

        if (!inBuildMode && previewObject != null)
        {
            Destroy(previewObject);
        }

        if (!inBuildMode)
        {
            DynamicNavMesh navMesh = floor.transform.parent.GetComponent<DynamicNavMesh>();

            navMesh.UpdateNavMesh();
        }
    }

    private void Update()
    {
        if (inBuildMode && placeablePrefab != null)
        {
            GetFloorPosition(GetMouseWorldPosition(), out float x, out float z);

            gridObject = grid.GetValue(GetMouseWorldPosition());

            if (previewObject == null)
            {
                previewObject = Instantiate(placeablePrefab, new Vector3(x, 0, z), Quaternion.identity);
            }
            previewRenderer = previewObject.GetComponentInChildren<Renderer>();

            if (previewObject.GetComponent<Buildable>().So.BuildableType != BuildableSO.EBuildableType.Wall)
            {
                previewObject.transform.position = new Vector3(x, 0, z);
            }

            if (CanBuild(placeablePrefab.GetComponent<Buildable>().So, gridObject))
            {
                previewRenderer.material = canPlaceMaterial;
            }
            else
            {
                previewRenderer.material = cantPlaceMaterial;
            }

            if (CanBuild(placeablePrefab.GetComponent<Buildable>().So, gridObject) && Input.GetMouseButtonDown(0) && !inputController.IsPointerOverUIElement())
            {
                Build(placeablePrefab, new Vector3(x, 0, z));
            }
        }
    }

    private void Build(GameObject placeablePrefab, Vector3 location)
    {
        BuildableSO so = placeablePrefab.GetComponent<Buildable>().So;

        switch (so.BuildableType)
        {
            case BuildableSO.EBuildableType.Floor:
                GameObject buildableFloor = Instantiate(placeablePrefab, location, Quaternion.identity);
                gridObject.PlacedObject = buildableFloor.transform;
                break;

            case BuildableSO.EBuildableType.Wall:
                GameObject wall = Instantiate(placeablePrefab, location, Quaternion.identity);
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                int floorLayerMask = LayerMask.GetMask("Floor");
                if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, floorLayerMask))
                {
                    wall.transform.position = SetWallPosition(hitInfo, wall, out FloorBuildable.EBuildableEdge edge);
                    Transform parentFloor = hitInfo.collider.gameObject.transform.parent;
                    FloorBuildable floorBuildable = parentFloor.GetComponent<FloorBuildable>();
                    floorBuildable.SetEdge(wall, edge);
                }
                break;

        }
    }

    private bool CanBuild(BuildableSO buildableSO, GridObject gridObject)
    {
        switch (buildableSO.BuildableType)
        {
            case BuildableSO.EBuildableType.Floor:
                return gridObject.CanBuild();

            case BuildableSO.EBuildableType.Wall:
                return CanBuildWall(gridObject);

            case BuildableSO.EBuildableType.Desk:
                if (gridObject.PlacedObject != null)
                {
                    BuildableSO so = gridObject.PlacedObject.GetComponent<Buildable>().So;
                    if (so.BuildableType == BuildableSO.EBuildableType.Floor)
                    {
                        return true;
                    }
                }
                return false;

            default:
                Debug.Log($"{buildableSO.BuildableType} unhandled in CanBuild(BuildableSO {buildableSO}, GridObject {gridObject}");
                return false;
        }
    }

    private bool CanBuildWall(GridObject gridObject)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        int floorLayerMask = LayerMask.GetMask("Floor");

        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, floorLayerMask))
        {
            GameObject buildableFloor = hitInfo.collider.gameObject;

            if (buildableFloor.layer == LayerMask.NameToLayer("Floor"))
            {
                previewObject.transform.position = SetWallPosition(hitInfo, previewObject, out FloorBuildable.EBuildableEdge edge);

                FloorBuildable floorBuildable = buildableFloor.transform.parent.GetComponent<FloorBuildable>();

                if (floorBuildable.HasEdge(edge))
                {
                    return false;
                }
                return true;
            }
        }
        return false;
    }

    private Vector3 SetWallPosition(RaycastHit hitInfo, GameObject wall, out FloorBuildable.EBuildableEdge edge)
    {
        Bounds floorBounds = hitInfo.collider.bounds;

        Vector3 floorCenter = floorBounds.center;

        float extentX = floorBounds.extents.x;
        float extentZ = floorBounds.extents.z;

        Vector3 direction = hitInfo.point - floorCenter;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.z))
        {
            wall.transform.rotation = Quaternion.Euler(0, 90, 0);

            if (direction.x > 0)
            {
                edge = FloorBuildable.EBuildableEdge.Right;
                return new Vector3(floorCenter.x + extentX, floorCenter.y, floorCenter.z + extentZ);
            }
            else
            {
                edge = FloorBuildable.EBuildableEdge.Left;
                return new Vector3(floorCenter.x - extentX, floorCenter.y, floorCenter.z + extentZ);
            }
        }
        else
        {
            wall.transform.rotation = Quaternion.Euler(0, 0, 0);

            if (direction.z > 0)
            {
                edge = FloorBuildable.EBuildableEdge.Up;
                return new Vector3(floorCenter.x - extentX, floorCenter.y, floorCenter.z + extentZ);
            }
            else
            {
                edge = FloorBuildable.EBuildableEdge.Down;
                return new Vector3(floorCenter.x - extentX, floorCenter.y, floorCenter.z - extentZ);
            }
        }
    }

    private void SetPlaceablePrefab(GameObject newPrefab)
    {
        if (placeablePrefab != null)
        {
            Destroy(previewObject);
        }
        placeablePrefab = newPrefab;
    }

    private void GetFloorPosition(Vector3 worldPosition, out float x, out float z)
    {
        GetVisualOffset(placeablePrefab.transform, out float offsetX, out float offsetZ);

        float cellSnap = cellSize / 2;
        Vector3 snapWorldPosition = worldPosition + new Vector3(-cellSnap, 0f, -cellSnap);

        grid.GetRoundedFloatCoordinates(snapWorldPosition, out x, out z);

        x = x * cellSize;
        z = z * cellSize;
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
        private Transform placedObject;
        private List<Transform> placedObjectList;

        public GridObject(Grid<GridObject> grid, int x, int z)
        {
            this.grid = grid;
            this.x = x;
            this.z = z;
        }

        public Transform PlacedObject
        {
            get => placedObject; 
            set 
            {
                placedObject = value;

                grid.TriggerGridObjectChange(x, z);
            }
        }

        public int X { get => x; }

        public int Z { get => z; }

        public List<Transform> PlacedObjectList { get => placedObjectList; }

        public void AddToPlacedObjectsList(Transform placedObject)
        {
            placedObjectList.Add(placedObject);
        }

        public void ClearObject()
        {
            placedObject = null;

            grid.TriggerGridObjectChange(x, z);
        }

        public Boolean CanBuild()
        {
            return placedObject == null;
        }

        public override string ToString()
        {
            return $"{x}, {z} {placedObject}";
        }
    }
}
