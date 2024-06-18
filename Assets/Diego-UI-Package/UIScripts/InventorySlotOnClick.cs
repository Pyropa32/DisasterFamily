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

    public void Apply(Vector3 loc, int id) { // location and id of dropped item
        ResetPos();
    }
}
