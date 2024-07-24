using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class DropKickEffect : MonoBehaviour, IRangeDependent
{
    const float OPTION_AVAILABILITY_MAX_DISTANCE = 1.5f;
    PlayerAdapter playerAdapter;
    Character myDropKickableCharacter;
    SpriteRenderer dropKickOptionUI;
    RoomGraph world;
    public bool IsRelevant => inRange && !isPrepared;
    private bool inRange = false;
    private bool isPrepared = false;
    bool awaitingMoveToKickPos = false;
    // Start is called before the first frame update
    void Start()
    {
        // hunt for reference to player
        var maybePlayerObject = GameObject.Find("Player");
        if (maybePlayerObject != null)
        {
            playerAdapter = maybePlayerObject.GetComponent<PlayerAdapter>();
        }
        var dropKickInteractUIObject = GameObject.Find("DropKickInteractUI");
        if (dropKickInteractUIObject != null)
        {
            dropKickOptionUI = dropKickInteractUIObject.GetComponent<SpriteRenderer>();
        }
        myDropKickableCharacter = GetComponent<Character>();
        world = myDropKickableCharacter.World;
    }

    void Abort()
    {
        isPrepared = false;
        awaitingMoveToKickPos = false;
    }

    // Update is called once per frame
    void Update()
    {
        // try to get the world
        if (world == null)
        {
            world = myDropKickableCharacter.World;
        }

        PollOptionAvailability();
        if (playerAdapter != null &&
            playerAdapter.PressedEButtonThisFrame &&
            IsRelevant)
        {
            isPrepared = true;
            PrepareDropKickEffect();
        }
    }

    void PrepareDropKickEffect()
    {
        isPrepared = true;
        if (world == null)
        {
            return;
        }
        Debug.Log("DropKickPrepare!");
        dropKickOptionUI.enabled = false;
        // make pilot character get over here.
        var dropKicker = playerAdapter.Player;
        // adjacent
        if (dropKicker.CurrentRoom == myDropKickableCharacter.CurrentRoom)
        {
            var internalPath = dropKicker.CurrentRoom.GetInteriorPathFrom(
                dropKicker.transform.position,
                myDropKickableCharacter.transform.position
            );
            dropKicker.MoveAlongPath(internalPath, this);
        }
        else
        {
            var externalPath = world.GetExteriorPathFrom(
                dropKicker.transform.position,
                myDropKickableCharacter.transform.position
            );
            // TY DIEGO!!! :))))) loving the interface here! :)
            dropKicker.MoveAlongPath(externalPath, this);
        }

        // subscribe to 
    }
    void ExecuteDropKickEffect()
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
                dropKickOptionUI.enabled = true;
                inRange = true;
            }
            else
            {
                dropKickOptionUI.enabled = false;
                inRange = false;
            }
        }
    }

    public void finish()
    {
    }

    public void cancel()
    {
        Abort();
    }

    public void start()
    {
        ExecuteDropKickEffect();
    }

    public bool isInRange(Vector2 player)
    {
         var inRange = (transform.position - playerAdapter.transform.position).magnitude
             < OPTION_AVAILABILITY_MAX_DISTANCE;
        return inRange;
    }
}
