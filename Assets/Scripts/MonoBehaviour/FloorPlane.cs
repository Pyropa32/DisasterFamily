using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using LostTrainDude;
using UnityEngine;

public class FloorPlane : MonoBehaviour, IEquatable<FloorPlane>
{
    // Start is called before the first frame update
    // X and Y vector stored in a matrix.
    public Vector2 BottomLeft => transform.position;
    // only affected by X
    public Vector2 BottomRight => new Vector2(transform.position.x + (uvX.x * extents.x), transform.position.y + uvX.y);
    // only affected by Y
    public Vector2 TopLeft => new Vector2(transform.position.x + (uvY.x * extents.y), transform.position.y + (uvY.y * extents.y));
    public Vector2 TopRight => new Vector2(transform.position.x + (uvX.x * extents.x) + (uvY.x * extents.y),
                                           transform.position.y + (uvY.y * extents.y) + uvX.y /* not broken (yet)*/);

    [SerializeField]
    private Vector2 uvX;
    [SerializeField]
    private Vector2 uvY;
    [SerializeField]
    private Vector2Int extents;
    private HashSet<FloorPlaneGateway> myGateways = new HashSet<FloorPlaneGateway>();

    private Vector2 offset;

    private Matrix4x4 orthographicBasis;
    private Matrix4x4 orthographicBasisInverse;
    // Not currently used
    private List<Actor> actors = new List<Actor>();
    private List<Obstacle> obstacles = new List<Obstacle>();
    private BitGrid obstacleGrid;
    private LostTrainDude.GridGraph obstacleGraph;

    // TODO: Support blocking obstacles
    // FloorPlane should also have its own grid built in
    // obstacles a part of this plane should erase walkable spaces from the grid.
    // Moreover, support Exits to link with other Planes.
    // Use Internal Pathfinding (A*) to:
    //      Navigate the plane
    //      Navigate to a plane exit included in the path to another plane.
    // test
    public void AddGateway(FloorPlaneGateway toAdd)
    {
        myGateways.Add(toAdd);
    }

    public FloorPlaneGateway[] GetGateways()
    {
        return myGateways.ToArray();
    }

    void Awake()
    {
        // uvX's Y coordinate doesn't hold any weight.
        // uvY's X coordinate only holds 1x weight regardless of extents.
        orthographicBasis = new Matrix4x4(
            new(uvX.x * extents.x, uvX.y * extents.x, 0f, 0f),
            new(uvY.x * extents.y, uvY.y * extents.y, 0f, 0f),
            new(0f, 0f, 1f, 0f),
            new(0f, 0f, 0f, 1f)
        );

        orthographicBasisInverse = orthographicBasis.inverse;
        offset = transform.position;

        // set up pathfinding grid.
        obstacleGrid = new BitGrid(extents.x, extents.y);
        obstacleGraph = new LostTrainDude.GridGraph(extents.x, extents.y);
    }

    public Vector2 ClampLocal(Vector2 localCoordinates)
    {
        return new Vector2(
            Mathf.Clamp(localCoordinates.x, 0f, 1f),
            Mathf.Clamp(localCoordinates.y, 0f, 1f)            
        );
    }

    public bool IsPointInRange(Vector2 globalCoordinates)
    {
        var clamped = ClampGlobal(globalCoordinates);
        return Mathf.Approximately(clamped.x, globalCoordinates.x) &&
               Mathf.Approximately(clamped.y, globalCoordinates.y);
    }

    public Vector2 ClampGlobal(Vector2 globalCoordinates)
    {
        return PlaneToScreen(ClampLocal(ScreenToPlane(globalCoordinates)));
    }

    public Vector2 ScreenToPlane(Vector2 screenCoordinates)
    {
        // place at 'origin', tform
        var result = orthographicBasisInverse * (screenCoordinates - offset);
        return ClampLocal(result);
    }

    public Vector2 PlaneToScreen(Vector2 worldCoordinates)
    {
        // transform, offset
        var result = orthographicBasis * ClampLocal(worldCoordinates);
        return (Vector2)result + offset;
    }

    public Vector2Int PlaneToGrid(Vector2 localCoordinates)
    {
        // use uvX and uvY to snap to increments.
        // if extents are 18, 4, then...
        // 0f, 0f maps to <0,0>
        // 0.5f, 0.5f maps to <9,2> (just multiply local by extents)
        return new Vector2Int(
            Mathf.RoundToInt(localCoordinates.x * extents.x),
            Mathf.RoundToInt(localCoordinates.y * extents.y)
        );
    }
    public Vector2 GridToPlane(Vector2Int gridCoordinates)
    {
        return new Vector2(
            gridCoordinates.x / (float)extents.x,
            gridCoordinates.y / (float)extents.y
        );
    }

    public void Add(Actor actor)
    {
        actor.LocalPosition = ScreenToPlane(actor.GlobalPosition);
        actors.Add(actor);
    }

    public void AddObstacle(Obstacle obstacle)
    {
        if (obstacle.CurrentPlane != this)
        {
            throw new ArgumentException(obstacle.name + " is not a part of this plane!");
        }
        var gridPosition = PlaneToGrid(ScreenToPlane(obstacle.transform.position));
        for (int i = 0; i < obstacle.CellSize.x; i++)
        {
            for (int j = 0; i < obstacle.CellSize.y; j++)
            {
                // cellSize = 1. 0, 0, 0
                // cellSize = 2. 1, 0, 1
                var coordinates = gridPosition + new Vector2Int(
                    i - (obstacle.CellSize.x / 2),
                    j - (obstacle.CellSize.y / 2));
                obstacleGrid.Set(coordinates.x, coordinates.y, true);
                
                obstacleGraph.Walls.Add(coordinates); 
            }
        }
        obstacle.transform.position = PlaneToScreen(GridToPlane(gridPosition));
        obstacles.Add(obstacle);
    }

    public List<Vector2> GetShortestInternalPath(Vector2 from, Vector2 to)
    {
        var result = new List<Vector2>();

        var fromGridCoords = PlaneToGrid(ScreenToPlane(from));
        var toGridCoords = PlaneToGrid(ScreenToPlane(to));

        var untrimmed = AStar.Search(
            obstacleGraph,
            obstacleGraph.Grid[fromGridCoords.x,fromGridCoords.y],
            obstacleGraph.Grid[toGridCoords.x,toGridCoords.y]);
        
        // TODO: Add trimming to reduce the amount of walk commands issued.
        // no trimming yet
        result = untrimmed.ConvertAll(node => node.Position);

        return result;
    }

// show 
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

    // Update is called once per frame
    void Update()
    {

    }

    public bool Equals(FloorPlane other)
    {
        return this == other;
    }
}
