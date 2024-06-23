using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flare : MonoBehaviour, Interactable {
    private Action<Item> action;
    private Sprite mySprite;
    
    public void Start() {
        mySprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        action = this.Action;
    }
    public void Action(Item item) {
        // when mouse put press up runs function
        if (item == null) {
            Debug.Log("Flare used");
            InventoryManager.toggleInInventory(1);
            Destroy(gameObject);
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
