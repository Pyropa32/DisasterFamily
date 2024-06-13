using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

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

    public void Receive(IStoryCommand command)
    {

        if ((command.ExecutionFlags & StoryCommandExecutionFlags.DiscardConcurrent) != 0)
        {
            if ((command.ExecutionFlags & StoryCommandExecutionFlags.DiscardAlike) != 0)
            {
                concurrentStoryCommandList.RemoveAll(_command => _command.GetType() == command.GetType());
            }
            if ((command.ExecutionFlags & StoryCommandExecutionFlags.DiscardNonAlike) != 0)
            {
                concurrentStoryCommandList.RemoveAll(_command => _command.GetType() != command.GetType());
            }
        }

        if ((command.ExecutionFlags & StoryCommandExecutionFlags.DiscardBlocking) != 0)
        {
            if ((command.ExecutionFlags & StoryCommandExecutionFlags.DiscardAlike) != 0)
            {
                blockingStoryCommandList.RemoveAll(_command => _command.GetType() == command.GetType());
            }
            if ((command.ExecutionFlags & StoryCommandExecutionFlags.DiscardNonAlike) != 0)
            {
                blockingStoryCommandList.RemoveAll(_command => _command.GetType() != command.GetType());
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
