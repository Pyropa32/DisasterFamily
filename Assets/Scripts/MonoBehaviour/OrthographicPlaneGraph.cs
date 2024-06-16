using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dijkstra.NET.Graph;
using Dijkstra.NET.ShortestPath;
using System;
using System.ComponentModel;
using System.Linq;
public class OrthographicPlaneGraph : MonoBehaviour
{
    // Start is called before the first frame update
    private Graph<OrthographicPlaneGateway, OrthographicPlane> graph = new Graph<OrthographicPlaneGateway, OrthographicPlane>();
    private Dictionary<OrthographicPlaneGateway, uint> gateToID = new Dictionary<OrthographicPlaneGateway, uint>();
    void Start()
    {
        // TODO: Get all of my child OrthographicPlanes and 
        // create an adjacency list that links them.
        // The list will be used for External pathfinding.
        OrthographicPlaneGateway[] allGates = GetComponentsInChildren<OrthographicPlaneGateway>();
        // create connections.
        List<Tuple<OrthographicPlaneGateway, OrthographicPlaneGateway, uint, uint>> connections =
        new List<Tuple<OrthographicPlaneGateway, OrthographicPlaneGateway, uint, uint>>();
        for (uint i = 0; i < allGates.Length; i++)
        {
            for (uint j = 0; j < allGates.Length; j++)
            {
                var gateway1 = allGates[i];
                var gateway2 = allGates[j];

                if (gateway1.ToPlane == gateway2.FromPlane ||
                    gateway1.FromPlane == gateway2.ToPlane ||
                    gateway1.FromPlane == gateway2.FromPlane ||
                    gateway1.ToPlane == gateway2.ToPlane)
                {
                    // duplicates should be okay.
                    connections.Add(new Tuple<OrthographicPlaneGateway, OrthographicPlaneGateway, uint, uint>(
                        gateway1,
                        gateway2,
                        i,
                        j
                    ));
                }
            }

            // add the current gate to the graph
            graph.AddNode(allGates[i]);
            // update planeToID
            gateToID[allGates[i]] = i;
            allGates[i].SetWorld(this);

        }
        // add gateways


        // connect all the gateways together
        for (int i = 0; i < connections.Count; i++)
        {
            var currentConnection = connections[i];

            var gateway1 = currentConnection.Item1;
            var gateway2 = currentConnection.Item2;
            var gateway1ID = currentConnection.Item3;
            var gateway2ID = currentConnection.Item4;

            var distance2 = gateway1.Distance2To(gateway2);

            // it doesnt matter if the distance is squared
            gateway1.TryGetSharedPlane(gateway2, out OrthographicPlane plane);
            graph.Connect(gateway1ID, gateway2ID, distance2, plane);
        }
    }

    // Eventually, this will need to be made private. The public method needs to find a path that is always usable.
    ShortestPathResult ShortestExternalPath(OrthographicPlane start, OrthographicPlane end)
    {
        // Have to figure out which gates to choose.
        // May be O(m^2) where m is number of gates.
        var startGateways = start.GetGateways();
        var endGateways = end.GetGateways();

        // don't include 
        int k = 0;
        List<ShortestPathResult> results = new List<ShortestPathResult>(startGateways.Length * endGateways.Length);
        
        for (int i = 0; i < startGateways.Length; i++)
        {
            for (int j = 0; j < endGateways.Length; j++)
            {
                var startGatewayID = gateToID[startGateways[i]];
                var endGatewayID = gateToID[endGateways[j]];

                var path = graph.Dijkstra(startGatewayID, endGatewayID);
                results.Add(path);
            }
        }

        // delete the paths that weren't found:
        results.RemoveAll(path => path.IsFounded == false);
        // find the smallest path
        ShortestPathResult result = new ShortestPathResult();
        for (int i = 0; i < results.Count; i++)
        {
            if (i == 0)
            {
                result = results[i];
                continue;
            }
            if (result.Distance > results[i].Distance)
            {
                result = results[i];
            }

        }
        result.GetPath();
        return result;
    }



    // Update is called once per frame
    void Update()
    {

    }
}
