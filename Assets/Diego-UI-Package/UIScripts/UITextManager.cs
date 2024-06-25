using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UITextManager : MonoBehaviour {
    private static UITextManager instance = null;
    private Queue<string> q;
    private TextMeshPro tmp;

    private bool clicked = false;

    private string writingText = "";
    private string currentText = "";
    private bool writing = false;
    private float timer = 0f;

    public const float writeSpeed = 20f;

    void Start() {
        if (instance != null) {
            Destroy(this);
            return;
        }
        instance = this;
        q = new Queue<string>();
        tmp = instance.transform.GetComponent<TextMeshPro>();
    }

    void Update() {
        if (tmp.text == "" && q.Count > 0) {
            writingText = q.Dequeue();
            writing = true;
        }
        if (writing) {
            timer += Time.deltaTime;
            while (timer > 1f / writeSpeed) {
                if (clicked || writingText.Length == currentText.Length) {
                    currentText = writingText;
                    writing = false;
                    timer = 0;
                    break;
                }
                currentText += writingText.Substring(writingText.Length, 1);
                timer -= 1f / writeSpeed;
            }
        }
        else if (tmp.text != "" && clicked) {
            tmp.text = "";
            currentText = "";
            writingText = "";
        }
        if (clicked) {
            clicked = false;
        }
        tmp.text = currentText;
    }

    void OnMouseDown() {
        clicked = true;
    }

    public static void EnqueueText(string text) {
        instance.q.Enqueue(text);
    }
    public static void EnqueueTexts(string[] text) {
        foreach (string s in text) {
            EnqueueText(s);
        }
    }
    public static void Clear() {
        instance.q.Clear();
        instance.writingText = "";
        instance.currentText = "";
        instance.writing = false;
        instance.timer = 0f;
        instance.tmp.text = "";
    }
    public static void SetText(string text) {
        Clear();
        instance.writingText = text;
        instance.currentText = text;
        instance.tmp.text = text;
    }
}
