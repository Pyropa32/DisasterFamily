using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public class StoryCommandDispatcher : MonoBehaviour
{
    public bool IsBlocked => blockingStoryCommandList.Count > 0;
    private List<IStoryCommand> blockingStoryCommandList = new();
    // Not all commands behave like this.
    // Only non blocking commands go here.
    private List<IStoryCommand> concurrentStoryCommandList = new();

    // Concurrent commands:
    //      Walk, etc.
    // They do not belong in the queue
    // They should enter a processing list that serves a tick to every command in the list.
    // Any finished commands are freed from the list.

    // Commands that must be completed first: 

    public void ReceiveSequentialRange(IEnumerable<IStoryCommand> commands, bool blocking=true, StoryCommandExecutionFlags flagOverride=StoryCommandExecutionFlags.Ignore)
    {
        _Receive(
            new SequentialStoryCommand(commands, _concurrent:!blocking, flags:flagOverride), true
        );
    }

    public void Receive(IStoryCommand command)
    {
        _Receive(command, true);
    }
    private void _Receive(IStoryCommand command, bool useFlags)
    {

        if (useFlags)
        {
            if (command.ExecutionFlags.HasFlag(StoryCommandExecutionFlags.DiscardConcurrent))
            {
                if (command.ExecutionFlags.HasFlag(StoryCommandExecutionFlags.DiscardAlike))
                {
                    concurrentStoryCommandList.RemoveAll(_command => _command.InstanceType == command.InstanceType);
                }
                if (command.ExecutionFlags.HasFlag(StoryCommandExecutionFlags.DiscardNonAlike))
                {
                    concurrentStoryCommandList.RemoveAll(_command => _command.InstanceType != command.InstanceType);
                }
            }

            if (command.ExecutionFlags.HasFlag(StoryCommandExecutionFlags.DiscardBlocking))
            {
                if (command.ExecutionFlags.HasFlag(StoryCommandExecutionFlags.DiscardAlike))
                {
                    blockingStoryCommandList.RemoveAll(_command => _command.InstanceType == command.InstanceType);
                }
                if (command.ExecutionFlags.HasFlag(StoryCommandExecutionFlags.DiscardNonAlike))
                {
                    blockingStoryCommandList.RemoveAll(_command => _command.InstanceType != command.InstanceType);
                }
            }
        }

        if (command.IsConcurrent)
        {
            concurrentStoryCommandList.Add(command);
        }
        else
        {
            blockingStoryCommandList.Add(command);
        }

    }

    private void ProcessBlockingCommands(float delta)
    {
        for (int i = 0; i < blockingStoryCommandList.Count; i++)
        {
            var currentCommand = blockingStoryCommandList[i];
            if (currentCommand.IsFinished)
            {
                blockingStoryCommandList.RemoveAt(0);
                i--;
                continue;
            }
            else if (!currentCommand.IsStarted)
            {
                currentCommand.Start();
            }
            else
            {
                currentCommand.Tick(delta);
            }
            break;
        }
    }

    private void ProcessConcurrentCommands(float delta)
    {
        if (IsBlocked)
        {
            return;
        }

        for (int i = 0; i < concurrentStoryCommandList.Count; i++)
        {
            var currentCommand = concurrentStoryCommandList[i];
            if (currentCommand.IsFinished)
            {
                concurrentStoryCommandList.RemoveAt(0);
                i--;
                continue;
            }
            else if (!currentCommand.IsStarted)
            {
                currentCommand.Start();
            }
            else
            {
                currentCommand.Tick(delta);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ProcessBlockingCommands(Time.deltaTime);
        ProcessConcurrentCommands(Time.deltaTime);
    }
}
