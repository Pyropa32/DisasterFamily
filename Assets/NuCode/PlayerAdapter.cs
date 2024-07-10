using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Put this component on the character in order to make them the player.
/// </summary>
[RequireComponent(typeof(Character))]
public class PlayerAdapter : MonoBehaviour
{
    bool previousLeftMouseDown = false;
    // Start is called before the first frame update
    private Character player;
    void Start()
    {
        player = GetComponent<Character>();
    }

    // Update is called once per frame
    void Update()
    {
        var currentLeftMouseDown = Input.GetMouseButtonDown(0);
        if (previousLeftMouseDown && !currentLeftMouseDown)
        {
            // 
            // Get Global Mouse Pos
            // 

            var globalMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            //
            Debug.Log("player walked");
            player.MoveTo(globalMousePos);
        }
        previousLeftMouseDown = currentLeftMouseDown;
    }
}
