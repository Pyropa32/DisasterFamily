using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prototypal;
using Diego;
using Unity.VisualScripting;
public class WorldItem : MonoBehaviour
{
    private string weight = "";
    private bool collected = false;
    private SimpleActor SimpleActor;

    // serialize fields
    [SerializeField]
    int ID;

    [HideInInspector]
    public Item Data { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("loading world item called: " + name);
        if (ItemsUniverse.TryGetValue(ID, out Item data))
        {
            Data = data;
            // set sprite
            var spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
            {
                Debug.LogError("Hey!!! put a sprite renderer on this item!");
            }
            spriteRenderer.sprite = data.Sprite;
        }
        else
        {
            Debug.LogError("Hey!!! You!!! Look at ItemsUniverse.cs for item IDs! Cause " + ID + " aint nothing!");
        }

        GameObject SimpleActorObject = GameObject.FindWithTag("Player");
        if (SimpleActorObject != null)
        {
            SimpleActor = SimpleActorObject.GetComponent<SimpleActor>();
        }
        else
        {
            Debug.LogError("No SimpleActor found in scene.");
        }
        //Debug.Log("Item generated");
    }

    //load attributes from xml

    // Update is called once per frame
    void Update()
    {
        DetectClick();
    }


    // user should walk to item first to collect it (implement position on plane)

    void DetectClick()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f; // Ensure z is 0 since we are in 2D

            if (IsMouseOverSprite(mousePosition))
            {
                Debug.Log("Item clicked: " + gameObject.name);
                ClickToCollect();
                if (collected)
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }

    public bool IsMouseOverSprite(Vector3 mousePosition)
    {
        Vector2 clickPosition = Diego.CameraToScreenspaceConverter.GetGlobalMousePosition();
        GameObject.Find("REDLOC").transform.position = mousePosition;
        GameObject.Find("BLULOC").transform.position = clickPosition;
        
        // chad GBD
        if (Input.GetMouseButtonDown(0))
        {
            if (name == "batteries")
            {
                int a = 0;
            }
            // Convert mouse position to world space

            //think about alternative
            BoxCollider2D collider = GetComponent<BoxCollider2D>();
            if (collider == null)
            {
                Debug.LogError("Sprite " + name + " doesn't have a box collider!");
                return false;
            }

            if (Input.GetMouseButtonDown(0))
            {
                int a = 4;
            }

            Bounds bounds = collider.bounds;
            bounds.Expand(Vector3.forward * float.MaxValue);

            return bounds.Contains(clickPosition) || bounds.Contains(mousePosition);

        }
        return false;
    }

    public void ClickToCollect()
    {
        if (SimpleActor.Inventory.Count < 1)
        {
            SimpleActor.Inventory.AddItem(this);
            collected = true;
            Debug.Log("Item collected: " + gameObject.name);
        }
        else
        {
            Debug.Log("No inventory space");
        }
    }
}
