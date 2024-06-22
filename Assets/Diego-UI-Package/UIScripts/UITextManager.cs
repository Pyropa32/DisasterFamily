using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UITextManager : MonoBehaviour {
    private static UITextManager instance = null;

    void Start() {
        if (instance != null) {
            Destroy(this);
            return;
        }
        instance = this;
    }

    public static void SetText(string text) {
        instance.transform.GetComponent<TextMeshPro>().text = text;
    }
}
