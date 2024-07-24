using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Diego {
    public class StartOrQuit : MonoBehaviour {
        public void click(bool starting) {
            if (starting) {
                SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex + 1)%(3));
            }
            else {
                Application.Quit();
#if UNITY_EDITOR
                if(EditorApplication.isPlaying) {
                    UnityEditor.EditorApplication.isPlaying = false;
                }
#endif
            }
        }
    }
}
