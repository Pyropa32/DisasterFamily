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
    Actor focusedActor;
    bool _hasSetInitialBounds = false;
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
        // get the absolute min and max of the world.

    }

    void SetUpBounds()
    {
        var bounds = world.GetBounds();
        boundedPosition.SetAxisLowerBound(BoundedVector.Axis.X, bounds.min.x);
        boundedPosition.SetAxisLowerBound(BoundedVector.Axis.Y, bounds.min.y);

        boundedPosition.SetAxisUpperBound(BoundedVector.Axis.X, bounds.max.x);
        boundedPosition.SetAxisUpperBound(BoundedVector.Axis.Y, bounds.max.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (world.HasStarted && !_hasSetInitialBounds)
        {
            _hasSetInitialBounds = false;
            SetUpBounds();
        }
        // bound the transform's position.
        boundedPosition.Value = transform.position;
    }
}
