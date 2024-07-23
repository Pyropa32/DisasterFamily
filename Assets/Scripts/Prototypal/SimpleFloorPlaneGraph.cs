using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Dijkstra.NET.ShortestPath;
using Dijkstra.NET.Graph;
using UnityEngine.U2D;

namespace Prototypal
{
    public class SimpleFloorPlaneGraph : ThrowawayPrototypeCode
    {
        // Start is called before the first frame update
        // Planes are considered edges for a good reason:
        //  You may need to pass through the same room again via a different gate to get somewhere.
        //  But a path will never lead you through the same gate twice.
        bool hasStarted = false;
        public bool HasStarted => hasStarted;
        private Graph<SimpleFloorPlaneGateway, SimpleFloorPlane> graph = new Graph<SimpleFloorPlaneGateway, SimpleFloorPlane>();
        private List<SimpleFloorPlane> planes = new List<SimpleFloorPlane>();
        private List<SpriteRenderer> sprites = new List<SpriteRenderer>();
        void Start()
        {
            hasStarted = true;
            // add sprites to sorting list (except the black square ones bound to the main camera + anything UI)
            sprites.AddRange(GetComponentsInChildren<SpriteRenderer>().Where(sprite => sprite.tag != "DoNotSort"));


            // TODO: Get all of my child SimpleFloorPlanes and 
            // create an adjacency list that links them.
            // The list will be used for External pathfinding.
            SimpleFloorPlaneGateway[] allGatesArray = GetComponentsInChildren<SimpleFloorPlaneGateway>();
            planes.AddRange(GetComponentsInChildren<SimpleFloorPlane>());

            // FIXME: Find better alternative to add to the graph without sorting
            var allGates = allGatesArray.ToList();

            foreach (var gate in allGates)
            {
                // attempt to get the planes next to the gate by 'looking' left and right / up and down
                var lookTopLeft = -Vector2.one * 0.5f + (Vector2)(gate.transform.position);
                var lookTopRight = Vector2.one * 0.5f + (Vector2)(gate.transform.position);

                var from = this.GetPlaneByPosition(lookTopLeft);
                var to = this.GetPlaneByPosition(lookTopRight);

                if (from == to)
                {
                    if (gate.HasOverrideFromTo())
                    {
                        gate.SetFromToWithOverrides();
                    }
                    else
                    {
                        throw new InvalidOperationException("gate placed wrongly and without overrides: " + gate.name);
                    }
                }
                if (gate.FromPlane == null || gate.ToPlane == null)
                {
                    if (gate.HasOverrideFromTo())
                    {
                        gate.SetFromToWithOverrides();
                    }
                    else
                    {
                        throw new InvalidOperationException("gate named: " + gate.name + " does not have 2 destinations and without overrides!");
                    }
                }
                if (from != to && from != null && to != null)
                {
                    gate.ToPlane = to;
                    gate.FromPlane = from;
                }
                gate.FromPlane.AddGateway(gate);
                gate.ToPlane.AddGateway(gate);
            }

            allGates.Sort((a, b) =>
                a.ID.CompareTo(b.ID)
            );
            // sort and add to graph.


            // populate all planes
            // create connections.
            List<Tuple<SimpleFloorPlaneGateway, SimpleFloorPlaneGateway>> connections =
            new List<Tuple<SimpleFloorPlaneGateway, SimpleFloorPlaneGateway>>();
            for (int i = 0; i < allGates.Count; i++)
            {
                for (int j = 0; j < allGates.Count; j++)
                {
                    var gateway1 = allGates[i];
                    var gateway2 = allGates[j];

                    if (gateway1.SharesPlaneWith(gateway2)
                         && gateway1 != gateway2)
                    {
                        // duplicates should be okay.
                        connections.Add(new Tuple<SimpleFloorPlaneGateway, SimpleFloorPlaneGateway>(
                            gateway1,
                            gateway2
                        ));
                    }
                }

                // add the current gate to the graph
                graph.AddNode(allGates[i]);
                allGates[i].SetWorld(this);

            }
            // add gateways


            // connect all the gateways together
            for (int i = 0; i < connections.Count; i++)
            {
                var currentConnection = connections[i];

                var gateway1 = currentConnection.Item1;
                var gateway2 = currentConnection.Item2;

                var distance2 = gateway1.Distance2To(gateway2);

                // it doesnt matter if the distance is squared
                gateway1.TryGetSharedPlane(gateway2, out SimpleFloorPlane plane);
                graph.Connect(gateway1.ID, gateway2.ID, distance2, plane);
            }
        }

        public Bounds GetBounds()
        {
            Bounds result = new Bounds();
            Vector3 minimum = new Vector2(float.MaxValue, float.MaxValue);
            Vector3 maximum = new Vector2(float.MinValue, float.MinValue);
            foreach (var plane in planes)
            {
                maximum = new Vector3(
                    Mathf.Max(plane.TopRight.x, maximum.x),
                    Mathf.Max(plane.TopRight.y, maximum.y),
                    transform.position.z
                );
                minimum = new Vector3(
                    Mathf.Min(plane.BottomLeft.x, minimum.x),
                    Mathf.Min(plane.BottomLeft.y, minimum.y),
                    transform.position.z
                );
            }
            result.SetMinMax(minimum, maximum);
            return result;
        }

