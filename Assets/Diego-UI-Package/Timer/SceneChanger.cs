using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneChanger : MonoBehaviour
{
    Timer timer;
    public TextMeshPro tmp;

    void Start()
    {
        timer = new Timer(60);
        timer.StartTimer();
    }

    // Update is called once per frame
    void Update()
    {
        timer.CountdownTimer();
        tmp.text = ((int)timer.GetTime()).ToString();
        if (timer.GetTime() <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
