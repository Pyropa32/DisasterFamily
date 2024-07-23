using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Diego;

public interface IRangeDependent {
    public void finish();
    public void cancel();
    public void start();
    public bool isInRange(UnityEngine.Vector2 player);
}

public class ItemInteraction : IRangeDependent {
    private Item heldItem;
    private int heldItemInd;
    private IInteractable interactable;
    private UnityEngine.Vector2 interactablePos;
    private float range;

    public bool isInRange(UnityEngine.Vector2 player) { return ((interactablePos - player).magnitude <= range); }
    public ItemInteraction(Item heldItem, int heldItemInd, IInteractable interactable, UnityEngine.Vector2 interactablePos, float range = 0.25f) {
        this.heldItem = heldItem;
        this.heldItemInd = heldItemInd;
        this.interactable = interactable;
        this.interactablePos = interactablePos;
        this.range = range;
    }
    public void finish() {
        interactable.OnInteract.Invoke(heldItem);
        InventoryManager.toggleActivity(heldItemInd);
    }
    public void cancel() {
        InventoryManager.toggleActivity(heldItemInd);
    }
    public void start() {
        InventoryManager.toggleActivity(heldItemInd);
    }
}

public class ItemDrop : IRangeDependent {
    private Item heldItem;
    private int heldItemInd;
    private UnityEngine.Vector2 targetPos;
    private float range;

    public bool isInRange(UnityEngine.Vector2 player) { return ((targetPos - player).magnitude <= range); }
    public ItemDrop(Item heldItem, int heldItemInd, UnityEngine.Vector2 targetPos, float range = 0.25f) {
        this.heldItem = heldItem;
        this.heldItemInd = heldItemInd;
        this.targetPos = targetPos;
        this.range = range;
    }
    public void finish() {
        DropItem(heldItem.ID, targetPos);
        InventoryManager.toggleActivity(heldItemInd);
    }
    public void cancel() {
        InventoryManager.toggleActivity(heldItemInd);
    }
    public void start() {
        InventoryManager.toggleActivity(heldItemInd);
    }

    public static void DropItem(int id, UnityEngine.Vector2 dropPos) {
        GameObject items = GameObject.Find("Items");
        GameObject itemObject = new GameObject();

        SpriteRenderer spriteRenderer = itemObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = ItemLookup.GetItemFromID(id).Sprite;

        itemObject.AddComponent<CircleCollider2D>();

        GeneralItem generalItem = itemObject.AddComponent<GeneralItem>();
        generalItem.OnInteract = generalItem.Action;
        itemObject.name = generalItem.name;

        Animator anim = itemObject.AddComponent<Animator>();
        anim.runtimeAnimatorController = InventoryManager.GetAnimatorController();

        InventoryManager.toggleInInventory(id);

        Vector2 gameSpacePosition = dropPos;
        // gameSpacePosition = GameObject.FindWithTag("Player").GetComponent<Prototypal.SimpleActor>().CurrentPlane.ClampGlobal(gameSpacePosition);
        itemObject.transform.position = new Vector3(gameSpacePosition.x, gameSpacePosition.y, 0.01f);
        itemObject.name = ItemLookup.GetItemFromID(id).Name;
        itemObject.GetComponent<GeneralItem>().id = id;
    }
}
