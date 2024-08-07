using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prototypal;
using Diego;
using UnityEngine.UIElements;
public class ClickToMove : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private StoryCommandDispatcher dispatcher;
    private SimpleActor myActor;

    private bool _previousLeftMouseDown = false;
    void Start()
    {
        myActor = GetComponent<SimpleActor>();
    }

    private Vector2 GetViewportMousePosition()
    {
        //var resolution = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);
        var resolution = new Vector2(Screen.currentResolution.width,
                                    Screen.currentResolution.height);
        var actualScreenSize = new Vector2(Screen.width, Screen.height);
        var viewportMax = actualScreenSize / resolution;
        var magicVector = (Vector3)(Vector2.left * (0.05f));
        var clickedCoordinatesViewport = Camera.main.ScreenToViewportPoint(Input.mousePosition);

        // var transformedCoordinates = CameraToScreenspaceConverter.
        //                              SingletonInstance.
        //                              ViewportToWorld(clickedCoordinatesViewport);
        // return transformedCoordinates;
        var transformed = new Vector2(
            Mathf.Clamp01((clickedCoordinatesViewport.x / viewportMax.x) + magicVector.x),
            Mathf.Clamp01((clickedCoordinatesViewport.y / viewportMax.y) + magicVector.y)
        );
        return transformed;
    }

    // Update is called once per frame
    void Update()
    {
        var currentLeftMouseDown = Input.GetMouseButtonDown(0);
        Vector2 worldMousePosition = CameraToScreenspaceConverter.GetGameSpaceFromScreenSpace(Input.mousePosition);
        bool inRange = Mathf.Abs(worldMousePosition.x) <= Camera.main.orthographicSize * 8 / 5;
        inRange = inRange && Mathf.Abs(worldMousePosition.y) <= Camera.main.orthographicSize;
        bool interacting = InteractGame.GetFromScreenSpace(Input.mousePosition) != null;
        Camera UICamera = GameObject.FindWithTag("UIInclude").GetComponent<Camera>();
        Collider2D hit = Physics2D.Raycast(UICamera.ScreenToWorldPoint(Input.mousePosition), UICamera.transform.forward).collider;
        interacting = interacting || (hit?.GetComponent<DialogueTextManager>() != null && hit.transform.parent.GetComponent<SpriteRenderer>().enabled);
        if (currentLeftMouseDown == false && _previousLeftMouseDown == true && inRange == true && interacting == false && Timer.isPaused() == false)
        {
            Vector3 CameraPos = Camera.main.transform.position;
            worldMousePosition += new Vector2(CameraPos.x, CameraPos.y);
            OnClicked(worldMousePosition);
        }
        _previousLeftMouseDown = currentLeftMouseDown;
    }

    public void OnClicked(Vector2 where, List<IStoryCommand> after = null)
    {

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
                var firstPlane = currentPlanePair.StartPlane;
                var gateway = currentPlanePair.Gate;
                var secondPlane = currentPlanePair.DestinationPlane;
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

            // Add after commands
            for (int i = 0; after != null && i < after.Count; i++) {
                movementCommandChain.Add(after[i]);
            }

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


            List<IStoryCommand> movementCommandChain = new List<IStoryCommand>();

            movementCommandChain.Add(moveCommand);

            // Add after commands
            for (int i = 0; after != null && i < after.Count; i++) {
                movementCommandChain.Add(after[i]);
            }

            // Send all of the commands to the dispatcher.
            dispatcher.ReceiveSequentialRange(
                movementCommandChain,
                blocking: false,
                flagOverride: StoryCommandExecutionFlags.DiscardAlike
                );
        }

    }
}
