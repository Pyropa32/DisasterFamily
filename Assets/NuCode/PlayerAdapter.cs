using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Diego;

/// <summary>
/// Put this component on the character in order to make them the player.
/// </summary>
[RequireComponent(typeof(Character))]
public class PlayerAdapter : MonoBehaviour
{
    // continuous click
    // continuous
    private const uint FRAMES_REQUIRED_FOR_HOLD_CLICK = 8;
    private const uint FRAMES_FOR_AUTO_MOVE_FREQUENCY = 5;
    private uint consecutiveHeldFrames = 0;
    private bool IsMouseButtonHeld => consecutiveHeldFrames >= FRAMES_REQUIRED_FOR_HOLD_CLICK;

    bool previousLeftMouseDown = false;
    // Start is called before the first frame update
    private Character player;
    private RoomGraph world;
    void Start()
    {
        player = GetComponent<Character>();
        world = GetComponentInParent<RoomGraph>();
        DialogueManager.NoOp();
    }

    // Consistency!
    void FixedUpdate()
    {
        var currentLeftMouseDownNow = Input.GetMouseButtonDown(0);
        var currentLeftMouseDown = Input.GetMouseButton(0);
        // update consecutiveness
        if (currentLeftMouseDown)
        {
            consecutiveHeldFrames += 1;
        }
        else
        {
            consecutiveHeldFrames = 0;
        }


        Vector2 worldMousePosition = CameraToScreenspaceConverter.GetGameSpaceFromScreenSpace(Input.mousePosition);
        bool inRange = Mathf.Abs(worldMousePosition.x) <= Camera.main.orthographicSize * 8 / 5;
        inRange = inRange && Mathf.Abs(worldMousePosition.y) <= Camera.main.orthographicSize;
        bool interacting = InteractGame.GetFromScreenSpace(Input.mousePosition) != null;
        Camera UICamera = GameObject.FindWithTag("UIInclude").GetComponent<Camera>();
        Collider2D hit = Physics2D.Raycast(UICamera.ScreenToWorldPoint(Input.mousePosition), UICamera.transform.forward).collider;
        interacting = interacting || (hit?.GetComponent<DialogueTextManager>() != null && hit.transform.parent.GetComponent<SpriteRenderer>().enabled);

        bool justClicked = currentLeftMouseDownNow == false && previousLeftMouseDown == true;

        if (justClicked && inRange == true && interacting == false && Timer.isPaused() == false)
        {
            // adjust?
            Vector3 CameraPos = Camera.main.transform.position;
            var dest = new Vector2(worldMousePosition.x + CameraPos.x, worldMousePosition.y + CameraPos.y);
            DoClickAndMoveTo(dest);
        }
        previousLeftMouseDown = currentLeftMouseDownNow;

        // use opportunity to try and move
        // for every FREQUENCY
        if (IsMouseButtonHeld &&
            interacting == false &&
            Timer.isPaused() == false &&
            (consecutiveHeldFrames - FRAMES_REQUIRED_FOR_HOLD_CLICK) % FRAMES_FOR_AUTO_MOVE_FREQUENCY == 0)
        {
            //auto move
            Debug.Log("AutoClick!");
            Vector3 CameraPos = Camera.main.transform.position;
            var dest = new Vector2(worldMousePosition.x + CameraPos.x, worldMousePosition.y + CameraPos.y);
            DoClickAndMoveTo(dest);
        }
    }

    private void DoClickAndMoveTo(Vector2 where)
    {
        //GameObject.Find("OrbPosition").transform.position = where;

        // pathfind
        var start = player.transform.position;
        var clickedRoom = world.GetRoomAt(where);
        if (player.CurrentRoom == null)
        {
            return;
        }
        if (clickedRoom == null || clickedRoom == player.CurrentRoom)
        {
            // If no room is clicked, just move in the current room.
            var currentRoom = player.CurrentRoom;
            var destination = currentRoom.ClampGlobal(where);
            var path = currentRoom.GetInteriorPathFrom(start, destination);
            player.MoveAlongPath(path);
        }
        else
        {
            // pathfind to other room
            var destination = clickedRoom.ClampGlobal(where);
            var path = world.GetExteriorPathFrom(start, destination);
            player.MoveAlongPath(path);
        }

    }
}
