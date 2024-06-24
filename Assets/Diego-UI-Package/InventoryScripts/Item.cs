using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace Diego
{
    public struct Item : ISelectable, IEquatable<Item>
    {
        private const string ITEM_SPRITE_PATHS = "ItemSprites";
        private int id;
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
            var path = _spritePath.Trim().ToLower();
            return new Item()
            {
                id = _id,
                sprite = Resources.Load<Sprite>(Path.Combine(ITEM_SPRITE_PATHS, path)),
                name = _name,
                remarks = _remarks,
                quality = _quality
            };
        }

        public Item(int id, Sprite _sprite, string _name="", string _remarks="", ItemQuality _quality=ItemQuality.Useless)
        {
            this.id = id;
            this.sprite = _sprite;
            name = _name;
            remarks = _remarks;
            quality = _quality;
        }
        public bool CanBeCollected => true;
        public bool CanBeInteractedWith => false;
        public int ID => id;
        public Sprite Sprite => sprite;
        public string Name => name;
        public string Remarks => remarks;
        public ItemQuality Quality => quality;
        public static Item Empty => new Item(-1, null, String.Empty, String.Empty);
        public bool Equals(Item other)
        {
            return id == other.id && sprite == other.sprite;
        }
    }
}