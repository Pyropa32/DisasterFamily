using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField]
    OrthographicPlaneGraph world;
    [SerializeField]
    Vector2Int cellSize;
    private OrthographicPlane currentPlane;
    public Vector2Int CellSize => cellSize;
    public OrthographicPlaneGraph World => world;
    public OrthographicPlane CurrentPlane => currentPlane;
    void Start()
    {
        currentPlane = world.GetPlaneByPosition(transform.position);
        if (currentPlane == null)
        {
            throw new InvalidOperationException("Failed to place obstacle! Not located on a plane!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
