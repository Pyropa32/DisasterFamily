using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]

public class Timer
{
    public float timeLimit;
    public float currTime; // time to display
    bool timerActive = true;
    bool timerUnpausedCalled = false;
    int timerUnpauseBuffer = 2;
    int timerUnpauseBufferCount;
    private static Timer instance = null;

    private Timer(float time) {
        timeLimit = time;
        currTime = timeLimit;
        timerActive = true;
    }
    private void StartTimer() {
        timerUnpausedCalled = true;
    }
    private void StopTimer() {
        timerActive = false;
    }
    private float GetTime() {
        return currTime;
    }
    private void CountdownTimer() {
        if (timerActive && !timerUnpausedCalled)
        {
            currTime -= Time.deltaTime;
            if (currTime <= 0)
            {
                StopTimer();
                currTime = 0;
            }
        }
        else if (timerActive)
        {
            timerUnpauseBufferCount++;
            if (timerUnpauseBufferCount >= timerUnpauseBuffer)
            {
                timerUnpausedCalled = false;
            }
        }
        else if (timerUnpausedCalled)
        {
            timerUnpauseBufferCount = 0;
            timerActive = true;
        }
    }

    private static Timer getInstance(float time = -1) {
        if (instance == null) {
            if (time == -1) {
                Debug.LogError("Timer referenced before initialized.");
                return null;
            }
            instance = new Timer(time);
        }
        return instance;
    }
     // resume
    public static void startTimer(float time = 60) {
        getInstance(time).StartTimer();
    }

    public static void stopTimer() {
        getInstance().StopTimer();
    }

    public static float getTime() {
        return getInstance().GetTime();
    }

    public static void countdownTimer()
    {
        getInstance().CountdownTimer();
    }

    // start
    public static void restartTimer(float time = 60) {
        instance = null;
        getInstance(time);
    }

    public static bool isPaused() {
        return !getInstance().timerActive || getInstance().timerUnpausedCalled;
    }
}
