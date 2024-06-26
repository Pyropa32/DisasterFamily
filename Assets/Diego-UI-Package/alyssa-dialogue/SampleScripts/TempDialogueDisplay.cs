using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempDialogueDisplay : MonoBehaviour {
    private int i = 0;

    void Start() {
        DialogueManager.loadFromFile("Sample");
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha0) && i < 3) {
            DialogueManager.textToLoad("Sample.mom." + i);
            i++;
        }
    }
}