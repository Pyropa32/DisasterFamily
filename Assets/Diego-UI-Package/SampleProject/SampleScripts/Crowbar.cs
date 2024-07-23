using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Diego
{
    public class Crowbar : MonoBehaviour, IInteractable
    {
        private Action<Item> action;
        private Sprite mySprite;

        public Action<Item> OnInteract { get => action; set => action = value; }

        public bool CanBeCollected => false;

        public bool CanBeInteractedWith => true;

        public int ID => 0;
        public float Range => 0.25f;

        public Sprite Sprite => mySprite;

        public void Start()
        {
            mySprite = gameObject.GetComponent<SpriteRenderer>().sprite;
            action = this.Action;
        }
        public void Action(Item item)
        {
            // when mouse put press up runs function
            if (item.Equals(Item.Empty))
            {
                Debug.Log("Crowbar used");
                InventoryManager.toggleInInventory(0);
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