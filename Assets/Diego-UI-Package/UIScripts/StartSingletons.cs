using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSingletons : MonoBehaviour {
    public string[] dialogueXMLs;
    void Start() {
        foreach (string s in dialogueXMLs) {
            DialogueManager.loadFromFile(s);
        }
    }
}
