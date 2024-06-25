using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Diego
{
    public class InventoryManager : MonoBehaviour
    {
        // What I really need
        // Injected
        public Item[] items;
        public Sprite[] sprites;

        public int[] invIds;
        public int[] globalInvIds;

        // Keep
        public int invSize = 5;
        // Destroy
        public int globalInvSize = 20;

        public static InventoryManager SingletonInstance { get => _singletonInstance; }
        private static InventoryManager _singletonInstance;
        void Start()
        {
            if (_singletonInstance != null)
            {
                Destroy(gameObject);
                return;
            }
            _singletonInstance = this;
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
            return _singletonInstance.items[id];
        }

        public static bool toggleInInventory(Item item)
        {
            return toggleInInventory(item.ID);
        }

        public static bool toggleInInventory(int id)
        {
            bool found = false;
            for (int i = 0; i < _singletonInstance.invIds.Length; i++)
            {
                if (found)
                {
                    _singletonInstance.invIds[i - 1] = _singletonInstance.invIds[i];
                    _singletonInstance.invIds[i] = -1;
                }
                if (_singletonInstance.invIds[i] == id)
                {
                    found = true;
                    _singletonInstance.invIds[i] = -1;
                }
            }
            if (found || _singletonInstance.invIds[_singletonInstance.invIds.Length - 1] != -1)
            {
                return false;
            }
            for (int i = _singletonInstance.invIds.Length - 2; i >= 0; i--)
            {
                if (_singletonInstance.invIds[i] != -1)
                {
                    _singletonInstance.invIds[i + 1] = id;
                    return true;
                }
            }
            _singletonInstance.invIds[0] = id;
            return true;
        }

        public static bool toggleInGlobalInventory(Item item)
        {
            return toggleInGlobalInventory(item.ID);
        }

        public static bool toggleInGlobalInventory(int id)
        {
            bool found = false;
            for (int i = 0; i < _singletonInstance.globalInvIds.Length; i++)
            {
                if (found)
                {
                    _singletonInstance.globalInvIds[i - 1] = _singletonInstance.globalInvIds[i];
                    _singletonInstance.globalInvIds[i] = -1;
                }
                if (_singletonInstance.globalInvIds[i] == id)
                {
                    found = true;
                    _singletonInstance.globalInvIds[i] = -1;
                }
            }
            if (found || _singletonInstance.globalInvIds[_singletonInstance.globalInvIds.Length - 1] != -1)
            {
                return false;
            }
            for (int i = _singletonInstance.globalInvIds.Length - 2; i >= 0; i--)
            {
                if (_singletonInstance.globalInvIds[i] != -1)
                {
                    _singletonInstance.globalInvIds[i + 1] = id;
                    return true;
                }
            }
            _singletonInstance.globalInvIds[0] = id;
            return true;
        }
    }
}