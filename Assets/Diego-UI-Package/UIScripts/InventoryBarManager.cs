using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Diego
{
    public class InventoryBarManager : MonoBehaviour
    {
        private InventoryManager IM;
        public SpriteRenderer[] children;
        public InventorySlotOnClick[] childrenSlots;

        private int onMouse = -1;

        void Start()
        {
            IM = GameObject.FindWithTag("InventoryManager").GetComponent<InventoryManager>();
            childrenSlots = transform.GetComponentsInChildren<InventorySlotOnClick>();
        }

        void Update()
        {
            for (int i = 0; i < IM.invIds.Length; i++)
            {
                if (IM.invIds[i] == -1)
                {
                    children[i].enabled = false;
                    continue;
                }
                children[i].enabled = (i != -1);
                children[i].sprite = IM.sprites[IM.invIds[i]];
            }
            if (onMouse != -1 && Input.GetMouseButton(0))
            {
                Vector3 worldSpace = new Vector3(Input.mousePosition.x / Screen.width - 0.5f, Input.mousePosition.y / Screen.height - 0.5f, 0);
                worldSpace.x *= -16;
                worldSpace.y *= 10;
                childrenSlots[onMouse].MoveSprite(worldSpace);
            }
            else if (onMouse != -1)
            {
                Vector3 worldSpace = new Vector3(Input.mousePosition.x / Screen.width - 0.5f, Input.mousePosition.y / Screen.height - 0.5f, 0);
                worldSpace.x *= -16;
                worldSpace.y *= 10;
                childrenSlots[onMouse].Apply(IM.invIds[onMouse]);
                onMouse = -1;
            }
        }

        public void clickInventory(int inventoryInd)
        {
            if (IM.invIds[inventoryInd] != -1)
            {
                onMouse = inventoryInd;
            }
        }
    }
}