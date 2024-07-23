using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
namespace Diego
{

    public class SpriteWrapper {
        // public Sprite sprite;
        public SpriteWrapper(string _path) {
            // sprite = _sprite;
        }

    }
    public struct Item : ISelectable, IEquatable<Item>
    {
        private const string ITEM_SPRITE_PATHS = "ItemSprites/Release";
        private int id;
        private string spritePath;
        private Sprite sprite;
        private string remarks;
        private string name;
        private ItemQuality quality;
        public static Item CreateWithSpritePath(int _id, 
                                                string _spritePath,
                                                string _name = "",
                                                string _remarks = "",
                                                ItemQuality _quality=ItemQuality.Useless)
        {
            var name = _spritePath.Trim().ToLower();

            return new Item()
            {
                id = _id,
                spritePath = Path.Combine(ITEM_SPRITE_PATHS, name),
                name = _name,
                remarks = _remarks,
                quality = _quality
            };
        }

        public Item(int id, string _sprite, string _name="", string _remarks="", ItemQuality _quality=ItemQuality.Useless)
        {
            this.id = id;
            this.spritePath = _sprite;
            sprite = null;
            name = _name;
            remarks = _remarks;
            quality = _quality;
        }
        public bool CanBeCollected => true;
        public bool CanBeInteractedWith => false;
        public int ID => id;
        public Sprite Sprite => sprite != null ? sprite : sprite = Resources.Load<Sprite>(spritePath);
        public string Name => name;
        public string Remarks => remarks;
        public ItemQuality Quality => quality;
        public static Item Empty => new Item(-1, null, String.Empty, String.Empty);
        public bool Equals(Item other)
        {
            return id == other.id && spritePath == other.spritePath;
        }

        public int GetID(){
            return id;
        }
    }
}