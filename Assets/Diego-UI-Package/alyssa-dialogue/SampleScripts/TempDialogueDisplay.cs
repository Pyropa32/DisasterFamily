using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempDialogueDisplay : MonoBehaviour {
    void Start() {
        gameObject.GetComponent<DialogueClient>()?.load();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha0)) {
            gameObject.GetComponent<DialogueClient>()?.displayText();
        }
    }
}
