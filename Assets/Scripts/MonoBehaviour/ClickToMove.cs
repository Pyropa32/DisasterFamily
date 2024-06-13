using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickToMove: MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private StoryCommandDispatcher dispatcher;
    private Actor myActor;

    private bool _previousLeftMouseDown = false;
    void Start()
    {
        myActor = GetComponent<Actor>();
    }

    // Update is called once per frame
    void Update()
    {
        var currentLeftMouseDown = Input.GetMouseButtonDown(0);
        if (currentLeftMouseDown == false && _previousLeftMouseDown == true)
        {
            Debug.Log("Local Mouse Position:" + Input.mousePosition);
            Debug.Log("Global Mouse Position: " + Input.mousePosition + transform.position);
            //TODO: Remove
            OnClicked(Input.mousePosition + transform.position);
            
        }
        _previousLeftMouseDown = currentLeftMouseDown;
    }

    private void SendToDispatcher()
    {

    }

    private void OnClicked(Vector2 where)
    {
        // Get the coordinates expressed in plane coords.
        // Create list of move commands using the Graph, with A* pathfinding.
        // Send the move commands to the dispatcher.

    }
}
