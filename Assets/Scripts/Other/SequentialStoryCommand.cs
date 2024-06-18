using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class SequentialStoryCommand : IStoryCommand
{
    public bool IsFinished => isFinished;
    public bool IsStarted => isStarted;
    public bool IsConcurrent => isConcurrent;

    public StoryCommandExecutionFlags ExecutionFlags => customFlags;
    public event Action<object> OnFinish;
    private bool isStarted = false;
    private bool isConcurrent;
    private int currentCommandIndex = 0;
    private bool isFinished = false;
    private StoryCommandExecutionFlags customFlags;
    private List<IStoryCommand> commands = new List<IStoryCommand>();
    private Type _type;
    public Type InstanceType => _type;

    public SequentialStoryCommand(IEnumerable<IStoryCommand> _commands, bool _concurrent=true, StoryCommandExecutionFlags flags=StoryCommandExecutionFlags.Ignore)
    {
        isConcurrent = _concurrent;
        customFlags = flags;
        OnFinish = new Action<object>((arg) => {});
        foreach (var item in _commands)
        {
            commands.Add(item);
        }
        if (commands.Count == 0)
        {
            throw new ArgumentException("command size cannot be zero!");
        }
        _type = commands[0].GetType();
    }

    public object GetProgressModel()
    {
        return currentCommandIndex;
    }

    public void Start()
    {
        commands[0].Start();
        isStarted = true;
    }

    public void Tick(float delta)
    {
        if (!IsFinished)
        {
            if (!commands[currentCommandIndex].IsFinished)
            {
                commands[currentCommandIndex].Tick(delta);
            }
            else
            {
                currentCommandIndex += 1;
                if (currentCommandIndex < commands.Count)
                {
                    commands[currentCommandIndex].Start();
                }
                else
                {
                    isFinished = true;
                }
            }
        }
    }
}