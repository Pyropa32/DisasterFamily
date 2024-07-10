using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    /// <summary>
    /// Moves the character to `finish`. Speed is determined by character's walk speed.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="finish"></param>
    public void MoveTo(Vector2 finish)
    {

    }

    private Vector2 MoveToward(Vector2 from, Vector2 to, float delta)
    {
        Vector2 v = from;
        Vector2 vd = to - v;
        float len = vd.magnitude;
        if (len <= delta || len < Mathf.Epsilon)
        {
            return to;
        }
        else
        {
            return v + (vd / len * delta);
        }
    }

    // Update is called once per frame
    // Fixed to physics
    void FixedUpdate()
    {
        
    }
}
