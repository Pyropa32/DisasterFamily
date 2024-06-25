using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField]
    FloorPlaneGraph world;
    [SerializeField]
    Vector2Int cellSize;
    private FloorPlane currentPlane;
    public Vector2Int CellSize => cellSize;
    public FloorPlaneGraph World => world;
    public FloorPlane CurrentPlane => currentPlane;
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
