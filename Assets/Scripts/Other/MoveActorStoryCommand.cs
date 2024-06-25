using System;
using UnityEngine;
using Prototypal;
public sealed class MoveActorStoryCommand : IStoryCommand
{

    // object = args
    public event Action<object> OnFinish;
    public Vector2 Destination { get; private set; }
    public Vector2 CurrentPosition => Vector2.Lerp(startPosition, Destination, progress);
    public float MovementSpeed { get; private set; }
    public bool IsFinished { get => finished; }
    public bool IsConcurrent { get => true; }
    public bool IsStarted { get => started; }
    public StoryCommandExecutionFlags ExecutionFlags => StoryCommandExecutionFlags.DiscardAlike |
                                                        StoryCommandExecutionFlags.DiscardConcurrent;
    private SimpleActor actor;
    private Vector2 startPosition;
    private float progress = 0f;
    private bool finished = false;
    private bool started = false;
    private SimpleFloorPlane adjacent;

    public MoveActorStoryCommand(SimpleActor _actor, Vector2 _start, Vector2 _end, float _movementSpeed, SimpleFloorPlane _adjacent=null)
    {
        if (_adjacent != null)
        {
            adjacent = _adjacent;
            OnFinish = (object arg) =>
            {
                _actor.DoSetCurrentPlane((SimpleFloorPlane)arg);
            };
        }
        
        startPosition = _start;
        Destination = _end;

        // Make walking take the same amount of time regardless of distance.
        var plane = _actor.CurrentPlane;
        var dist = Vector2.Distance(plane.PlaneToScreen(_start), plane.PlaneToScreen(_end));
        _movementSpeed /= dist;

        var bottomToTopDiagonal = plane.TopRight - plane.BottomLeft;
        var topToBottomDiagonal = plane.TopLeft - plane.BottomRight;
        var ratio = bottomToTopDiagonal.sqrMagnitude / topToBottomDiagonal.sqrMagnitude;
        var direction = (_end - _start).normalized;
        // ratio is 1-1 in normal situations, so division shouldn't hurt.
        // both are square(d), so who cares(d)

        // divide by the ratio when perpendicularity is lowest.
        // AKA, when dot product is 0.
        var perpendicularity = Vector2.Dot(bottomToTopDiagonal.normalized, direction.normalized);

        // divide movement speed fully if moving fully perpendicularly.
        _movementSpeed /= Mathf.Lerp(ratio, 1f, Math.Abs(perpendicularity));


        // // FIXME: There's one more thing
        // // You move slightly faster if you walk at an angle perpendicular to the angle ➚ from BottomLeft to TopRight ➚
        // var diagonal = plane.TopRight - plane.BottomLeft;
        // // 1.0 = walking perpendicular. 0.0 = walking parallel to diagonal.
        // var perpendicularity = 1.0 - Mathf.Abs(Vector2.Dot(diagonal.normalized, (_end - _start).normalized));
        // // Use perpendicularity to negate the extra walk speed.

        MovementSpeed = _movementSpeed;
        actor = _actor;
    }

    public object GetProgressModel()
    {
        return progress;
    }

    public void Start()
    {
        started = true;
        actor.LocalPosition = startPosition;
        // mayhaps?
        actor.SetAnim("walk");
        Debug.Log("started walk command");
    }

    public void Tick(float delta)
    {
        if (!finished)
        {
            progress += delta * MovementSpeed;
            if (progress >= 1f)
            {
                finished = true;
                actor.SetAnim("idle");
                OnFinish?.Invoke(adjacent);
            }
            else
            {
                actor.LocalPosition = CurrentPosition;
            }
        }
    }
}
