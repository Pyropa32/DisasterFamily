using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Diego
{
    public class InventoryManager : MonoBehaviour
    {
        public Item[] invIds;

        public int invSize = 5;

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
            invIds = new Item[invSize];
            for (int i = 0; i < invSize; i++)
            {
                invIds[i] = Item.Empty;
            }
        }

        public static bool toggleInInventory(Item item)
        {
            return toggleInInventory(item.ID);
        }

        public static bool toggleInInventory(int id)
        {
            bool found = false;
            ItemsUniverse.TryGetValue(id, out Item item);
            for (int i = 0; i < _singletonInstance.invIds.Length; i++)
            {
                if (found)
                {
                    _singletonInstance.invIds[i - 1] = _singletonInstance.invIds[i];
                    _singletonInstance.invIds[i] = Item.Empty;
                }
                if (_singletonInstance.invIds[i].Equals(item))
                {
                    found = true;
                    _singletonInstance.invIds[i] = Item.Empty;
                }
            }
            if (found || !(_singletonInstance.invIds[_singletonInstance.invIds.Length - 1].Equals(Item.Empty)))
            {
                return false;
            }
            for (int i = _singletonInstance.invIds.Length - 2; i >= 0; i--)
            {
                if (!_singletonInstance.invIds[i].Equals(Item.Empty))
                {
                    _singletonInstance.invIds[i + 1] = item;
                    return true;
                }
            }
            _singletonInstance.invIds[0] = item;
            return true;
        }

        public static bool removeInInventory(int id)
        {
            bool found = false;
            ItemsUniverse.TryGetValue(id, out Item item);
            for (int i = 0; i < _singletonInstance.invIds.Length; i++)
            {
                if (found)
                {
                    _singletonInstance.invIds[i - 1] = _singletonInstance.invIds[i];
                    _singletonInstance.invIds[i] = Item.Empty;
                }
                if (_singletonInstance.invIds[i].Equals(item))
                {
                    found = true;
                    _singletonInstance.invIds[i] = Item.Empty;
                }
            }
            if (found)
            {
                return true;
            }
            return false;
        }
        public static bool addInInventory(int id) {
            if (!(_singletonInstance.invIds[_singletonInstance.invIds.Length - 1].Equals(Item.Empty))) {
                return false;
            }
            Debug.Log(id);
            ItemsUniverse.TryGetValue(id, out Item item);
            for (int i = _singletonInstance.invIds.Length - 2; i >= 0; i--)
            {
                if (!_singletonInstance.invIds[i].Equals(Item.Empty))
                {
                    _singletonInstance.invIds[i + 1] = item;
                    return true;
                }
            }
            _singletonInstance.invIds[0] = item;
            return true;
        }
    }
}