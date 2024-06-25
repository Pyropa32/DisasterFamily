using System.Collections.Generic;
using System;

public record ExternalPathfindingResult
{
    public bool Success;
    public Tuple<FloorPlane, FloorPlaneGateway, FloorPlane>[] Solution;
};