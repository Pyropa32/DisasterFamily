using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollection : MonoBehaviour, IStoryCommand
{
    private Actor actor;
    private Item item;
    private float progress = 0f;
    private float totalDuration = 0f;
    private bool finished = false;
    private bool started = false;
    public bool IsFinished { get => finished; }
    public bool IsConcurrent { get => true; }
    public bool IsStarted { get => true; }
    public StoryCommandExecutionFlags ExecutionFlags => StoryCommandExecutionFlags.Ignore;
    
    
    public void Start(){
        started = true;
    }
    public void Tick(float delta){
         if (!finished)
        {
            progress += delta * 1f;
            if (progress >= 1f)
            {
                finished = true;
                //actor.SetAnim("idle");
                OnFinish?.Invoke();
            }
        }
    }
    public object GetProgressModel(){
        return progress;
    }
    public event Func<object> OnFinish;

    /*public ItemCollection(Actor _actor, Item _item){
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
        item.collected = true;
    }

}
