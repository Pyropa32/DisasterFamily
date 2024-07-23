using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Diego
{
    public class InventorySlotOnClick : MonoBehaviour
    {
        public int index = -1;
        private Vector3 origin;

        public void ItemAnimation() {
            transform.GetComponent<Animator>().SetTrigger("insert");
            Debug.Log("Item really should animate");
        }
        
        void OnMouseDown() {
            if (Timer.isPaused() || !InventoryManager.SingletonInstance.invActive[index]) {
                return;
            }
            // if onClick is null, don't invoke.
            origin = transform.position;
            if (index != -1) {
                transform.parent.GetComponent<InventoryBarManager>()?.clickInventory(index);
            }
        }

        public void MoveSprite(Vector3 dest)
        {
            Vector3 vec = transform.GetChild(0).position;
            transform.position = new Vector3(dest.x, dest.y, transform.position.z);
            transform.GetChild(0).position = vec;
        }

        public void ResetPos()
        {
            MoveSprite(origin);
        }

        public void Apply(int id)
        {
            Transform hit = CameraToScreenspaceConverter.GetFromScreenSpace(Input.mousePosition);
            if (hit != null && hit.GetComponent<IInteractable>() != null)
            {
                ItemsUniverse.TryGetValue(id, out Item i);
                hit.GetComponent<IInteractable>()?.GetInRangeAndDo(i, hit.transform.position);
            }
            else {
                MoveThenDrop(ItemLookup.GetItemFromID(id), Input.mousePosition, index);
            }
            ResetPos();
        }

        private static void MoveThenDrop(Item item, UnityEngine.Vector2 mousePos, int index) {
            if (!CameraToScreenspaceConverter.isInGameBounds(mousePos)) {
                return;
            }
            float range = 0.25f;
            //UnityEngine.Vector2 playerPivot = new UnityEngine.Vector2(0, 0.75f);
            var player = UnityEngine.GameObject.FindWithTag("Player").GetComponent<Character>();
            var world = UnityEngine.GameObject.FindWithTag("Player").GetComponentInParent<RoomGraph>();
            UnityEngine.Vector2 playerPos = UnityEngine.GameObject.FindWithTag("Player").transform.position;// + new UnityEngine.Vector3(playerPivot.x, playerPivot.y, 0);
            UnityEngine.Vector2 dropPos = Camera.main.transform.position;
            dropPos += CameraToScreenspaceConverter.GetGameSpaceFromScreenSpace(mousePos);
            ItemDrop dropAction = new ItemDrop(item, index, dropPos, range);

            if (Timer.isPaused() == false) {
                var start = UnityEngine.GameObject.FindWithTag("Player").transform.position;
                var clickedRoom = world.GetRoomAt(dropPos);
                if (player.CurrentRoom == null) {
                    return;
                }
                if (clickedRoom == null || clickedRoom == player.CurrentRoom) {
                    // If no room is clicked, just move in the current room.
                    var currentRoom = player.CurrentRoom;
                    var destination = currentRoom.ClampGlobal(dropPos);
                    var path = currentRoom.GetInteriorPathFrom(start, destination);
                    player.MoveAlongPath(path, dropAction);
                }
                else {
                    // pathfind to other room
                    var destination = clickedRoom.ClampGlobal(dropPos);
                    var path = world.GetExteriorPathFrom(start, destination);
                    player.MoveAlongPath(path, dropAction);
                }
            }
        }/*

        public static void ApplyNoItem()
        {
            Transform hit = CameraToScreenspaceConverter.GetFromScreenSpace(Input.mousePosition);
            if (hit != null && hit.GetComponent<IInteractable>() != null)
            {
                hit.GetComponent<IInteractable>()?.OnInteract(Item.Empty);
            }
        }*/
    }
}
