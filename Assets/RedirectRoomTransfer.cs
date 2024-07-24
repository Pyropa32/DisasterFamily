using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Functionality for stairs
[RequireComponent(typeof(Room))]
public class RedirectRoomTransfer : MonoBehaviour
{
    private Room myRoom;
    /// <summary>
    /// When using a gate where myRoom is the ENTRANCE, take me to entranceRedirect instead.
    /// </summary>
    [SerializeField]
    private Room entranceRedirect;
    /// <summary>
    /// When using a gate where myRoom is the EXIT, take me to exitRedirect instead.
    /// </summary>
    [SerializeField]
    private Room exitRedirect;
    private RoomDoorway myRoomToEntranceRedirect;
    private RoomDoorway myRoomToExitRedirect;
    private RoomGraph world;
    public bool IsValid { get; protected set; }
    private bool hasLinkedEntranceExit = false;
    /// <summary>
    /// Get the path that's used to redirect from this room to the possible redirection rooms.
    /// The latest position must be referenced in order to select the correct redirect.
    /// </summary>
    /// <param name="latest"></param>
    public Vector2[] GetRedirectionPathFrom(Vector2 latestPosition)
    {
        var latestRoom = world.GetRoomAt(latestPosition);
        if (latestRoom == null)
        {
            throw new ArgumentException("latestPosition " + latestPosition + " does not yield a room!");
        }
        var relevant = latestRoom.TryGetDoorwayTo(myRoom, out var thisDoorway);
        if (!relevant)
        {
            throw new InvalidOperationException("Could not redirect! latestRoom" + latestRoom.name + "is not adjacent to this room!");
        }
        Room targetRoom;
        if (thisDoorway == myRoomToEntranceRedirect)
        {
            targetRoom = exitRedirect;
        }
        else if (thisDoorway == myRoomToExitRedirect)
        {
            targetRoom = entranceRedirect;
        } 
        else
        {
            throw new InvalidOperationException("shared doorway somehow is not the one between the entrance/exit redirect.");
        }
        // navigate to room center.
        var targetDestination = targetRoom.Center;
        return world.GetExteriorPathFrom(latestPosition, targetDestination);
    }
    // Start is called before the first frame update
    void Start()
    {
        myRoom = GetComponent<Room>();
        if (entranceRedirect == exitRedirect)
        {
            throw new InvalidOperationException("entrance and exit redirects are identical!");
        }
        if (entranceRedirect == null || exitRedirect == null)
        {
            throw new InvalidOperationException("one of the redirects is null!");
        }
        if (entranceRedirect == myRoom || exitRedirect == myRoom)
        {
            throw new InvalidOperationException("redirection cannot point to itself!");
        }
        world = GetComponentInParent<RoomGraph>();
        if (world == null)
        {
            throw new InvalidOperationException("No world parent found for RedirectRoomTransfer " + name);
        }
    }
    

    void Update()
    {
        if (world.IsSetupComplete && !hasLinkedEntranceExit)
        {
            // give me the gates on this room that link to the enter/exit.
            // When approaching this room from Gate1, go to room1, vice-versa
            if (myRoom.GetDoorways().Length != 2)
            {
                throw new InvalidOperationException("Redirect Room Transfer can only be used on a room with 2 doors");
            }
            myRoom.TryGetDoorwayTo(entranceRedirect, out myRoomToEntranceRedirect);
            myRoom.TryGetDoorwayTo(exitRedirect, out myRoomToExitRedirect);

            IsValid = true;
        }
    }
}
