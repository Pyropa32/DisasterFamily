using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorPlaneGateway : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    FloorPlane from;
    [SerializeField]
    FloorPlane to;
    private bool worldHasBeenSet = false;
    FloorPlaneGraph world;

    public uint ID { get; protected set; }

    static uint _gatewayCurrentID = 1;

    public void SetWorld(FloorPlaneGraph to)
    {
        if (worldHasBeenSet)
        {
            throw new InvalidOperationException("This gateway already has a Plane Graph associated with it.");
        }
        world = to;
        worldHasBeenSet = true;
    }

    public FloorPlane FromPlane => from;
    public FloorPlane ToPlane => to;

    public int Distance2To(FloorPlaneGateway what)
    {
        return (int)((transform.position.x - what.transform.position.x) * (transform.position.x - what.transform.position.x) +
                (transform.position.y - what.transform.position.y) * (transform.position.y - what.transform.position.y));
    }

    public bool SharesPlaneWith(FloorPlaneGateway with)
    {
        return TryGetSharedPlane(with, out _);
    }

    public bool TryGetSharedPlane(FloorPlaneGateway with, out FloorPlane result)
    {
        if (with.FromPlane == FromPlane ||
            with.FromPlane == ToPlane)
        {
            result = with.FromPlane;
        }
        else if (with.ToPlane == ToPlane ||
                 with.ToPlane == FromPlane)
        {
            result = with.ToPlane;
        }
        else
        {
            result = null;
            return false;
        }
        return true;
    }

    void Awake()
    {
        ID = _gatewayCurrentID;
        _gatewayCurrentID += 1;

        if (ToPlane == null)
        {
            throw new InvalidOperationException("ToPlane of " + name + " not set!");
        }

        if (FromPlane == null)
        {
            throw new InvalidOperationException("FromPlane of " + name + " not set!");
        }

        ToPlane.AddGateway(this);
        FromPlane.AddGateway(this);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
