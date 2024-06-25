using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Diego
{
    public class Keys : MonoBehaviour, IInteractable
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
                Debug.Log("Keys used");
                InventoryManager.toggleInInventory(2);
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