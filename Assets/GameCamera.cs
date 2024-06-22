using System;
using System.Collections;
using System.Collections.Generic;
using Prototypal;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    // Start is called before the first frame update
    SimpleFloorPlaneGraph world;
    BoundedVector boundedPosition; 
    Actor focus;
    void Start()
    {
        boundedPosition = new BoundedVector();
        // Camera should not be able to go up or down.
        boundedPosition.LockAxis(BoundedVector.Axis.Y);
        if (world == null)
        {
            world = GetComponentInParent<SimpleFloorPlaneGraph>();
            if (world == null)
            {
                throw new InvalidOperationException("unable to obtain reference to World");
            }
        }
        // get the absolute furthest back point in the whole world.
        var bounds = world.GetBounds();
    }

    // Update is called once per frame
    void Update()
    {
        // bound the transform's position.
        boundedPosition.Value = transform.position;
    }
}
