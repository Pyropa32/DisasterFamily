using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Diego
{
    public class InventorySlotOnClick : MonoBehaviour
    {
        public UnityEvent onClick;
        private Vector3 origin;

        void OnMouseDown()
        {
            // if onClick is null, don't invoke.
            onClick?.Invoke();
            origin = transform.position;
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
                hit.GetComponent<IInteractable>()?.OnInteract(InventoryManager.GetItemFromID(id));
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
    }
}