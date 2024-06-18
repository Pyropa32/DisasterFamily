using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickToMove : MonoBehaviour
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

    private void OnClicked(Vector2 where)
    {
        // Get the coordinates expressed in plane coords.

        // see if you clicked on another plane.
        var otherPlane = myActor.World.GetPlaneByPosition(where);

        if (otherPlane != myActor.CurrentPlane && otherPlane != null)
        {
            // get the location of the click for otherPlane.
            var clickedCoordinates = otherPlane.ScreenToPlane(where);
            // get the list of planes to navigate when getting to the other plane.
            var externalPath = myActor.World.GetShortestExternalPath(myActor.CurrentPlane, otherPlane);

            // pathfinding failure
            if (!externalPath.Success)
            {
                Debug.Log("Failed to find path between " + myActor.CurrentPlane.name + " and " + otherPlane);
                return;
            }

            List<IStoryCommand> movementCommandChain = new List<IStoryCommand>();

            var lastPosition = myActor.LocalPosition;

            for (int i = 0; i < externalPath.Solution.Length; i++)
            {
                var currentPlanePair = externalPath.Solution[i];
                var firstPlane = currentPlanePair.Item1;
                var gateway = currentPlanePair.Item2;
                var secondPlane = currentPlanePair.Item3;
                // move towards gateway
                // on finish, set plane to secondPlane;
                Vector2 startPosition = Vector2.zero;
                if (i == 0)
                {
                    startPosition = myActor.LocalPosition;
                }
                else
                {
                    startPosition = lastPosition;
                }
                var destination = firstPlane.ScreenToPlane(gateway.transform.position);

                // this argument (adjacent) is what causes the gateway change.
                var moveCommand = new MoveActorStoryCommand(
                    myActor,
                    startPosition,
                    destination,
                    myActor.MovementSpeed,
                    _adjacent: secondPlane
                );

                movementCommandChain.Add(moveCommand);

                lastPosition = secondPlane.ScreenToPlane(gateway.transform.position);
            }
            // The last movement command in this series is just going to be a basic move command
            var internalMoveCommand = new MoveActorStoryCommand(myActor,
                                                                lastPosition,
                                                                otherPlane.ScreenToPlane(where),
                                                                myActor.MovementSpeed
                                                                );
            movementCommandChain.Add(internalMoveCommand);
            // Send all of the commands to the dispatcher.
            dispatcher.ReceiveSequentialRange(
                movementCommandChain,
                blocking:false,
                flagOverride:StoryCommandExecutionFlags.DiscardAlike
                );
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
