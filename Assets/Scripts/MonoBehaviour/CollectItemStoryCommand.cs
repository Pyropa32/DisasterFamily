using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CollectItemCommand : IStoryCommand
{
    private Actor actor;
    private WorldItem item = null;
    private float progress = 0f;
    private bool finished = false;
    public bool IsFinished { get => finished; }
    public bool IsConcurrent { get => true; }
    public bool IsStarted { get => true; }
    public StoryCommandExecutionFlags ExecutionFlags => StoryCommandExecutionFlags.Ignore;
    private event Action<object> _onFinish;
    public event Action<object> OnFinish
    {
        add
        {
            _onFinish += value;
        }

        remove
        {
            _onFinish -= value;
        }
    }

    public void Start()
    {
    }
    public void Tick(float delta)
    {
        if (!finished)
        {
            progress += delta * 1f;
            if (progress >= 1f)
            {
                finished = true;
                //actor.SetAnim("idle");
                _onFinish?.Invoke(null);
            }
        }
    }
    public object GetProgressModel()
    {
        return progress;
    }

    /*public CollectItemCommand(Actor _actor, Item _item){
        actor = _actor;
        item = _item;

        //check if the actor is close enough to the item (nvm) 
        //and if it has enough inventory slots ir ILIST?

        /*if(item.weight == "light"){
            totalDuration = 1f;
        }
        else if(item.weight == "medium"){
            totalDuration = 3f;
        }
        else if(item.weight == "heavy"){
            totalDuration = 6f;
        }

        //implement case where the duration to pick up item exceeds remaining time (maybe not)

        //add item to actor inventory (make it its own class?)
    }*/

    public void ClickToCollect()
    {
        /*if(actor.inventory.inventory.Count < 1)
        {
            actor.inventory.AddItem(item);
            item.collected = true;
        }*/
        item.ClickToCollect();
    }

}
