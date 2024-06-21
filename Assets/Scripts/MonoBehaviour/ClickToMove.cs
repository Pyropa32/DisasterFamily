using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickToMove: MonoBehaviour
{/*
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
            var worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            OnClicked(worldMousePosition);
            
        }
        _previousLeftMouseDown = currentLeftMouseDown;
    }

    private void SendToDispatcher()
    {

    }

    private void OnClicked(Vector2 where)
    {
        // Get the coordinates expressed in plane coords.
        var planeCoordinates = myActor.CurrentPlane.ScreenToPlane(where); 
        // Create list of move commands using the Graph, with A* pathfinding.
        // FIXME: For now, since no Pathfinding exists, just create one Move command.
        var moveCommand = new MoveActorStoryCommand(myActor, myActor.LocalPosition, planeCoordinates, myActor.MovementSpeed);
        // Send the move commands to the dispatcher.
        dispatcher.Receive(moveCommand);

    }*/
}
