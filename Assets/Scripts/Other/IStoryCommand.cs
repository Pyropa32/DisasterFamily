using System;

public interface IStoryCommand
{
    //TODO: Add Suspend() and Cancel() methods as such
    // Suspend() is called when a blocking StoryCommand goes to the StoryCommandDispatcher
    // Cancel() is called when a StoryCommand with Deleting StoryCommandExecutionFlags goes to the StoryCommandDispatcher.
    public void Start();
    public void Tick(float delta);
    public object GetProgressModel();
    public event Func<object> OnFinish;
    public bool IsFinished { get; }
    public bool IsStarted { get; }
    public bool IsConcurrent { get; }
    public StoryCommandExecutionFlags ExecutionFlags { get; }

}

