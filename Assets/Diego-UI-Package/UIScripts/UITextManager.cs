using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace Diego
{
    public class UITextManager : MonoBehaviour
    {
        private static UITextManager instance = null;
        private TextMeshPro tmp;

        private string text = "";
        private string mission = "";

        void Start() {
            if (instance != null) {
                Destroy(this);
                return;
            }
            instance = this;
            tmp = instance.transform.GetComponent<TextMeshPro>();
        }

        void Update() {
            tmp.text = text;
            if (mission != "") {
                tmp.text += "\nMission:\n\t"+mission;
            }
        }

        public static void Clear() {
            instance.text = "";
            instance.mission = "";
            instance.tmp.text = "";
        }
        public static void SetText(string text) {
            instance.text = text;
        }
        public static void SetMission(string text) {
            instance.mission = text;
        }
    }
}
