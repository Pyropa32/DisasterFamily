using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Diego;

public class SceneChanger : MonoBehaviour
{
    public TextMeshPro tmp;
    private bool gameStart = false;
    private bool lateStart = true;
    private static SceneChanger instance = null;

    void Start() {
        if (instance != null) {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (lateStart) {
            UITextManager.SetMission("Explore the house.");
            Timer.restartTimer();
            lateStart = false;
        }
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
            UITextManager.SetMission("Get supplies to the drop off!");
            gameStart = true;
            Timer.restartTimer();
            DialogueManager.textToLoad("Sample.Depparin.2");
        }
    }

    public static bool getState() {
        return instance.gameStart;
    }
}
