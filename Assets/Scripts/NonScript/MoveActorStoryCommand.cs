using System;
using UnityEngine;

public sealed class MoveActorStoryCommand : IStoryCommand
{

    public event Func<object> OnFinish;
    public Vector2 Destination { get; private set; }
    public Vector2 CurrentPosition => Vector2.Lerp(startPosition, Destination, progress);
    public float MovementSpeed { get; private set; }
    public bool IsFinished { get => finished; }
    public bool IsConcurrent { get => true; }
    public bool IsStarted { get => true; }
    public StoryCommandExecutionFlags ExecutionFlags => StoryCommandExecutionFlags.DiscardAlike |
                                                        StoryCommandExecutionFlags.DiscardConcurrent;
    private Actor actor;
    private Vector2 startPosition;
    private float progress = 0f;
    private bool finished = false;
    private bool started = false;

    public MoveActorStoryCommand(Actor _actor, Vector2 _start, Vector2 _end, float _movementSpeed)
    {
        startPosition = _start;
        Destination = _end;
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
                OnFinish?.Invoke();
            }
            else
            {
                actor.LocalPosition = CurrentPosition;
            }
        }
    }
}
