
using System;

[Flags]
public enum StoryCommandExecutionFlags
{
    Ignore,
    DiscardAlike,
    DiscardNonAlike,
    DiscardConcurrent,
    DiscardBlocking
}

