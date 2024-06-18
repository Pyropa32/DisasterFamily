using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrthographicPlaneGateway : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    OrthographicPlane from;
    [SerializeField]
    OrthographicPlane to;
    private bool worldHasBeenSet = false;
    OrthographicPlaneGraph world;

    public uint ID { get; protected set; }

    static uint _gatewayCurrentID = 0;

    public void SetWorld(OrthographicPlaneGraph to)
    {
        if (worldHasBeenSet)
        {
            throw new InvalidOperationException("This gateway already has a Plane Graph associated with it.");
        }
        world = to;
        worldHasBeenSet = true;
    }

    public OrthographicPlane FromPlane => from;
    public OrthographicPlane ToPlane => to;

    public int Distance2To(OrthographicPlaneGateway what)
    {
        return (int)((transform.position.x - what.transform.position.x) * (transform.position.x - what.transform.position.x) +
                (transform.position.y - what.transform.position.y) * (transform.position.y - what.transform.position.y));
    }

    public bool TryGetSharedPlane(OrthographicPlaneGateway with, out OrthographicPlane result)
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
