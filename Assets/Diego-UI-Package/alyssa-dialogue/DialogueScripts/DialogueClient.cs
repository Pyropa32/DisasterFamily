using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueClient : MonoBehaviour {
    public string path;

    public void load() {
        DialogueManager.loadFromFile(path);
    }

    public void displayText() {
        bool erm = DialogueManager.textToLoad(path);
    }
}
