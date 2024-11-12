using System;
using Unity.Mathematics;
using UnityEngine;

public class Grid<TGridObject>
{
    public event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged;
    public class OnGridValueChangedEventArgs : EventArgs
    {
        public int x; public int z;
    }
    public bool showDebug;

    private Vector3 originPosition;
    private Vector3 floorPosition;
    private Color color;
    private int width;
    private int height;
    private int fontSize;
    private float cellSize;
    private TGridObject[,] gridArray;

    private const int sortingOrderDefault = 5000;

    public Grid(int width, int height, int fontSize, float cellSize, Vector3 originPosition, Func<Grid<TGridObject>, int, int, TGridObject> createGridObject, bool showDebug = false, Color? color = null)
    {
        this.width = width;
        this.height = height;
        this.fontSize = fontSize;
        this.cellSize = cellSize;
        this.originPosition = originPosition;
        this.showDebug = showDebug;

        if (color == null) this.color = Color.white;
        else this.color = (Color)color;

        gridArray = new TGridObject[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int z = 0; z < gridArray.GetLength(1); z++)
            {
                gridArray[x, z] = createGridObject(this, x, z);
            }
        }

        if (showDebug)
        {
            TextMesh[,] debugTextArray = new TextMesh[width, height];

            GameObject textParent = new GameObject("World_Text_Container");

            textParent.transform.position = Vector3.zero;

            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int z = 0; z < gridArray.GetLength(1); z++)
                {
                    debugTextArray[x, z] = CreateWorldText(gridArray[x, z]?.ToString(), textParent.transform, fontSize, GetWorldPosition(x, z) + new Vector3(cellSize, 0, cellSize) * .5f, this.color, TextAnchor.MiddleCenter);
                    DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z + 1), textParent.transform, this.color);
                    DrawLine(GetWorldPosition(x, z), GetWorldPosition(x + 1, z), textParent.transform, this.color);
                }
            }
            DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), textParent.transform, this.color);
            DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), textParent.transform, this.color);

            OnGridValueChanged += (object sender, OnGridValueChangedEventArgs eventArgs) =>
            {
                debugTextArray[eventArgs.x, eventArgs.z].text = gridArray[eventArgs.x, eventArgs.z]?.ToString();
            };
        }
    }

    public int X { get; }

    public int Z { get; }

    public float CellSize { get; }

    public void SetValue(int x, int z, TGridObject value)
    {
        if (x >= 0 && z >= 0 && x < width && z < height)
        {
            gridArray[x, z] = value;
            OnGridValueChanged(this, new OnGridValueChangedEventArgs { x = x, z = z });
        }
        else Debug.Log($"{value} is an invalid value");
    }

    public void SetValue(Vector3 worldPosition, TGridObject value)
    {
        int x, z;

        GetCoordinates(worldPosition, out x, out z);

        SetValue(x, z, value);
    }

    public TGridObject GetValue(int x, int z)
    {
        if (x >= 0 && z >= 0 && x < width && z < height)
        {
            return gridArray[x, z];
        }
        else
        {
            return default(TGridObject);
        }
    }

    public TGridObject GetValue(Vector3 worldPosition)
    {
        int x, z;
        GetCoordinates(worldPosition, out x, out z);
        return GetValue(x, z);
    }

    private TextMesh CreateWorldText(string text, Transform parent, int fontSize, Vector3 localPosition = default(Vector3), Color? color = null, TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = sortingOrderDefault)
    {
        if (color == null) color = Color.white;
        return CreateWorldText(parent, text, localPosition, fontSize, (Color)color, textAnchor, textAlignment, sortingOrder);
    }

    private void DrawLine(Vector3 start, Vector3 end, Transform parent, Color color)
    {
        GameObject lineObj = new GameObject("GridLine");
        LineRenderer lineRenderer = lineObj.AddComponent<LineRenderer>();

        float width = 0.25f;
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
        lineRenderer.material = new Material(Shader.Find("Unlit/Color"));
        lineRenderer.material.color = color;

        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);

        lineRenderer.transform.parent = parent;
    }

    private TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder)
    {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        return textMesh;
    }

    public Vector3 GetWorldPosition(int x, int z)
    {
        return new Vector3(x, 0, z) * cellSize + originPosition;
    }

    public void GetCoordinates(Vector3 worldPosition, out int x, out int z)
    {
        x = Mathf.FloorToInt(worldPosition.x / cellSize);
        z = Mathf.FloorToInt(worldPosition.z / cellSize);
    }

    public void GetRoundedFloatCoordinates(Vector3 worldPosition, out float x, out float z)
    {
        x = Mathf.Round(worldPosition.x / cellSize);
        z = Mathf.Round(worldPosition.z / cellSize);
    }
}
