using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prototypal;

public class Item : MonoBehaviour
{
    private string weight = "";
    private bool collected = false;
    private SimpleActor SimpleActor;

    // Start is called before the first frame update
    void Start()
    {
        GameObject SimpleActorObject = GameObject.FindWithTag("Player");
        if (SimpleActorObject != null)
        {
            SimpleActor = SimpleActorObject.GetComponent<SimpleActor>();
        }
        else
        {
            Debug.LogError("No SimpleActor found in scene.");
        }
        Debug.Log("Item generated");
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

    bool IsMouseOverSprite(Vector3 mousePosition)
    {
        //think about alternative
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            return false;
        }

        Bounds bounds = spriteRenderer.bounds;
        return bounds.Contains(mousePosition);
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
