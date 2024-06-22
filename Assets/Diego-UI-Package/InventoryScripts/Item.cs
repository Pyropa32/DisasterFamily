using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Selectable {
    private int id = -1;
    private Sprite sprite;

    public Item(int id, Sprite sprite) {
        this.id = id;
        this.sprite = sprite;
    }

    public bool isCollectable() {
        return true;
    }
    public bool isInteractable() {
        return false;
    }
    public int getID() {
        return id;
    }
    public Sprite getSprite() {
        return sprite;
    }
}
