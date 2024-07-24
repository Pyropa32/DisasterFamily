using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropKickEffect : MonoBehaviour
{
    const float OPTION_AVAILABILITY_MAX_DISTANCE = 1.5f; 
    PlayerAdapter playerAdapter; 
    // Start is called before the first frame update
    void Start()
    {
        // hunt for reference to player
        var maybePlayerObject = GameObject.Find("Player");
        if (maybePlayerObject != null)
        {
            playerAdapter = maybePlayerObject.GetComponent<PlayerAdapter>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PollOptionAvailability()
    {
        if (playerAdapter != null)
        {
            var inRange = (transform.position - playerAdapter.transform.position).magnitude
             < OPTION_AVAILABILITY_MAX_DISTANCE;
             if (inRange)
             {
                
             }
        }
    }
}
