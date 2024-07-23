using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
namespace Diego
{
    public class InventoryManager : MonoBehaviour {
        public Item[] invIds;
        public bool[] invActive;

        public int invSize = 5;

        public static InventoryManager SingletonInstance { get => _singletonInstance; }
        private static InventoryManager _singletonInstance;
        public static event Action<int> OnSlotAnimationRequested;
        public RuntimeAnimatorController GeneralItemAnimController;

        void Start()
        {
            if (_singletonInstance != null)
            {
                Destroy(gameObject);
                return;
            }
            _singletonInstance = this;
            invIds = new Item[invSize];
            for (int i = 0; i < invSize; i++) {
                invIds[i] = Item.Empty;
            }
            invActive = new bool[invSize];
            for (int i = 0; i < invSize; i++) {
                invActive[i] = true;
            }
        }

        public static void toggleActivity(int ind) {
            if (ind == -1) {
                return;
            }
            _singletonInstance.invActive[ind] = !_singletonInstance.invActive[ind];
        }
        public static int getIndInInv(Item item) {
            if (item.Equals(Item.Empty)) {
                return -1;
            }
            for (int i = 0; i < _singletonInstance.invSize; i++) {
                if (item.Equals(_singletonInstance.invIds[i])) {
                    return i;
                }
            }
            return -1;
        }

        public static RuntimeAnimatorController GetAnimatorController() {
            return _singletonInstance.GeneralItemAnimController;
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
            ItemsUniverse.TryGetValue(id, out Item item);
            for (int i = _singletonInstance.invIds.Length - 2; i >= 0; i--)
            {
                if (!_singletonInstance.invIds[i].Equals(Item.Empty))
                {
                    _singletonInstance.invIds[i + 1] = item;
                    OnSlotAnimationRequested?.Invoke(i + 1);
                    return true;
                }
            }
            _singletonInstance.invIds[0] = item;
            OnSlotAnimationRequested?.Invoke(0);
            return true;
        }
    }
}