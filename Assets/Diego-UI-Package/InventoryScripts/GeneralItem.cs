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
        public Room parentRoom;

        public void Start() {
            mySprite = gameObject.GetComponent<SpriteRenderer>().sprite;
            action = this.Action;
            if (gameObject.GetComponent<Collider2D>() == null) {
                gameObject.AddComponent<CircleCollider2D>();
            }
        }
        public void Kill() {
            Destroy(gameObject);
        }
        public void Action(Item item) {
            if (item.Equals(Item.Empty)) {
                bool success = InventoryManager.addInInventory(id);
                if (success) {
                    GameObject.FindWithTag("MainCamera")?.GetComponent<switchAudioOnStart>()?.playSound(0);
                    transform.GetComponent<Animator>().SetTrigger("click");
                }
                else {
                    //display message saying inventory is full
                    //UITextManager.SetTextForSeconds("I can't pick that up my hands are full.", 5f);
                    DialogueManager.textToLoad("Sample.Depparin.0");
                }
            }
        }
        public int ID => 0;
        public float Range => 1f;
        public Room ParentRoom => parentRoom;
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