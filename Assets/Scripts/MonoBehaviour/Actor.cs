using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    [SerializeField]
    float movementSpeed = 2f;

    [SerializeField]
    OrthographicPlane currentPlane;
    [SerializeField]
    Transform footPosition;
    Vector2 localPosition;

    void Start()
    {
        Debug.Log("hello, world!");
        currentPlane.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        localPosition = Vector2.one / 2f;
        if (Input.GetKey(KeyCode.W))
        {
            localPosition += Vector2.up / 2f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            localPosition += Vector2.right / 2f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            localPosition += Vector2.left / 2;
        }
        if (Input.GetKey(KeyCode.S))
        {
            localPosition += Vector2.down / 2;
        }

        GlobalPosition = currentPlane.PlaneToScreen(localPosition);
    }

    public void SetAnim(string what)
    {

    }

    public OrthographicPlane CurrentPlane => currentPlane;
    public float MovementSpeed => movementSpeed;

    public Vector2 GlobalPosition
    {
        get
        {
            return new Vector2(footPosition.position.x, footPosition.position.y);
        }
        set
        {
            transform.position = new Vector3(value.x - footPosition.localPosition.x, value.y - footPosition.localPosition.y, -0.2f);
        }
    }

    // TODO: Actually make sure to convert to plane coordinates.
    public Vector2 LocalPosition
    {
        get
        {
            return localPosition;
        }
        set
        {
            localPosition = value;
        }
    }
}
