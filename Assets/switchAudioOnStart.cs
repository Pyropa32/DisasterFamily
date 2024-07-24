using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class switchAudioOnStart : MonoBehaviour {
    public GameObject audio1;
    public GameObject audio2;
    public AudioSource pickUp;
    public AudioSource drop;
    public AudioSource dropOff;
    private bool turned = false;
    void Update() {
        if (SceneChanger.getState() && !turned) {
            turned = true;
            audio1.SetActive(false);
            audio2.SetActive(true);
        }
    }
    public void playSound(int id) {
        switch (id) {
            case 0:
                pickUp.Play();
                break;
            case 1:
                drop.Play();
                break;
            case 2:
                dropOff.Play();
                break;
        }
    }
}
