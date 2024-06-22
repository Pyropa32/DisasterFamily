using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace Diego
{
    public class UITextManager : MonoBehaviour
    {
        private static UITextManager instance = null;
        private TextMeshPro label;

        void Start()
        {
            label = GetComponent<TextMeshPro>();
            if (instance != null)
            {
                Destroy(this);
                return;
            }
            instance = this;
        }

        public static void SetText(string text)
        {
            // don't need to call GetComponent every time, it *is* kind of expensive
            instance.label.text = text;
        }
    }
}