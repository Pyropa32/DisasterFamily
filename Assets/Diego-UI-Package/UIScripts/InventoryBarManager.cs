using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryBarManager : MonoBehaviour {
    private InventoryManager IM;
    public SpriteRenderer[] children;
    public InventorySlotOnClick[] childrenSlots;

    public int onMouse = -1;

    void Start() {
        IM = GameObject.FindWithTag("InventoryManager").GetComponent<InventoryManager>();
        childrenSlots = transform.GetComponentsInChildren<InventorySlotOnClick>();
    }

    void Update() {
        UITextManager.Clear();
        for (int i = 0; i < IM.invIds.Length; i++) {
            if (IM.invIds[i] == -1) {
                children[i].enabled = false;
                continue;
            }
            children[i].enabled = (i != -1);
            children[i].sprite = IM.sprites[IM.invIds[i]];
        }
        if (onMouse != -1 && Input.GetMouseButton(0)) {
            Vector3 worldSpace = new Vector3(Input.mousePosition.x / Screen.width - 0.5f, Input.mousePosition.y / Screen.height - 0.5f, 0);
            worldSpace.x *= -16;
            worldSpace.y *= 10;
            childrenSlots[onMouse].MoveSprite(worldSpace+transform.parent.position);
            UITextManager.SetText(ItemLookup.GetItemFromID(IM.invIds[onMouse]).getSprite().name);
        }
        else if (onMouse != -1) {
            childrenSlots[onMouse].Apply(IM.invIds[onMouse]);
            onMouse = -1;
        }
        else if (Input.GetMouseButtonUp(0)) {
            Vector2 worldSpace = InteractGame.GetGameSpaceFromScreenSpace(Input.mousePosition);
            bool inRange = Mathf.Abs(worldSpace.x) <= Camera.main.orthographicSize * 8 / 5;
            inRange = inRange && Mathf.Abs(worldSpace.y) <= Camera.main.orthographicSize;
            if (inRange) {
                Transform hit = InteractGame.GetFromScreenSpace(Input.mousePosition);
                if (hit != null && hit.GetComponent<Interactable>() != null) {
                    hit.GetComponent<Interactable>().GetInRangeAndDo(null, hit.position);
                }
            }
        }
        else {
            Vector2 gameSpace = InteractGame.GetGameSpaceFromScreenSpace(Input.mousePosition);
            bool inRange = Mathf.Abs(gameSpace.x) <= Camera.main.orthographicSize * 8 / 5;
            inRange = inRange && Mathf.Abs(gameSpace.y) <= Camera.main.orthographicSize;
            if (!inRange) {
                Vector3 worldSpace = new Vector3(Input.mousePosition.x / Screen.width - 0.5f, Input.mousePosition.y / Screen.height - 0.5f, 0);
                worldSpace.x *= -16;
                worldSpace.y *= 10;
                RaycastHit2D hit = Physics2D.GetRayIntersection(new Ray(transform.parent.position + worldSpace, transform.parent.forward));
                if (hit.collider != null && hit.collider.transform.GetComponent<InventorySlotOnClick>() != null && IM.invIds[(hit.collider.transform.GetComponent<InventorySlotOnClick>().index)] > -1) {
                    UITextManager.SetText(ItemLookup.GetItemFromID(IM.invIds[(hit.collider.transform.GetComponent<InventorySlotOnClick>().index)]).getSprite().name); // TODO: change to use xml (ask dylan)
                }
            }
            else {
                Transform hit = InteractGame.GetFromScreenSpace(Input.mousePosition);
                if (hit != null && hit.GetComponent<Interactable>() != null) {
                    UITextManager.SetText(hit.gameObject.name);
                }
            }
        }
    }

    public void clickInventory(int inventoryInd) {
        if (IM.invIds[inventoryInd] != -1) {
            onMouse = inventoryInd;
        }
    }
}
