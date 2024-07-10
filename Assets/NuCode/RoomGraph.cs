using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dijkstra.NET.ShortestPath;
using Dijkstra.NET.Graph;
using Unity.VisualScripting;
using System;
using System.Net.NetworkInformation;
using System.Linq;

public class RoomGraph : MonoBehaviour
{
    // Start is called before the first frame update
    public bool IsSetupComplete { get; protected set; }
    private Graph<RoomDoorway, Room> data = new Graph<RoomDoorway, Room>();
    private Room[] rooms;
    private RoomDoorway[] doorways;
    void Start()
    {

        rooms = GetComponentsInChildren<Room>();
        doorways = GetComponentsInChildren<RoomDoorway>();

        LinkDoorsAndDoorways();
        BuildGraph();

        IsSetupComplete = true;
    }

    void BuildGraph()
    {
        // build connections list
        List<Tuple<RoomDoorway, RoomDoorway>> connections =
        new List<Tuple<RoomDoorway, RoomDoorway>>();

        for (int i = 0; i < doorways.Length; i++)
        {
            for (int j = 0; j < doorways.Length; j++)
            {
                var d1 = doorways[i];
                var d2 = doorways[j];

                if (d1.SharesRoomWith(d2) && d1 != d2)
                {
                    connections.Add(new Tuple<RoomDoorway, RoomDoorway>(
                        d1,
                        d2
                    ));
                }
            }

            // door gets added to the graph
            data.AddNode(doorways[i]);
            doorways[i].World = this;
        }

        // add connections to graph
        for (int i = 0; i < connections.Count; i++)
        {
            var conn = connections[i];
            var distance = conn.Item1.Distance2To(conn.Item2);

            conn.Item1.TryGetSharedRoom(conn.Item2, out Room shared);
            data.Connect(conn.Item1.PathfindingID, conn.Item2.PathfindingID, distance, shared);
        }
    }

    void LinkDoorsAndDoorways()
    {
        for (int i = 0; i < doorways.Length; i++)
        {
            var currentDoorway = doorways[i];

            var lookTopLeft = -Vector2.one * 0.5f + (Vector2)(currentDoorway.transform.position);
            var lookTopRight = Vector2.one * 0.5f + (Vector2)(currentDoorway.transform.position);

            var from = GetRoomAt(lookTopLeft);
            var to = GetRoomAt(lookTopRight);
            if (from == to || (currentDoorway.EntranceRoom == null || currentDoorway.ExitRoom == null))
            {
                if (currentDoorway.HasOverrideFromTo())
                {
                    currentDoorway.UseOverridenEntranceExit();
                }
                else
                {
                    throw new InvalidOperationException("Could not find rooms, no overrides set for... " + name);
                }
            }
            if (from != to && from && to)
            {
                currentDoorway.EntranceRoom = to;
                currentDoorway.ExitRoom = from;
            }
            currentDoorway.EntranceRoom.AddGateway(currentDoorway);
            currentDoorway.ExitRoom.AddGateway(currentDoorway);
        }

        // PERFORMANT ELEMENT: Faster Sorting
        var list = doorways.ToList();
        list.Sort((a, b) =>
            a.PathfindingID.CompareTo(b.PathfindingID)
        );
        doorways = list.ToArray();
    }

    public Room GetRoomAt(Vector2 where)
    {
        foreach (var room in rooms)
        {
            if (room.ContainsPoint(where))
            {
                return room;
            }
        }
        return null;
    }

    public Bounds GetMapBounds()
    {
        Bounds result = new Bounds();
        Vector3 minimum = new Vector2(float.MaxValue, float.MaxValue);
        Vector3 maximum = new Vector2(float.MinValue, float.MinValue);
        foreach (var room in rooms)
        {
            maximum = new Vector3(
                Mathf.Max(room.TopRight.x, maximum.x),
                Mathf.Max(room.TopRight.y, maximum.y),
                transform.position.z
            );
            minimum = new Vector3(
                Mathf.Min(room.BottomLeft.x, minimum.x),
                Mathf.Min(room.BottomLeft.y, minimum.y),
                transform.position.z
            );
        }
        result.SetMinMax(minimum, maximum);
        return result;
    }

    /// <summary>
    /// The main driver for pathfinding.
    /// Use whenever you want to know how to move the character.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="finish"></param>
    /// <returns></returns>
    public Vector2[] PathTo(Vector2 start, Vector2 finish)
    {
        var startRoom = GetRoomAt(start);
        var finishRoom = GetRoomAt(finish);

        if (!startRoom || !finishRoom)
        {
            return new Vector2[] {};
        }

        if (startRoom.TryGetDoorwayTo(finishRoom, out RoomDoorway doorway))
        {
            
        }

        var result = new Vector2[] { };
        return result;
    }
}
