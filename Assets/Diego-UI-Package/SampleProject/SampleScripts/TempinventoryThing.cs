using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempinventoryThing : MonoBehaviour {
    public InventoryManager IM;
    void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            toggle(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            toggle(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            toggle(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4)) {
            toggle(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5)) {
            toggle(4);
        }
    }

    private void toggle(int id) {
        bool found = false;
        for (int i = 0; i < IM.ids.Length; i++) {
            if (found) {
                IM.ids[i - 1] = IM.ids[i];
                IM.ids[i] = -1;
            }
            if (IM.ids[i] == id) {
                found = true;
                IM.ids[i] = -1;
            }
        }
        if (found || IM.ids[IM.ids.Length - 1] != -1) {
            return;
        }
        for (int i = IM.ids.Length - 2; i >= 0; i--) {
            if (IM.ids[i] != -1) {
                IM.ids[i + 1] = id;
                return;
            }
        }
        IM.ids[0] = id;
    }
}
