using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempDialogue : MonoBehaviour {
    private bool check = false;
    void Update() {
        if (check) {
            return;
        }
        check = true;
        DialogueTextManager.EnqueueText("This is test text1.");
        DialogueTextManager.EnqueueText("This is test text2.");
        DialogueTextManager.EnqueueText("This is test text3.");
    }
}
