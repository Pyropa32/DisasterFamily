using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Selectable {
    public bool isCollectable();
    public bool isInteractable();
    public int getID();
    public Sprite getSprite();
}
