using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Diego
{
    public class InventorySlotOnClick : MonoBehaviour
    {
        public int index = -1;
        private Vector3 origin;

        void OnMouseDown()
        {
            // if onClick is null, don't invoke.
            origin = transform.position;
            if (index != -1) {
                transform.parent.GetComponent<InventoryBarManager>()?.clickInventory(index);
            }
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
                ItemsUniverse.TryGetValue(id, out Item i);
                hit.GetComponent<IInteractable>()?.GetInRangeAndDo(i, hit.transform.position);
            }
            else {
                MoveThenDrop(ItemLookup.GetItemFromID(id), Input.mousePosition);
            }
            ResetPos();
        }

        private class DropAction : IStoryCommand {
            public bool started;
            public bool finished;
            public Item item;
            public event Action<System.Object> OnFinish;
            public bool IsFinished { get => finished; }
            public bool IsStarted { get => started; }
            public bool IsConcurrent { get => true; }
            public StoryCommandExecutionFlags ExecutionFlags => StoryCommandExecutionFlags.DiscardAlike |
                                                                StoryCommandExecutionFlags.DiscardConcurrent;

            public DropAction(Item item, Vector2 mousePos) {
                this.item = item;
                OnFinish = (object arg) => {
                    DropItem(item.ID, mousePos);
                };
            }
            public void Start() {
                OnFinish.Invoke(item);
                finished = true;
                started = true;
            }
            public void Tick(float delta) {
                // nothing
            }
            public object GetProgressModel() {
                if (IsStarted && IsFinished) {
                    return 1f;
                }
                return 0f;
            }
            public Type InstanceType => GetType();
        }

        private static void MoveThenDrop(Item item, UnityEngine.Vector2 mousePos) {
            if (!CameraToScreenspaceConverter.isInGameBounds(mousePos)) {
                return;
            }
            float range = 1f;
            UnityEngine.Vector2 playerPivot = new UnityEngine.Vector2(0, 0.75f);

            UnityEngine.Vector2 playerPos = UnityEngine.GameObject.FindWithTag("Player").transform.position + new UnityEngine.Vector3(playerPivot.x, playerPivot.y, 0);
            UnityEngine.Vector2 dropPos = Camera.main.transform.position;
            dropPos += CameraToScreenspaceConverter.GetGameSpaceFromScreenSpace(mousePos);
            UnityEngine.Vector2 displacement = dropPos - playerPos;
            if (displacement.magnitude <= range) {
                DropItem(item.ID, dropPos);
                return;
            }
            displacement *= 1 - (range / displacement.magnitude);
            List<IStoryCommand> after = new List<IStoryCommand>();
            after.Add(new DropAction(item, dropPos));
            UnityEngine.GameObject.FindWithTag("Player").GetComponent<ClickToMove>().OnClicked(displacement + playerPos - playerPivot, after);
        }

        public static void DropItem(int id, Vector2 dropPos) {
            //Item item = InventoryManager.GetItemFromID(id);
            //string name = item.GetName();
            //Debug.Log("Dropping item with name: " + item.GetName());
            GameObject items = GameObject.Find("Items");
            //var generatedItem = items.Instantiate();
            GameObject itemObject = new GameObject();
            itemObject.AddComponent<CircleCollider2D>();
            SpriteRenderer spriteRenderer = itemObject.AddComponent<SpriteRenderer>();

            spriteRenderer.sprite = ItemLookup.GetItemFromID(id).Sprite;
            GeneralItem generalItem = itemObject.AddComponent<GeneralItem>();
            generalItem.OnInteract = generalItem.Action;
            itemObject.name = generalItem.name;

            InventoryManager.toggleInInventory(id);

            Vector2 gameSpacePosition = dropPos;
            gameSpacePosition = GameObject.FindWithTag("Player").GetComponent<Prototypal.SimpleActor>().CurrentPlane.ClampGlobal(gameSpacePosition);
            itemObject.transform.position = new Vector3(gameSpacePosition.x, gameSpacePosition.y, -9.9f);
            itemObject.name = ItemLookup.GetItemFromID(id).Name;
            itemObject.GetComponent<GeneralItem>().id = id;
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
