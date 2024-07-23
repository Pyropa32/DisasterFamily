using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Character : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float walkSpeed;
    
    // PERFORMANT ELEMENT: Reduce number of times this is called.
    /// <summary>
    /// Expensive
    /// </summary>
    public Room CurrentRoom 
    {
        get
        {
            return World.GetRoomAt(transform.position);
        }
    }

    public RoomGraph World { get; protected set; }

    private MoveDataChain currentMovementQueue;
    void Start()
    {
        World = GetComponentInParent<RoomGraph>();
    }

    /// <summary>
    /// Moves the character to `finish`. Speed is determined by character's walk speed.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="finish"></param>
    public void MoveToPoint(Vector2 finish)
    {
        var startPos = transform.position;
        currentMovementQueue = new MoveDataChain(startPos, new Vector2[] {finish}, walkSpeed);
    }

    /// <summary>
    /// Moves the character to each finish point one-by-one.
    /// </summary>
    /// <param name="points">Can be an Array, List, whatever the hell you want.</param>
    public void MoveAlongPath(IEnumerable<Vector2> points)
    {
        var startPos = transform.position;
        currentMovementQueue = new MoveDataChain(startPos, points.ToArray(), walkSpeed);
    }



    // Update is called once per frame
    // Fixed to physics
    void FixedUpdate()
    {
        // movement chain context
        // wraps a list of movement commands
        // 
        if (currentMovementQueue != null && !currentMovementQueue.IsFinished)
        {
            transform.position = currentMovementQueue.Value;
            currentMovementQueue.Tick();
        }
    }
}
