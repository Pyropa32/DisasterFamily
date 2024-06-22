using System.Collections.Generic;
using System;
namespace Prototypal
{
    public record PlaneTransferData
    {

        public PlaneTransferData(SimpleFloorPlane _start, SimpleFloorPlaneGateway _gate, SimpleFloorPlane _finish)
        {
            StartPlane = _start;
            Gate = _gate;
            DestinationPlane = _finish;
        }

        public PlaneTransferData()
        {
        }
        public SimpleFloorPlane StartPlane;
        public SimpleFloorPlaneGateway Gate;
        public SimpleFloorPlane DestinationPlane;

    };
}