        public SimpleFloorPlane GetPlaneByPosition(Vector2 where)
        {
            foreach (var plane in planes)
            {
                if (plane.IsPointInRange(where))
                {
                    return plane;
                }
            }
            return null;
        }

        void Update()
        {
            SortGraphics();
        }

        private void SortGraphics()
        {
            // fuck this crap
            // waste of effort for it to not be that necessary

            // const float POSITION_INCREMENT = 0.1f;
            // // For the future: only do this if the position of something changes.
            // // sort the sprites by Y position
            // sprites.Sort((spriteA, spriteB) =>
            // {
            //     var spriteABottom = spriteA.bounds.center.y + (spriteA.bounds.extents.y / 2f);
            //     var spriteBBottom = spriteB.bounds.center.y + (spriteB.bounds.extents.y / 2f);
            //     return spriteABottom.CompareTo(spriteBBottom) + (spriteA.sortingOrder - spriteB.sortingOrder);
            // });
            // // assign the Z value;
            // for (int i = 0; i < sprites.Count; i++)
            // {
            //     var current = sprites[i].transform;
            //     var zValue = transform.position.z + (POSITION_INCREMENT * i);
            //     sprites[i].transform.position = new Vector3(current.position.x, current.position.y, zValue);       
            // }
        }

        // Eventually, this will need to be made private. The public method needs to find a path that is always usable.
        public SimplePlaneTraversalResult GetShortestExternalPath(SimpleFloorPlane start, SimpleFloorPlane end)
        {
            // Have to figure out which gates to choose.
            // May be O(m^2) where m is number of gates.
            var startGateways = start.GetGateways();
            var endGateways = end.GetGateways();

            // Generate a really simple traversal if the planes touch
            // Which is is probably 100% of the case by the end
            // and 0% of the case if a certain artist wussies out from doing 2 rooms
            if (start.TryGetAdjoiningGate(end, out SimpleFloorPlaneGateway adjoiningGate))
            {
                return new SimplePlaneTraversalResult()
                {
                    Solution = new PlaneTransferData[1] {
                        new PlaneTransferData() {
                            StartPlane = start,
                            Gate = adjoiningGate,
                            DestinationPlane = end
                    }},
                    Success = true
                };
            }

            // don't include 
            List<ShortestPathResult> results = new List<ShortestPathResult>(startGateways.Length * endGateways.Length);

            for (int i = 0; i < startGateways.Length; i++)
            {
                for (int j = 0; j < endGateways.Length; j++)
                {
                    var from = startGateways[i];
                    var to = endGateways[j];

                    if (from != to && !from.SharesPlaneWith(to))
                    {
                        var path = graph.Dijkstra(from.ID, to.ID);
                        results.Add(path);
                    }
                }
            }

            // delete the paths that weren't found:
            results.RemoveAll(path => path.IsFounded == false);

            // Sort the results by distance
            results.Sort((a, b) =>
                {
                    return a.Distance.CompareTo(b.Distance);
                }
                );

            // // For each of these paths, see if internal pathfinding can take you there.
            // foreach (var result in results)
            // {
            //     // TODO: Adjust ExternalPathfindingResult to accomodate the list of landmarks to pass through
            //     // when getting through each SimpleFloor.

            // }

            // find the smallest path
            ShortestPathResult shortestPath = results[0];
            var pathIDs = shortestPath.GetPath();
            List<SimpleFloorPlaneGateway> gateList = new List<SimpleFloorPlaneGateway>();
            foreach (var thing in pathIDs)
            {
                gateList.Add(graph[thing].Item);
            }

            if (gateList.Count < 1)
            {
                ExternalPathfindingResult result = new ExternalPathfindingResult
                {
                    Success = false,
                    Solution = null
                };
            }

            SimpleFloorPlane first = start;
            SimpleFloorPlaneGateway gateway = gateList[0];
            SimpleFloorPlane second = end;
            PlaneTransferData[] solution = new PlaneTransferData[gateList.Count];

            for (int i = 0; i < gateList.Count; i++)
            {
                // first is considered previous.
                var currentGate = gateList[i];
                if (i == gateList.Count - 1)
                {
                    // cap off with 'end'
                    second = end;
                }
                else
                {
                    if (currentGate.ToPlane == first)
                    {
                        second = currentGate.FromPlane;
                    }
                    if (currentGate.FromPlane == first)
                    {
                        second = currentGate.ToPlane;
                    }
                }
                solution[i] = new PlaneTransferData(
                    first,
                    currentGate,
                    second
                );
                // update with next gate.
                first = second;
            }

            return new SimplePlaneTraversalResult()
            {
                Success = true,
                Solution = solution
            };
        }
    }
}