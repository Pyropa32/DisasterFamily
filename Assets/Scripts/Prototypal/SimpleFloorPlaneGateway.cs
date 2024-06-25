using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Prototypal
{
    public class SimpleFloorPlaneGateway : ThrowawayPrototypeCode
    {
        // Start is called before the first frame update
        [SerializeField]
        SimpleFloorPlane overrideTo;
        [SerializeField]
        SimpleFloorPlane overrideFrom;
        SimpleFloorPlaneGraph world;

        public void SetFromToWithOverrides()
        {
            if (!HasOverrideFromTo())
            {
                Debug.LogError("Could not detect To/From planes, and no override planes present in " + name + "!");
            }
            FromPlane = overrideFrom;
            ToPlane = overrideTo;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasOverrideFromTo()
        {
            return overrideFrom != null && overrideTo != null;
        }

        public void SetWorld(SimpleFloorPlaneGraph to)
        {
            if (world != null)
            {
                //throw new InvalidOperationException("This gateway already has a Plane Graph associated with it.");
            }
            world = to;
        }

        public SimpleFloorPlane FromPlane { get; set; }
        public SimpleFloorPlane ToPlane { get; set; }
        public uint ID { get; protected set; }

        static uint _gatewayCurrentID = 1;

        public int Distance2To(SimpleFloorPlaneGateway what)
        {
            return (int)((transform.position.x - what.transform.position.x) * (transform.position.x - what.transform.position.x) +
                    (transform.position.y - what.transform.position.y) * (transform.position.y - what.transform.position.y));
        }

        public bool SharesPlaneWith(SimpleFloorPlaneGateway with)
        {
            return TryGetSharedPlane(with, out _);
        }

        public bool TryGetSharedPlane(SimpleFloorPlaneGateway with, out SimpleFloorPlane result)
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
            // set world
            var world = GetComponentInParent<SimpleFloorPlaneGraph>();
            //SetWorld(world);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}