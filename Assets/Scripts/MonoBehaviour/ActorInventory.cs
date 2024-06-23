using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorInventory : MonoBehaviour
{
    private IList<Item> inventory = new List<Item>();

    public int Count
    {
        get {return inventory.Count;}
    }

    public void AddItem(Item item)
    {
        inventory.Add(item);
    }
}
