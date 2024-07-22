using System;
using System.Collections;
using System.Collections.Generic;
using Diego;

namespace Diego {
    public class DropOff : UnityEngine.MonoBehaviour, IInteractable {
        private static DropOff instance;

        private Action<Item> action;
        private UnityEngine.Sprite mySprite;
        private List<Item> items;
        private int maxItems = 12;

        public void Start() {
            if (instance != null) {
                Destroy(gameObject);
                return;
            }
            instance = this;
            DontDestroyOnLoad(gameObject);
            mySprite = gameObject.GetComponent<UnityEngine.SpriteRenderer>().sprite;
            action = this.Action;
            items = new List<Item>();
        }
        public void Action(Item item) {
            if (item.Equals(Item.Empty)) {
                return;
            }
            if (items.Count >= maxItems) {
                DialogueManager.textToLoad("Sample.Depparin.1");
                return;
            }
            else {
                items.Add(item);
                InventoryManager.removeInInventory(item.ID);
                transform.GetChild(0).GetComponent<FullnessBar>().SetValue(items.Count, maxItems);
            }
        }
        public int ID => 0;
        public UnityEngine.Sprite Sprite => mySprite;
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

        public static List<Item> GetItemsAndKill() {
            List<Item> items = instance.items;
            Destroy(instance.gameObject);
            return items;
        }
        public static int GetMaxNum() {
            return instance.maxItems;
        }
    }
}