using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InventorySlotOnClick : MonoBehaviour {
    public UnityEvent onClick;
    private Vector3 origin;

    void OnMouseDown() {
        onClick.Invoke();
        origin = transform.position;
    }

    public void MoveSprite(Vector3 dest) {
        Vector3 vec = transform.GetChild(0).position;
        transform.position = new Vector3(dest.x, dest.y, transform.position.z);
        transform.GetChild(0).position = vec;
    }

    public void ResetPos() {
        MoveSprite(origin);
    }

    public void Apply(int id) {
        Transform hit = InteractGame.GetFromScreenSpace(Input.mousePosition);
        if (hit != null && hit.GetComponent<Interactable>() != null) {
            hit.GetComponent<Interactable>().getAction().Invoke(InventoryManager.GetItemFromID(id));
        }
        ResetPos();
    }
}
