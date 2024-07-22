using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneChanger : MonoBehaviour
{
    public TextMeshPro tmp;
    private bool gameStart = false;

    void Start()
    {
        Timer.restartTimer();
    }

    // Update is called once per frame
    void Update()
    {
        Timer.countdownTimer();
        if (gameStart) {
            tmp.text = ((int)Timer.getTime()).ToString();
        }
        if (Timer.getTime() <= 0) {
            if (gameStart) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                return;
            }
            tmp.text = "60";
            gameStart = true;
            Timer.restartTimer();
            DialogueManager.textToLoad("Sample.Depparin.2");
        }
    }
}
