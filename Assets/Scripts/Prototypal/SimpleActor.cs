using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prototypal
{
    public class SimpleActor : MonoBehaviour
    {
        [SerializeField]
        float movementSpeed = 2f;

        [SerializeField]
        SimpleFloorPlane currentPlane;
        Vector2 localPosition;
        SimpleFloorPlaneGraph world;

        public SimpleFloorPlaneGraph World => world;

        public SimpleFloorPlane CurrentPlane
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
        public void DoSetCurrentPlane(SimpleFloorPlane newPlane)
        {
            // FIXME: do the conversion work in the CurrentPlane property instead.
            // first, convert local coordinates (which are the coordinates of CurrentPlane) to screen coordinates
            // then, convert to the basis of the new plane, then clamp it to fit inside the plane;
            Debug.Log("set current plane!");

            // revert old coordinate system, apply new coordinate system
            var val = new Vector2();
            if (CurrentPlane != null)
            {
                val = newPlane.ScreenToPlane(CurrentPlane.PlaneToScreen(LocalPosition));
            }
            else
            {
                val = newPlane.ScreenToPlane(transform.position);
            }
            CurrentPlane = newPlane;
            LocalPosition = val;
        }

        void Start()
        {
            world = GetComponentInParent<SimpleFloorPlaneGraph>();
            if (world == null)
            {
                throw new InvalidOperationException("World on this actor is null!!");
            }
            Debug.Log("hello, says Actor!");
        }

        // Update is called once per frame
        void Update()
        {
            if (currentPlane == null)
            {
                DoSetCurrentPlane(World.GetPlaneByPosition(transform.position));
            }
            GlobalPosition = currentPlane.PlaneToScreen(localPosition);
        }

        public void SetAnim(string what)
        {

        }

        public float MovementSpeed => movementSpeed;

        public Vector2 GlobalPosition
        {
            get
            {
                return transform.position;
            }
            set
            {
                transform.position = new Vector3(value.x, value.y, -0.2f);
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
}