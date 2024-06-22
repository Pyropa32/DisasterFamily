using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{

    [SerializeField]
    FloorPlane currentPlane;
    [SerializeField]
    Transform footPosition;
    Vector2 localPosition;
    FloorPlaneGraph world;

    public FloorPlaneGraph World => world;

    public FloorPlane CurrentPlane 
    {
        get
        {
            return currentPlane;
        }
        set
        {
            currentPlane = value;
        }
    }

    // FIXME: this does not need to exist. Just set the property.
    public void DoSetCurrentPlane(FloorPlane newPlane)
    {
        // FIXME: do the conversion work in the CurrentPlane property instead.
        // first, convert local coordinates (which are the coordinates of CurrentPlane) to screen coordinates
        // then, convert to the basis of the new plane, then clamp it to fit inside the plane;
        Debug.Log("set current plane!");
        
        // revert old coordinate system, apply new coordinate system
        var val = newPlane.ScreenToPlane(CurrentPlane.PlaneToScreen(LocalPosition));
        CurrentPlane = newPlane;
        LocalPosition = val;
    }

    void Start()
    {
        world = GetComponentInParent<FloorPlaneGraph>();
        if (world == null)
        {
            throw new InvalidOperationException("World on this actor is null!!");
        }
        Debug.Log("hello, says Actor!");
        currentPlane.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        GlobalPosition = currentPlane.PlaneToScreen(localPosition);
        if (GlobalPosition.x < -6f || GlobalPosition.x > 10f)
        {
            Debug.Log("found error in update");
        }
    }

    public void SetAnim(string what)
    {

    }

    public float MovementSpeed => movementSpeed;

    public Vector2 GlobalPosition
    {
        get
        {
            return new Vector2(footPosition.position.x, footPosition.position.y);
        }
        set
        {
            transform.position = new Vector3(value.x - footPosition.position.x, value.y - footPosition.position.y);
        }
    }

    // TODO: Actually make sure to convert to plane coordinates.
    public Vector2 LocalPosition
    {
        get
        {
            return localPosition;
        }
        set
        {
            localPosition = value;
        }
    }
}
