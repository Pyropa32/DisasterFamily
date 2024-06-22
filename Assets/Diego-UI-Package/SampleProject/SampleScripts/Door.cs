using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, Interactable {
    private Action<Item> action;
    private Sprite mySprite;
    
    public void Start() {
        mySprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        action = this.Action;
    }
    public void Action(Item item) {
        if(item == null) {
            return;
        }
        if (item.getID() == 2) {
            Destroy(gameObject);
            InventoryManager.toggleInInventory(item.getID());
        }
    }
    public int getID() {
        return 0;
    }
    public Sprite getSprite() {
        return mySprite;
    }
    public Action<Item> getAction() {
        return action;
    }
    public bool isCollectable() {
        return false;
    }
    public bool isInteractable() {
        return true;
    }
}
