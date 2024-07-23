using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using UnityEngine;

public class RoomDoorway : MonoBehaviour
{
    [SerializeField]
    Room entranceOverride;
    [SerializeField]
    Room exitOverride;
    public Room EntranceRoom { get; set; }
    public Room ExitRoom { get; set; }

    public uint PathfindingID { get; protected set; }
    public RoomGraph World { get; set; }

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
    /// </summary>
    /// <returns>2 points: #1 is point closest to doorway on Exit room, #2 is point closest to doorway on Entrance room.</returns>
    public Vector2[] GetTransferRoomPath()
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
            pointB = ExitRoom.ClosestBorderPointTo(transform.position);
        }
        return new Vector2[2]
        {
            pointA,
            pointB
        };
    }

    void Awake()
    {
        PathfindingID = (uint)GetHashCode();
        World = GetComponentInParent<RoomGraph>();
        if (World == null)
        {
            throw new InvalidOperationException("Doorway not a part of a room graph!");
        }
    }
}
