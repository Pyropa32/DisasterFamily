using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Diego
{
    // Yep, we're throwing away this goddamned prototype, so many goddamn duplicate classes
    // I think we literally have like 50 classes for what this game is. Holy damn
    // Let's build it back up not from scratch, reuse some classes, but only consider what we actually need.
    public class GeneralItem : MonoBehaviour, IInteractable
    {
        private Action<Item> action;
        private Sprite mySprite;

        public Action<Item> OnInteract { get => action; set => action = value; }

        public bool CanBeCollected => false;

        public bool CanBeInteractedWith => true;

        public int ID => 0;

        public Sprite Sprite => mySprite;

        public void Start()
        {
            mySprite = gameObject.GetComponent<SpriteRenderer>().sprite;
            action = this.Action;
        }
        public void Action(Item item)
        {
            if (item.Equals(Item.Empty))
            {
                InventoryManager.toggleInInventory(ItemLookup.GetItemFromSprite(mySprite));
                Destroy(gameObject);
            }
        }
        public int getID()
        {
            return 0;
        }
        public Sprite getSprite()
        {
            return mySprite;
        }
        public Action<Item> getAction()
        {
            return action;
        }
        public bool isCollectable()
        {
            return false;
        }
        public bool isInteractable()
        {
            return true;
        }
    }
}