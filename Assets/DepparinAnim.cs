using System;
using UnityEngine;
[RequireComponent(typeof(Character), typeof(Animator), typeof(SpriteRenderer))]
public class DepparinAnim : MonoBehaviour
{
    private Character myCharacter;
    private SpriteRenderer sprite;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        myCharacter = GetComponent<Character>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (myCharacter.IsMoving)
        {
            animator.SetBool("moving", true);
        }
        else
        {
            animator.SetBool("moving", false);
        }
        var direction = myCharacter.MoveDirection;
        if (direction.x < 0f)
        {
            sprite.flipX = true;
        }
        else
        {
            sprite.flipX = false;
        }
    }
}
