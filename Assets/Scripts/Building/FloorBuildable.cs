using UnityEngine;

public class FloorBuildable : Buildable
{
    public enum EBuildableEdge
    {
        Up, Down, Left, Right
    }

    private GameObject upWall, downWall, leftWall, rightWall;

    public bool HasEdge(EBuildableEdge edge)
    {
        switch(edge)
        {
            case EBuildableEdge.Up:
                if (upWall != null)
                {
                    return true;
                }
                return false;

            case EBuildableEdge.Down:
                if (downWall != null)
                {
                    return true;
                }
                return false;

            case EBuildableEdge.Left:
                if (leftWall != null)
                {
                    return true;
                }
                return false;

            case EBuildableEdge.Right:
                if (rightWall != null)
                {
                    return true;
                }
                return false;

            default:
                Debug.LogError($"No buildable with edge of type: {edge}");
                break;
        }
        return false;
    }

    public void SetEdge(GameObject wall, EBuildableEdge edge)
    {
        switch (edge)
        {
            case EBuildableEdge.Up:
                upWall = wall;
                break;

            case EBuildableEdge.Down:
                downWall = wall;
                break;

            case EBuildableEdge.Left:
                leftWall = wall;
                break;

            case EBuildableEdge.Right:
                rightWall = wall;
                break;

            default:
                Debug.LogError($"No buildable edge for object {wall}");
                break;
        }
    }
}
