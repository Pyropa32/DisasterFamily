using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]

public class Timer
{
    public float timeLimit;
    public float currTime; // time to display
    bool timerActive;

    public Timer(float time)
    {
        timeLimit = time;
    }
    public void StartTimer ()
    {
        timerActive = true;
        currTime = timeLimit;
    }
    public void StopTimer()
    {
        timerActive = false;
    }
    public float GetTime ()
    {
        return currTime;
    }
    public void CountdownTimer()
    {
        if (timerActive)
        {
            currTime -= Time.deltaTime;
            if (currTime <= 0)
            {
                StopTimer();
                currTime = 0;
            }
        }
    }

    
}
