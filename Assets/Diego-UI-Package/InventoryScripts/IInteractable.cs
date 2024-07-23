using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Diego;

public interface IInteractable : ISelectable {
    public Action<Item> OnInteract { get; set; }
    public float Range { get; }
    public void GetInRangeAndDo(Item item, Vector2 itemPos) {
        if (!CameraToScreenspaceConverter.isInGameBounds(Input.mousePosition)) {
            return;
        }
        float range = Range;
        //Vector2 playerPivot = new Vector2(0, 0.75f);
        var player = GameObject.FindWithTag("Player").GetComponent<Character>();
        var world = GameObject.FindWithTag("Player").GetComponentInParent<RoomGraph>();
        Vector2 playerPos = GameObject.FindWithTag("Player").transform.position;// + new Vector3(playerPivot.x, playerPivot.y, 0);
        Vector2 dropPos = Camera.main.transform.position;
        dropPos += CameraToScreenspaceConverter.GetGameSpaceFromScreenSpace(Input.mousePosition);
        ItemInteraction interactAction = new ItemInteraction(item, InventoryManager.getIndInInv(item), this, dropPos, range);

        if (Timer.isPaused() == false) {
            var start = GameObject.FindWithTag("Player").transform.position;
            var clickedRoom = world.GetRoomAt(dropPos);
            if (player.CurrentRoom == null) {
                return;
            }
            if (clickedRoom == null || clickedRoom == player.CurrentRoom) {
                // If no room is clicked, just move in the current room.
                var currentRoom = player.CurrentRoom;
                var destination = currentRoom.ClampGlobal(dropPos);
                var path = currentRoom.GetInteriorPathFrom(start, destination);
                player.MoveAlongPath(path, interactAction);
            }
            else {
                // pathfind to other room
                var destination = clickedRoom.ClampGlobal(dropPos);
                var path = world.GetExteriorPathFrom(start, destination);
                player.MoveAlongPath(path, interactAction);
            }
        }
    }
}