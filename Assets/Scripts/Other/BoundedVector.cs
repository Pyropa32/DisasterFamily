using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prototypal;
using System;
using UnityEditor;
public struct BoundedVector
{
    public enum Axis
    {
        X,
        Y,
        Z
    }

    public Vector3 Value
    {
        get 
        {
            return position;
        }
        set
        {
            _SetPosition(value);
        }
    }

    // Start is called before the first frame update
    bool lockX;
    bool lockY;
    bool lockZ;
    Vector3 lockBackTo;
    Vector3 maximum;
    Vector3 minimum;
    Vector3 position;
    public BoundedVector(Vector3 initialValue)
    {
        lockBackTo = initialValue;
        position = initialValue;
        maximum = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
        minimum = new Vector3(float.MinValue, float.MinValue, float.MinValue);
        lockX = false;
        lockY = false;
        lockZ = false;
    }

    public void LockAxis(Axis which)
    {
        switch (which)
        {
            case Axis.X:
                lockX = true;
            break;
            case Axis.Y:
                lockY = true;
            break;
            case Axis.Z:
                lockZ = true;
            break;
        }
    }

    public void UnlockAxis(Axis which)
    {
        switch (which)
        {
            case Axis.X:
                lockX = false;
            break;
            case Axis.Y:
                lockY = false;
            break;
            case Axis.Z:
                lockZ = false;
            break;
        }
    }

    public void ToggleAxis(Axis which)
    {
        switch (which)
        {
            case Axis.X:
                lockX = !lockX;
            break;
            case Axis.Y:
                lockY = !lockY;
            break;
            case Axis.Z:
                lockZ = !lockZ;
            break;
        }
    }
    public void SetAxisUpperBound(Axis which, float value)
    {
        maximum = new Vector3(
            which == Axis.X ? value : maximum.x,
            which == Axis.Y ? value : maximum.y,
            which == Axis.Z ? value : maximum.z
        );
    }

    public void SetAxisLowerBound(Axis which, float value)
    {
        minimum = new Vector3(
            which == Axis.X ? value : minimum.x,
            which == Axis.Y ? value : minimum.y,
            which == Axis.Z ? value : minimum.z
        );
    }

    private void _SetPosition(Vector3 what)
    {
        position = new Vector3(
            lockX ? lockBackTo.x : Mathf.Clamp(what.x, minimum.x, maximum.x),
            lockY ? lockBackTo.y : Mathf.Clamp(what.y, minimum.y, maximum.y),
            lockZ ? lockBackTo.z : Mathf.Clamp(what.z, minimum.z, maximum.z) 
        );
        lockBackTo = new Vector3(
            lockX ? lockBackTo.x : position.x,
            lockY ? lockBackTo.y : position.y,
            lockZ ? lockBackTo.z : position.z
        );
    }
}
