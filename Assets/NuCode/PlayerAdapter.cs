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
    bool previousLeftMouseDown = false;
    // Start is called before the first frame update
    private Character player;
    private RoomGraph world;
    void Start()
    {
        player = GetComponent<Character>();
        world = GetComponentInParent<RoomGraph>();
    }

    // Update is called once per frame
    void Update()
    {
        var currentLeftMouseDown = Input.GetMouseButtonDown(0);
        // Maybe this works?
        Vector2 worldMousePosition = CameraToScreenspaceConverter.GetGameSpaceFromScreenSpace(Input.mousePosition);
        bool inRange = Mathf.Abs(worldMousePosition.x) <= Camera.main.orthographicSize * 8 / 5;
        inRange = inRange && Mathf.Abs(worldMousePosition.y) <= Camera.main.orthographicSize;
        bool interacting = InteractGame.GetFromScreenSpace(Input.mousePosition) != null;
        Camera UICamera = GameObject.FindWithTag("UIInclude").GetComponent<Camera>();
        Collider2D hit = Physics2D.Raycast(UICamera.ScreenToWorldPoint(Input.mousePosition), UICamera.transform.forward).collider;
        interacting = interacting || (hit?.GetComponent<DialogueTextManager>() != null && hit.transform.parent.GetComponent<SpriteRenderer>().enabled);
        //
        if (currentLeftMouseDown == false && previousLeftMouseDown == true && inRange == true && interacting == false)
        {
            // adjust?
            Vector3 CameraPos = Camera.main.transform.position;
            worldMousePosition += new Vector2(CameraPos.x, CameraPos.y);

            GameObject.Find("OrbPosition").transform.position = worldMousePosition;
            
            // pathfind
            var start = player.transform.position;
            var clickedRoom = world.GetRoomAt(worldMousePosition);
            if (player.CurrentRoom == null)
            {
                previousLeftMouseDown = currentLeftMouseDown;
                return;
            }
            if (clickedRoom == null || clickedRoom == player.CurrentRoom)
            {
                // If no room is clicked, just move in the current room.
                var currentRoom = player.CurrentRoom;
                var destination = currentRoom.ClampGlobal(worldMousePosition);
                var path = currentRoom.GetInteriorPathFrom(start, destination);
                player.MoveAlongPath(path);
            }
            else
            {
                // pathfind to other room
                var destination = clickedRoom.ClampGlobal(worldMousePosition);
                var path = world.GetExteriorPathFrom(start, destination);
                player.MoveAlongPath(path);
            }
            
        }
        previousLeftMouseDown = currentLeftMouseDown;
    }
}
