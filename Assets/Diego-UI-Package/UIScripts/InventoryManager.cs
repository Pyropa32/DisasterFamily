using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Diego
{
    public class InventoryManager : MonoBehaviour
    {
        public Item[] items;
        public Sprite[] sprites;

        public int[] invIds;
        public int[] globalInvIds;

        public int invSize = 5;
        public int globalInvSize = 20;

        public static InventoryManager instance = null;

        void Start()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
            invIds = new int[invSize];
            for (int i = 0; i < invSize; i++)
            {
                invIds[i] = -1;
            }
            globalInvIds = new int[globalInvSize];
            for (int i = 0; i < globalInvSize; i++)
            {
                globalInvIds[i] = -1;
            }
            items = new Item[sprites.Length];
            for (int i = 0; i < sprites.Length; i++)
            {
                items[i] = new Item(i, sprites[i]);
            }
        }

        public static Item GetItemFromID(int id)
        {
            return instance.items[id];
        }

        public static bool toggleInInventory(Item item)
        {
            return toggleInInventory(item.getID());
        }

        public static bool toggleInInventory(int id)
        {
            bool found = false;
            for (int i = 0; i < instance.invIds.Length; i++)
            {
                if (found)
                {
                    instance.invIds[i - 1] = instance.invIds[i];
                    instance.invIds[i] = -1;
                }
                if (instance.invIds[i] == id)
                {
                    found = true;
                    instance.invIds[i] = -1;
                }
            }
            if (found || instance.invIds[instance.invIds.Length - 1] != -1)
            {
                return false;
            }
            for (int i = instance.invIds.Length - 2; i >= 0; i--)
            {
                if (instance.invIds[i] != -1)
                {
                    instance.invIds[i + 1] = id;
                    return true;
                }
            }
            instance.invIds[0] = id;
            return true;
        }

        public static bool toggleInGlobalInventory(Item item)
        {
            return toggleInGlobalInventory(item.getID());
        }

        public static bool toggleInGlobalInventory(int id)
        {
            bool found = false;
            for (int i = 0; i < instance.globalInvIds.Length; i++)
            {
                if (found)
                {
                    instance.globalInvIds[i - 1] = instance.globalInvIds[i];
                    instance.globalInvIds[i] = -1;
                }
                if (instance.globalInvIds[i] == id)
                {
                    found = true;
                    instance.globalInvIds[i] = -1;
                }
            }
            if (found || instance.globalInvIds[instance.globalInvIds.Length - 1] != -1)
            {
                return false;
            }
            for (int i = instance.globalInvIds.Length - 2; i >= 0; i--)
            {
                if (instance.globalInvIds[i] != -1)
                {
                    instance.globalInvIds[i + 1] = id;
                    return true;
                }
            }
            instance.globalInvIds[0] = id;
            return true;
        }
    }
}