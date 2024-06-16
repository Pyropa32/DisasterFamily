using System.Collections.Generic;
using UnityEngine;

public class OrthographicPlane : MonoBehaviour
{
    // Start is called before the first frame update
    // X and Y vector stored in a matrix.
    public Vector2 BottomLeft => transform.position;
    // only affected by X
    public Vector2 BottomRight => new Vector2(transform.position.x + (uvX.x * extents.x), transform.position.y + uvX.y);
    // only affected by Y
    public Vector2 TopLeft => new Vector2(transform.position.x + (uvY.x * extents.y), transform.position.y + (uvY.y * extents.y));
    public Vector2 TopRight => new Vector2(transform.position.x + (uvX.x * extents.x) + (uvY.x * extents.y),
                                           transform.position.y + (uvY.y * extents.y) + uvX.y);

    [SerializeField]
    private Vector2 uvX;
    [SerializeField]
    private Vector2 uvY;
    [SerializeField]
    private Vector2Int extents;

    private Vector2 offset;

    private Matrix4x4 orthographicBasis;
    private Matrix4x4 orthographicBasisInverse;
    private List<Actor> actors = new List<Actor>();

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
        offset = ScreenToPlane(transform.position);

        Debug.Log("uvX is " + uvX);
        Debug.Log("uvX is " + uvY);
        Debug.Log(orthographicBasis);
        Debug.Log("offset is " + offset);
        Debug.Log("0.5,0.5 is " + ScreenToPlane(new Vector2(0.5f,0.5f)));
        Debug.Log("0.75,0.5 is " + ScreenToPlane(new Vector2(0.75f,0.5f)));
        Debug.Log("0.0,0.0 is " + ScreenToPlane(new Vector2(0.0f,0.0f)));
        Debug.Log("1.0,1.0 is " + ScreenToPlane(new Vector2(1.0f,1.0f)));
        Debug.Log("0.0,0.0 is " + ScreenToPlane(new Vector2(0.5f,0.5f)));

    }

    public Vector2 ClampLocal(Vector2 localCoordinates)
    {
        return new Vector2(
            Mathf.Clamp(localCoordinates.x, 0f, 1f),
            Mathf.Clamp(localCoordinates.y, 0f, 1f)            
        );
    }

    public Vector2 ClampGlobal(Vector2 globalCoordinates)
    {
        return new Vector2(
            Mathf.Clamp(globalCoordinates.x, offset.x, TopRight.x + offset.x),
            Mathf.Clamp(globalCoordinates.y, offset.x, TopRight.y + offset.y)            
        );
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

    public void Add(Actor actor)
    {
        actor.LocalPosition = ScreenToPlane(actor.GlobalPosition);
        actors.Add(actor);
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
}
