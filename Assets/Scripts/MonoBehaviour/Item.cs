using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private string weight = "";
    private ItemCollection itemCollection;
    private bool collected = false;
    private Actor actor;

    // Start is called before the first frame update
    void Start()
    {
        GameObject actorObject = GameObject.FindWithTag("Player");
        if (actorObject != null)
        {
            actor = actorObject.GetComponent<Actor>();
        }
        else
        {
            Debug.LogError("No actor found in scene.");
        }
        Debug.Log("Item generated");
        itemCollection = GetComponent<ItemCollection>();
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

    void ClickToCollect()
    {
        if (actor.inventory.Count < 1)
        {
            actor.inventory.AddItem(this);
            collected = true;
            Debug.Log("Item collected: " + gameObject.name);
        }
        else
        {
            Debug.Log("No inventory space");
        }
    }
}
