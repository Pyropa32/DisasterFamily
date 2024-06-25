using System;
using System.Collections;
using System.Collections.Generic;
using Diego;

public class InteractableActionStoryCommand : IStoryCommand {
    public bool started;
    public bool finished;
    public Item item;
    public event Action<Object> OnFinish;
    public bool IsFinished { get => finished; }
    public bool IsStarted { get => started; }
    public bool IsConcurrent { get => true; }
    public StoryCommandExecutionFlags ExecutionFlags => StoryCommandExecutionFlags.DiscardAlike |
                                                        StoryCommandExecutionFlags.DiscardConcurrent;

    public InteractableActionStoryCommand(Action<Item> onFinish, Item item) {
        this.item = item;
        OnFinish = (object arg) => {
            onFinish.Invoke(item);
        };
    }
    public void Start() {
        OnFinish.Invoke(item);
        finished = true;
        started = true;
    }
    public void Tick(float delta) {
        // nothing
    }
    public object GetProgressModel() {
        if (IsStarted && IsFinished) {
            return 1f;
        }
        return 0f;
    }
    public Type InstanceType => GetType();
}
public interface IInteractable : ISelectable {
    public Action<Item> OnInteract { get; set; }
    private InteractableActionStoryCommand ToStoryCommand(Item item) {
        return new InteractableActionStoryCommand(OnInteract, item);
    }
    public void GetInRangeAndDo(Item item, UnityEngine.Vector2 itemPos) {
        UnityEngine.Vector2 playerPos = UnityEngine.GameObject.FindWithTag("Player").transform.position + new UnityEngine.Vector3(0, 0.75f, 0);
        UnityEngine.Vector2 displacement = itemPos - playerPos;
        float range = 1f;
        if (displacement.magnitude <= range) {
            OnInteract.Invoke(item);
            return;
        }
        displacement /= 1 - (range / displacement.magnitude);
        List<IStoryCommand> after = new List<IStoryCommand>();
        after.Add(ToStoryCommand(item));
        UnityEngine.GameObject.FindWithTag("Player").GetComponent<ClickToMove>().OnClicked(displacement + playerPos, after);
    }
}