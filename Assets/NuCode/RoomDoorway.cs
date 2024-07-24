using System;
using UnityEngine;

public class RoomDoorway : MonoBehaviour
{
    private const float SPAZ_REMOVAL_TOLERANCE = 0.2f;
    [SerializeField]
    Room entranceOverride;
    [SerializeField]
    Room exitOverride;
    public Room EntranceRoom { get; set; }
    public Room ExitRoom { get; set; }

    public uint PathfindingID { get; protected set; }
    public RoomGraph World { get; set; }

    private static uint currID = 1;

    public static void resetStatic() {
        currID = 1;
    }

    void Start()
    {
        var sprite = GetComponent<SpriteRenderer>();
        if (sprite != null)
        {
            Destroy(sprite);
        }
    }

    public void UseOverridenEntranceExit()
    {
        if (!HasOverrideFromTo())
        {
            Debug.LogError("Could not detect To/From planes, and no override planes present in " + name + "!");
        }
        EntranceRoom = entranceOverride;
        ExitRoom = exitOverride;
    }

    public bool SharesRoomWith(RoomDoorway other)
    {
        return TryGetSharedRoom(other, out _);
    }

    public bool TryGetSharedRoom(RoomDoorway other, out Room result)
    {
        if (other.EntranceRoom == EntranceRoom ||
                other.EntranceRoom == ExitRoom)
        {
            result = other.EntranceRoom;
        }
        else if (other.ExitRoom == ExitRoom ||
                other.ExitRoom == EntranceRoom)
        {
            result = other.ExitRoom;
        }
        else
        {
            result = null;
            return false;
        }
        return true;
    }
    public int Distance2To(RoomDoorway what)
    {
        return (int)((transform.position.x - what.transform.position.x) * (transform.position.x - what.transform.position.x) +
                (transform.position.y - what.transform.position.y) * (transform.position.y - what.transform.position.y));
    }
    public bool HasOverrideFromTo()
    {
        return entranceOverride != null && exitOverride != null;
    }

    /// <summary>
    /// Gets the path from the edge of the Entrance room and the edge of the Exit room.
    /// uses startPosition to align the path properly.
    /// </summary>
    /// <returns>2 points: #1 is point closest to doorway on Exit room, #2 is point closest to doorway on Entrance room.</returns>
    public Vector2[] GetTransferRoomPathFrom(Vector2 startPosition)
    {
        // If point is in plane, make no change 
        var pointA = transform.position;
        var pointB = transform.position;
        // If out of bounds, snap back in bounds.
        if (!ExitRoom.ContainsPoint(transform.position))
        {
            pointA = ExitRoom.ClosestBorderPointTo(transform.position);
        }
        if (!EntranceRoom.ContainsPoint(transform.position))
        {
            pointB = EntranceRoom.ClosestBorderPointTo(transform.position);
        }
        var dist1 = ((Vector3)startPosition - pointA).sqrMagnitude;
        var dist2 = ((Vector3)startPosition - pointB).sqrMagnitude;
        if (Math.Abs(dist2 - dist1) < SPAZ_REMOVAL_TOLERANCE)
        {
            // if two points are very close
            // and to avoid conflict, just return 2 copies of the same point.
            return new Vector2[2]
            {
                pointA,
                pointA
            };
        }
        if (dist1 > dist2)
        {
            // if point b is closer...
            return new Vector2[2]
            {
                pointB,
                pointA
            };
        }
        else
        {
            // to prevent minor spazzing, don't include 2 really close points.
            // if point a is closer...
            return new Vector2[2]
            {
                pointA,
                pointB
            };

        }
    }

    void Awake()
    {
        PathfindingID = currID++;
        World = GetComponentInParent<RoomGraph>();
        if (World == null)
        {
            throw new InvalidOperationException("Doorway not a part of a room graph!");
        }
    }
}
