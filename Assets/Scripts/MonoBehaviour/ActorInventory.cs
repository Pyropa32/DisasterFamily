using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Diego;
public class ActorInventory : MonoBehaviour
{
    private IList<Item> inventory = new List<Item>();

    void Start()
    {
        ItemsUniverse.loadFromFile("ItemList");
    }

    public int Count
    {
        get {return inventory.Count;}
    }

    public void AddItem(WorldItem item)
    {
        inventory.Add(item.Data);
        // Sync with InventoryManager;
        //GameObject addedObject = GameObject.Find("Items");
        //InventoryManager.toggleInInventory(item.ID);
        //Destroy(addedObject);
        
    }
}
