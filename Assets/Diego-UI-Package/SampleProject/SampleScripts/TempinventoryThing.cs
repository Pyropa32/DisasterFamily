using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Diego
{
    public class TempinventoryThing : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                InventoryManager.toggleInInventory(0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                InventoryManager.toggleInInventory(1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                InventoryManager.toggleInInventory(2);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                InventoryManager.toggleInInventory(3);
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                InventoryManager.toggleInInventory(4);
            }
        }
    }
}
