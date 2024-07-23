using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Room : MonoBehaviour, IEquatable<Room>
{
    internal enum Side
    {
        Left,
        Right,
        Top,
        Bottom
    }

    [SerializeField]
    private Vector2 uvX;
    [SerializeField]
    private Vector2 uvY;
    [SerializeField]
    private Vector2Int extents;
    public Vector2 BottomLeft => transform.position;
    // only affected by X
    public Vector2 BottomRight => new Vector2(transform.position.x + (uvX.x * extents.x), transform.position.y + uvX.y);
    // only affected by Y
    public Vector2 TopLeft => new Vector2(transform.position.x + (uvY.x * extents.y), transform.position.y + (uvY.y * extents.y));
    public Vector2 TopRight => new Vector2(transform.position.x + (uvX.x * extents.x) + (uvY.x * extents.y),
                                           transform.position.y + (uvY.y * extents.y) + uvX.y /* not broken (yet)*/);
    private Vector2 offset;
    private HashSet<RoomDoorway> doorways = new HashSet<RoomDoorway>();

    private Matrix4x4 transformation;

    public RoomGraph World { get; set; }


    // Start is called before the first frame update
    void Start()
    {
        World = GetComponentInParent<RoomGraph>();
    }

    public RoomDoorway[] GetDoorways()
    {
        return doorways.ToArray();
    }

    void Awake()
    {
        // uvX's Y coordinate doesn't hold any weight.
        // uvY's X coordinate only holds 1x weight regardless of extents.
        transformation = new Matrix4x4(
            new(uvX.x * extents.x, uvX.y * extents.x, 0f, 0f),
            new(uvY.x * extents.y, uvY.y * extents.y, 0f, 0f),
            new(0f, 0f, 1f, 0f),
            new(0f, 0f, 0f, 1f)
        );
        offset = transform.position;
    }
    /// <summary>
    /// If a doorway is shared between `this` and `adjoining`, return it.
    /// </summary>
    /// <param name="adjoining"></param>
    /// <param name="gateway"></param>
    /// <returns></returns>
    public bool TryGetDoorwayTo(Room adjoining, out RoomDoorway doorway)
    {
        doorway = null;
        // test all gates of mine, and see if it works...
        foreach (var myGate in doorways)
        {
            if (myGate.EntranceRoom == adjoining ||
                myGate.ExitRoom == adjoining)
            {
                doorway = myGate;
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Navigate start to finish within the same room.
    /// 
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="alignAxes">make true if navigating to a door. false if not.</param>
    /// <returns></returns>
    public Vector2[] GetInteriorPathFrom(Vector2 start, Vector2 end, bool alignAxes=false)
    {
        // TODO: A* Pathfinding within a room if there's time.

        alignAxes = false;

        if (alignAxes)
        {
            // \
            //  \
            //   \
            //    ---------
            // always 45 degree angle
            var destination = ClampGlobal(end);
            
            var diff = destination - start;
            var applyDiff = Vector2.zero;
            
            // always max out the shortest dimension first.
            if (diff.x < diff.y)
            {
                applyDiff = Vector2.right * diff.x;
            }
            else
            {
                applyDiff = Vector2.up * diff.y;
            }
            var midpoint = ClampGlobal(start + applyDiff);
            return new Vector2[] { midpoint, destination };
        }
        else
        {
            // --
            //   \
            //     --
            //        \
            //          --
            var destination = ClampGlobal(end);
            return new Vector2[] { destination };
        }
    }

    internal Room.Side WhichSide(Vector2 globalPosition)
    {
        var position = GlobalToLocalNoClamp(globalPosition);
        var middle = Vector2.one / 2f;
        var diff = position - middle;
        var diffAbs = new Vector2(Mathf.Abs(diff.x), Mathf.Abs(diff.y));
        if (diffAbs.x > diffAbs.y)
        {
            // positive diff = right
            return diff.x >= 0f ? Side.Right : Side.Left; 
        }
        else
        {
            // positive diff = up
            return diff.y >= 0f ? Side.Top : Side.Bottom;
        }
    }
    /// <summary>
    /// Gets a point on the room plane that's closest to the provided one
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    public Vector2 ClosestBorderPointTo(Vector2 point)
    {
        var side = WhichSide(point);
        var lineU = Vector2.zero;
        var lineV = Vector2.zero;
        switch (side)
        {
            case Side.Left:
                lineU = BottomLeft;
                lineV = TopLeft;
            break;
            case Side.Right:
                lineU = BottomRight;
                lineV = TopRight;
            break;
            case Side.Top:
                lineU = TopLeft;
                lineV = TopRight;
            break;
            case Side.Bottom:
                lineU = BottomLeft;
                lineV = BottomRight;
            break;
        }
        var len2 = (lineV - lineU).sqrMagnitude;
        if (len2 == 0)
        {
            return point;
        }
        var parameterT = Mathf.Clamp01(Vector2.Dot(point - lineU, lineV - lineU) / len2); 
        var projection = Vector2.Lerp(lineU, lineV, parameterT);
        return projection;
    }

    public void AddDoorway(RoomDoorway toAdd)
    {
        doorways.Add(toAdd);
    }

    public Vector2 ClampLocal(Vector2 localCoordinates)
    {
        return new Vector2(
            Mathf.Clamp(localCoordinates.x, 0f, 1f),
            Mathf.Clamp(localCoordinates.y, 0f, 1f)
        );
    }

    public bool ContainsPoint(Vector2 globalCoordinates)
    {
        var clamped = ClampGlobal(globalCoordinates);
        return ApproximatelyEqualsDelta(clamped.x, globalCoordinates.x, 0.01f) &&
            ApproximatelyEqualsDelta(clamped.y, globalCoordinates.y, 0.01f);
    }

    private bool ApproximatelyEqualsDelta(float a, float b, float delta)
    {
        return Mathf.Abs(a - b) < delta;
    }

    public Vector2 ClampGlobal(Vector2 globalCoordinates)
    {
        return LocalToGlobal(GlobalToLocal(globalCoordinates));
    }

    public Vector2 GlobalToLocal(Vector2 GlobalCoordinates)
    {
        var result = transformation.inverse * (GlobalCoordinates - offset);
        return ClampLocal(result);
    }

    private Vector2 GlobalToLocalNoClamp(Vector2 GlobalCoordinates)
    {
        var result = transformation.inverse * (GlobalCoordinates - offset);
        return result;
    }

    public Vector2 LocalToGlobal(Vector2 worldCoordinates)
    {
        var result = transformation * ClampLocal(worldCoordinates);
        return (Vector2)result + offset;
    }

    public Vector2Int LocalToGrid(Vector2 localCoordinates)
    {
        return new Vector2Int(
            Mathf.RoundToInt(localCoordinates.x * extents.x),
            Mathf.RoundToInt(localCoordinates.y * extents.y)
        );
    }
    public Vector2 GridToLocal(Vector2Int gridCoordinates)
    {
        return new Vector2(
            gridCoordinates.x / (float)extents.x,
            gridCoordinates.y / (float)extents.y
        );
    }
    void OnDrawGizmos()
    {
        // draw X
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, new(transform.position.x + uvX.x, transform.position.y + uvX.y, 0));
        // draw Y
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, new(transform.position.x + uvY.x, transform.position.y + uvY.y, 0));

        // draw parallelogram
        Gizmos.color = new Color(Color.magenta.r, Color.magenta.g, 0.5f, 0.3f);
        Vector3[] points = new Vector3[4]
        {
                new Vector3(BottomLeft.x, BottomLeft.y, 0f),
                new Vector3(BottomRight.x, BottomRight.y, 0f),
                new Vector3(TopRight.x, TopRight.y, 0f),
                new Vector3(TopLeft.x, TopLeft.y, 0f)
        };
        Gizmos.DrawLineStrip(points, true);
    }

    public bool Equals(Room other)
    {
        return this == other;
    }
}
