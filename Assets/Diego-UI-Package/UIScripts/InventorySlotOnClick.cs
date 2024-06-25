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

        void OnMouseDown()
        {
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
                hit.GetComponent<IInteractable>()?.OnInteract(i);
            }
            else {
                DropItem(id);
            }
            ResetPos();
        }

        public static void ApplyNoItem()
        {
            Transform hit = CameraToScreenspaceConverter.GetFromScreenSpace(Input.mousePosition);
            if (hit != null && hit.GetComponent<IInteractable>() != null)
            {
                hit.GetComponent<IInteractable>()?.OnInteract(Item.Empty);
            }
        }

        public static void DropItem(int id) {
            //Item item = InventoryManager.GetItemFromID(id);
            //string name = item.GetName();
            //Debug.Log("Dropping item with name: " + item.GetName());
            GameObject itemObject = new GameObject();
            itemObject.AddComponent<CircleCollider2D>();
            SpriteRenderer spriteRenderer = itemObject.AddComponent<SpriteRenderer>();

            spriteRenderer.sprite = ItemLookup.GetItemFromID(id).Sprite;
            GeneralItem generalItem = itemObject.AddComponent<GeneralItem>();
            generalItem.OnInteract = generalItem.Action;
            itemObject.name = generalItem.name;

            InventoryManager.toggleInInventory(id);

            Vector2 gameSpacePosition = CameraToScreenspaceConverter.GetGameSpaceFromScreenSpace(Input.mousePosition);
            itemObject.transform.position = new Vector3(gameSpacePosition.x, gameSpacePosition.y, 0);
        }
    }
}
