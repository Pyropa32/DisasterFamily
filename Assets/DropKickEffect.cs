using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class DropKickEffect : MonoBehaviour, IRangeDependent
{
    const float OPTION_AVAILABILITY_MAX_DISTANCE = 0.8f;
    PlayerAdapter playerAdapter;
    Character myDropKickableCharacter;
    SpriteRenderer dropKickOptionUI;
    Animator dropKickEffectAnimator;
    RoomGraph world;
    public bool IsRelevant => InRange && !isPrepared;
    private bool InRange =>  (new Vector3(4.17f, 3.0f, playerAdapter.transform.position.z) - playerAdapter.transform.position).magnitude
             < OPTION_AVAILABILITY_MAX_DISTANCE;
    private bool active = false; 
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
        dropKickOptionUI.enabled = false;

    }

    void Abort()
    {
        active = false;
        isPrepared = false;
        awaitingMoveToKickPos = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!InRange)
        {
            dropKickOptionUI.enabled = false;
        }
        // try to get the world
        if (world == null)
        {
            world = myDropKickableCharacter.World;
            dropKickEffectAnimator = world.GetComponent<Animator>();
        }
        if (playerAdapter.PressedEButtonThisFrame)
        {
            int a = 4;
        }

        PollOptionAvailability();
        if (playerAdapter != null &&
            playerAdapter.PressedEButtonThisFrame &&
            IsRelevant)
        {
            isPrepared = true;
            PrepareDropKickEffect();
        }
        PollDropKickAnimDone();
    }

    void PollDropKickAnimDone()
    {
        if (!active)
        {
            return;
        }
        if (dropKickEffectAnimator == null)
        {
            return;
        }
        Debug.Log("time:" + dropKickEffectAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        if (dropKickEffectAnimator.GetCurrentAnimatorStateInfo(0).IsName("Exit") ||
            dropKickEffectAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.80f)
        {
            Debug.Log("Finished!");
            Finish();
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
        dropKickEffectAnimator.enabled = true;
        dropKickEffectAnimator.Rebind();
        dropKickEffectAnimator.Update(0f);
        dropKickEffectAnimator.Play("DoDropKick",0);
        active = true;
        playerAdapter.enabled = false;
        dropKickEffectAnimator.SetTrigger("dropKick");
    }

    public void Finish()
    {
        active = false;
        playerAdapter.enabled = true;
        dropKickEffectAnimator.enabled = false;
        myDropKickableCharacter.enabled = false;
        // JANK!
        myDropKickableCharacter.GetComponent<SpriteRenderer>().color = Color.clear;
        dropKickOptionUI.color = Color.clear;
        // FUCK YOU!! I thought that shit would remove the COMPONENT'S GAME OBJECT OMG!!!
        // NO, I HAVE TO SPECIFY THE GAME OBJECT
        Destroy(myDropKickableCharacter.gameObject);
    }

    void PollOptionAvailability()
    {
        if (playerAdapter != null)
        {
            var whatTheFuckIsWrongWithTheTransform = new Vector3(4.17f, 3.0f, playerAdapter.transform.position.z);
            var inRange = (whatTheFuckIsWrongWithTheTransform - playerAdapter.transform.position).magnitude
             < OPTION_AVAILABILITY_MAX_DISTANCE;
            if (inRange)
            {
                dropKickOptionUI.enabled = true;
            }
            else
            {
                dropKickOptionUI.enabled = false;
            }
        }
    }

    public void finish()
    {
        ExecuteDropKickEffect();
    }

    public void cancel()
    {
        Abort();
    }

    public void start()
    {
        
    }

    public bool isInRange(Vector2 player)
    {
         var inRange = (transform.position - playerAdapter.transform.position).magnitude
             < OPTION_AVAILABILITY_MAX_DISTANCE;
        return inRange;
    }
}
