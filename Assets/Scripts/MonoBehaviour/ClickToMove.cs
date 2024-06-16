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

        // see if you clicked on another plane.
        var otherPlane = myActor.World.GetPlaneByPosition(where);
        
        if (otherPlane != myActor.CurrentPlane && otherPlane != null)
        {
            // get the list of planes to navigate when getting to the other plane.
            var externalPath = myActor.World.GetShortestExternalPath(myActor.CurrentPlane, otherPlane);
            
            // pathfinding failure
            if (!externalPath.Success)
            {
                Debug.Log("Failed to find path between " + myActor.CurrentPlane.name + " and " + otherPlane);
                return;
            }

            for (int i = 0; i < externalPath.Solution.Length; i++)
            {
                var current = externalPath.Solution[i];
                // construct walk commands 
            }
           
        }
        else
        {
            // move around in local space
            var planeCoordinates = myActor.CurrentPlane.ScreenToPlane(where); 
            var moveCommand = new MoveActorStoryCommand(myActor, myActor.LocalPosition, planeCoordinates, myActor.MovementSpeed);
            dispatcher.Receive(moveCommand);
        }

    }
}
