using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    // TODO:
    // 1. Have ClickToMove send 20,20 to me.
    // 2. Get mouse coordinates and send them here repeatedly.
    // 3. On click, get mouse coordinates ,print once.
    // 4. Communicate with StoryCommandDispatcher.
    // 5. Send a MoveCommand to StoryCommandDispatcher.
    // 6. This should get me to move.

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetAnim(string what)
    {

    }

    public Vector2 GlobalPosition
    {
        get
        {
            return new Vector2(transform.position.x, transform.position.y);
        }
        set
        {
            transform.position = new Vector3(value.x, value.y);

        }
    }

    // TODO: Actually make sure to convert to plane coordinates.
    public Vector2 LocalPosition
    {
        get
        {
            return new Vector2(transform.position.x, transform.position.y);
        }
        set
        {
            transform.position = new Vector3(value.x, value.y);

        }
    }
}
