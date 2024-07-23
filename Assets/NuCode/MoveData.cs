using System;
using UnityEngine;

/// <summary>
/// Replaces MoveActorCommand.
/// use Tick() to move inch by inch
/// refer to Value to get the current position
/// </summary>
public class MoveData
{
    public Vector2 Value { get; set; }
    public float WalkSpeed { get; set; }
    public Vector2 Destination { get; set; }

    public bool IsFinished => (Destination - Value).sqrMagnitude < (Mathf.Epsilon * Mathf.Epsilon); 
    public MoveData(Vector2 _start, Vector2 _finish, float _walkSpeed=2f)
    {
        Value = _start;
        Destination = _finish;
        WalkSpeed = _walkSpeed;
    }
    public void Tick()
    {
        var delta = Time.deltaTime * WalkSpeed;
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
