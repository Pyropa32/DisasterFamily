using System;

public interface IStoryCommand
{
    public void Start();
    public void Tick(float delta);
    public object GetProgressModel();
    public event Func<object> OnFinish;
    public bool IsFinished { get; }
    public bool IsStarted { get; }
    public bool IsConcurrent { get; }
    public StoryCommandExecutionFlags ExecutionFlags { get; }

}

