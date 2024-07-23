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
        public int id;

        public void Start() {
            mySprite = gameObject.GetComponent<SpriteRenderer>().sprite;
            action = this.Action;
            if (gameObject.GetComponent<Collider2D>() == null) {
                gameObject.AddComponent<Collider2D>();
            }
        }
        public void Kill() {
            Destroy(gameObject);
        }
        public void Action(Item item) {
            Debug.Log("Clicked");
            if (item.Equals(Item.Empty)) {
                bool success = InventoryManager.addInInventory(id);
                if (success) {
                    transform.GetComponent<Animator>().SetTrigger("click");
                }
                else {
                    //display message saying inventory is full
                    //UITextManager.SetTextForSeconds("I can't pick that up my hands are full.", 5f);
                    DialogueManager.textToLoad("Sample.Depparin.0");
                    Debug.Log("Inventory Full!");
                }
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