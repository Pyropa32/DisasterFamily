using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Diego
{
    public readonly struct Item : ISelectable, IEquatable<Item>
    {
        private readonly int id;
        private readonly Sprite sprite;
        private readonly string remarks;
        private readonly string name;
        public Item(int id, Sprite sprite, string _name="", string _remarks="")
        {
            this.id = id;
            this.sprite = sprite;
            name = _name;
            remarks = _remarks;
        }
        public bool CanBeCollected => true;
        public bool CanBeInteractedWith => false;
        public int ID => id;
        public Sprite Sprite => sprite;
        public static Item Empty => new Item(-1, null);
        public bool Equals(Item other)
        {
            return id == other.id && sprite == other.sprite;
        }
    }
}