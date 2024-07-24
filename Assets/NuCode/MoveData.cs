using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Replaces MoveActorCommand.
/// use Tick() to move inch by inch
/// refer to Value to get the current position
/// </summary>
/// 
public class MoveDataChain
{
    const float FULL_ACCEL_STOP_DISTANCE = 0.1f;
    const float FULL_DECEL_STOP_DISTANCE = 1.7f;
    public float WalkSpeed { get; set; }
    public bool IsFinished => (Progress >= (1.0f - Mathf.Epsilon)) || forceIsFinished;
    public float Progress => totalProgressedLength / pathLength;
    public Vector2 Value => value;
    private Vector2 value;
    private MoveData[] data;
    private float pathLength;
    private float totalProgressedLength;
    private int dataIndex;
    private bool forceIsFinished = false;
    private Vector2 previousValue;
    public MoveDataChain(Vector2 start, Vector2[] path, float _walkSpeed = 2f)
    {
        // needs start + (path.size * 2 - 1)
        data = new MoveData[path.Length];
        totalProgressedLength = 0f;
        dataIndex = 0;
        value = start;
        previousValue = Vector2.zero;

        pathLength = 0f;
        Vector2 currentStart = start;
        for (int i = 0; i < path.Length; i += 1)
        {
            data[i] = new MoveData(currentStart, path[i], _walkSpeed);

            currentStart = data[i].Destination;
            pathLength += (data[i].Destination - data[i].Value).magnitude;
        }
    }

    public void Tick()
    {
        previousValue = value;
        var currentData = data[dataIndex];
        var speedModifier = 1f;
        var decelBegin = Math.Abs((pathLength) - FULL_DECEL_STOP_DISTANCE);
        //Debug.Log("percent there: " + decimal.Round((decimal)(totalProgressedLength / decelBegin * 100)));
        if (totalProgressedLength < FULL_ACCEL_STOP_DISTANCE)
        {
            speedModifier = Mathf.Lerp(0.5f, 1f, totalProgressedLength / FULL_ACCEL_STOP_DISTANCE);
        }
        else if (Math.Abs(totalProgressedLength) >= decelBegin &&
                 pathLength > FULL_DECEL_STOP_DISTANCE)
        {
            var start = (pathLength) - FULL_DECEL_STOP_DISTANCE;
            speedModifier = Mathf.Lerp(1f, 0f, (totalProgressedLength - start) / pathLength - start);
            //Debug.Log("DECREASING!");
        }
        if (speedModifier < 0.1f)
        {
            forceIsFinished = true;
            return;
        }
        currentData.Tick(speedModifier);
        
        previousValue = value;
        value = currentData.Value;
        
        totalProgressedLength += (value - previousValue).magnitude;
        if (currentData.IsFinished)
        {
            dataIndex += 1;
            if (dataIndex >= data.Length)
            {
                forceIsFinished = true;
                return;
            }
        }
    }

    public Vector2 CurrentDirection()
    {
        return (Value - previousValue).normalized;
    }

}
public class MoveData
{
    public Vector2 Value { get; set; }
    public float WalkSpeed { get; set; }
    public Vector2 Destination { get; set; }

    public bool IsFinished => (Destination - Value).sqrMagnitude < (Mathf.Epsilon * Mathf.Epsilon);
    public MoveData(Vector2 _start, Vector2 _finish, float _walkSpeed = 2f)
    {
        Value = _start;
        Destination = _finish;
        WalkSpeed = _walkSpeed;
    }
    public void Tick(float _speedModifier = 1f)
    {
        var delta = Time.deltaTime * WalkSpeed * _speedModifier;
        Vector2 v = Value;
        Vector2 vd = Destination - v;
        float len = vd.magnitude;
        if (len <= delta || len < Mathf.Epsilon)
        {
            Value = Destination;
        }
        else
        {
            Value = v + (vd / len * delta);
        }
    }
}
