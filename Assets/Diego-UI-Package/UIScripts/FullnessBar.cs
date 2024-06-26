using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Diego {
    public class FullnessBar : MonoBehaviour {
        public float maxSize = 2;
        void Start() {
            transform.localScale = new Vector3(0, 1, 1);
        }

        public void SetValue(int num, int max) {
            transform.localScale = new Vector3((num / (float)max) * maxSize, 1, 1);
        }
    }
}
