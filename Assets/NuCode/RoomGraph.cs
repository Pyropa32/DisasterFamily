using System.Collections.Generic;
using UnityEngine;
using Dijkstra.NET.ShortestPath;
using Dijkstra.NET.Graph;
using System;
using System.Linq;
using UnityEditor.ProjectWindowCallback;

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
            currentDoorway.EntranceRoom.AddDoorway(currentDoorway);
            currentDoorway.ExitRoom.AddDoorway(currentDoorway);
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
    /// Get the path from point A to point B across many rooms.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="finish"></param>
    /// <returns>The shortest path linking the room at point A to the room at point B</returns>
    public Vector2[] GetExteriorPathFrom(Vector2 start, Vector2 finish)
    {
        var startRoom = GetRoomAt(start);
        var finishRoom = GetRoomAt(finish);

        if (!startRoom || !finishRoom)
        {
            Debug.Log("Attempted to find exterior path, but one or both rooms are null!");
            return new Vector2[] { };
        }
        // Trivial Pathfinding: Adjacent room
        if (startRoom.TryGetDoorwayTo(finishRoom, out RoomDoorway doorway))
        {
            // always 2 elements.
            var transferRoomPath = doorway.GetTransferRoomPathFrom(start);
            var pathA = startRoom.GetInteriorPathFrom(start, transferRoomPath[0], alignAxes: true);
            var pathB = finishRoom.GetInteriorPathFrom(transferRoomPath[1], finish, alignAxes: true);
            return PathHelper.CombinePaths(pathA, transferRoomPath, pathB);
        }

        // otherwise, do pathfinding.
        var startRoomDoorways = startRoom.GetDoorways();
        var finishRoomDoorways = finishRoom.GetDoorways();

        // If the start/end rooms have multiple doorways each,
        // each one has to be considered.

        List<ShortestPathResult> sprList = new List<ShortestPathResult>();

        for (int i = 0; i < startRoomDoorways.Length; i++)
        {
            for (int j = 0; j < finishRoomDoorways.Length; j++)
            {
                var from = startRoomDoorways[i];
                var to = finishRoomDoorways[j];
                // The doorways should not be adjacent here.

                var path = data.Dijkstra(from.PathfindingID, to.PathfindingID);
                sprList.Add(path);
            }
        }

        // delete the paths that weren't found:
        sprList.RemoveAll(path => path.IsFounded == false);

        if (sprList.Count < 1)
        {
            Debug.Log("no paths found!");
            return new Vector2[] {};
        }
        // // Sort the results by distance
        // sprList.Sort((a, b) =>
        // {
        //     return a.Distance.CompareTo(b.Distance);
        // });

        // // Hyper-Inneficiencah!!
        var paths = sprList.ConvertAll(item => item.GetPath().ToList());

        // sort the results by path size
        paths.Sort((a,b) =>
        {
            return a.Count.CompareTo(b.Count);
        });

        var doorwayIDList = paths[0];

        Room currentRoom = startRoom;
        Room nextRoom = finishRoom;
        RoomDoorway currentDoorway = data[doorwayIDList[0]].Item;
        List<Vector2> points = new List<Vector2>();

        Vector2 currentPos = start;

        //
        //  starting point to first doorway
        //

        var firstPath = startRoom.GetInteriorPathFrom(start, currentDoorway.transform.position, alignAxes:true); 
        points.AddRange(firstPath);
        Debug.Log("big path ordering summary:");
        Debug.Log("START POS: " + start);
        Debug.Log("FINISH POS: " + finish);
        //

        for (int i = 0; i < doorwayIDList.Count; i++)
        {
            currentDoorway = data[doorwayIDList[i]].Item;
            Debug.Log("navigating doorway called:" + currentDoorway.name);
            if (i == doorwayIDList.Count - 1)
            {
                // cap off with the end room.
                nextRoom = finishRoom;
            }
            else
            {
                if (currentRoom == currentDoorway.EntranceRoom)
                {
                    nextRoom = currentDoorway.ExitRoom;
                }
                else
                {
                    nextRoom = currentDoorway.EntranceRoom;
                }
            }

            //  doorway transfer path
            var doorwayTransferPath = currentDoorway.GetTransferRoomPathFrom(currentPos);
            foreach (var p in doorwayTransferPath)
            {
                Debug.Log("DoorTransfer" + p);
            }
            
            //  current position to next doorway path
            var currentPosToDoorwayPath = currentRoom.GetInteriorPathFrom(currentPos, doorwayTransferPath[0], alignAxes:true); 
            foreach (var p in currentPosToDoorwayPath)
            {
                Debug.Log("Pos2Doorway" + p);
            }

            // combine and add these paths
            var bothPaths = PathHelper.CombinePaths(currentPosToDoorwayPath, doorwayTransferPath);
            points.AddRange(bothPaths);

            currentRoom = nextRoom;
            // current pos is now the latest point (other side of the transfer)
            currentPos = doorwayTransferPath[1];
        }

        //
        //  last doorway to end point.
        //
        var lastPath = finishRoom.GetInteriorPathFrom(currentDoorway.transform.position, finish, alignAxes:true); 
        points.AddRange(lastPath);

        foreach (var p in points)
        {
            Debug.Log(p);
        }

        return points.ToArray();
    }
}
