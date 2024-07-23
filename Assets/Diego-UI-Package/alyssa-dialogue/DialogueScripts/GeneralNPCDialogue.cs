using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Diego {
    public class GeneralNPCDialogue : MonoBehaviour, IInteractable {
        public string[] dialoguePaths;
        public bool incrementBy1 = true;
        public bool loop = false;
        public bool infinite = true;
        public int[] steps;
        public int changeIndOnStart = -1;

        private int ind = 0;
        private Action<Item> action;

        public void Start() {
            action = this.Action;
            if (gameObject.GetComponent<Collider2D>() == null) {
                gameObject.AddComponent<BoxCollider2D>();
            }
        }
        public void Update() {
            if (SceneChanger.getState() && changeIndOnStart != -1) {
                ind = changeIndOnStart;
                changeIndOnStart = -1;
            }
        }
        public void Action(Item item) {
            if (ind != -1 && dialoguePaths.Length > 0) {
                DialogueManager.textToLoad(dialoguePaths[ind]);
                if (incrementBy1) {
                    ind++;
                }
                else {
                    if (steps.Length <= ind) {
                        Debug.LogError("No valid dialogue step at index: " + ind);
                        return;
                    }
                    ind += steps[ind];
                }
                int temp = ind;
                if (loop) {
                    ind %= dialoguePaths.Length;
                }
                else {
                    ind = Mathf.Clamp(ind, 0, dialoguePaths.Length-1);
                }
                if (!infinite && temp != ind) {
                    ind = -1;
                }
            }
        }
        public int ID => ind;
        public float Range => 1f;
        public Sprite Sprite => GetComponent<SpriteRenderer>()?.sprite;
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