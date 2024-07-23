using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
        if (previousLeftMouseDown && !currentLeftMouseDown)
        {
            // 
            // mouse pos

            var globalMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //
            // pathfind
            var start = player.transform.position;
            var clickedRoom = world.GetRoomAt(globalMousePos);
            if (clickedRoom == null)
            {
                // If no room is clicked, just move in the current room.
                var currentRoom = player.CurrentRoom;
                var destination = currentRoom.ClampGlobal(globalMousePos);
                var path = currentRoom.GetInteriorPathFrom(start, destination);
                player.MoveAlongPath(path);
            }
            else
            {
                // pathfind to other room
                var destination = clickedRoom.ClampGlobal(globalMousePos);
                var path = world.GetExteriorPathFrom(start, destination);
                player.MoveAlongPath(path);
            }
            
        }
        previousLeftMouseDown = currentLeftMouseDown;
    }
}
