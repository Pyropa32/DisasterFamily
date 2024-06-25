using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Diego {
    public class GeneralItem : MonoBehaviour, IInteractable {
        private Action<Item> action;
        private Sprite mySprite;

        public void Start() {
            mySprite = gameObject.GetComponent<SpriteRenderer>().sprite;
            action = this.Action;
        }
        public void Action(Item item) {
            if (item.Equals(Item.Empty)) {
                InventoryManager.toggleInInventory(ItemLookup.GetItemFromSprite(mySprite));
                Destroy(gameObject);
            }
        }
        public int ID => 0;
        public Sprite Sprite => mySprite;
        public Action<Item> OnInteract {
            get {
                return action;
            }
            set {
                action = value;
            }
        }
        public bool CanBeCollected => false;
        public bool CanBeInteractedWith => true;
    }
}