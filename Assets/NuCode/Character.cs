using System.Collections;
using System.Collections.Generic;
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

    private Queue<MoveData> moveCommandsQueue = new Queue<MoveData>();
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
        var movement = new MoveData(startPos, finish, walkSpeed);
        moveCommandsQueue.Clear();
        moveCommandsQueue.Enqueue(movement);
    }

    /// <summary>
    /// Moves the character to each finish point one-by-one.
    /// </summary>
    /// <param name="points">Can be an Array, List, whatever the hell you want.</param>
    public void MoveAlongPath(IEnumerable<Vector2> points)
    {
        var startPos = transform.position;
        moveCommandsQueue.Clear();
        foreach (var point in points)
        {
            var movement = new MoveData(startPos, point, walkSpeed);
            moveCommandsQueue.Enqueue(movement);
            startPos = point;
        }
    }



    // Update is called once per frame
    // Fixed to physics
    void FixedUpdate()
    {
        if (moveCommandsQueue.Count < 1)
        {
            return;
        }
        var currentMove = moveCommandsQueue.Peek();
        currentMove.Tick();
        transform.position = currentMove.Value;
        if (currentMove.IsFinished)
        {
            moveCommandsQueue.Dequeue();
        }
    }
}
