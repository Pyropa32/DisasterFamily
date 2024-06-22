using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Diego
{
    // Readonly structs live on the stack
    // They're quite nice to have
    public readonly struct Item : ISelectable
    {
        private readonly int id;
        private readonly Sprite sprite;

        public Item(int id, Sprite sprite)
        {
            this.id = id;
            this.sprite = sprite;
        }
        public bool CanBeCollected => true;
        public bool CanBeInteractedWith => false;
        public int ID => id;
        public Sprite Sprite => sprite;
    }
}