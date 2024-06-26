using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextSceneOnReturn : MonoBehaviour {
    void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha9)) {
            SceneManager.LoadScene(1);
        }
    }
}
