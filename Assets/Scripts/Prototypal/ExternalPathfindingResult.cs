using System.Collections.Generic;
using System;
namespace Prototypal
{
    public record SimplePlaneTraversalResult
    {
        public bool Success;
        public PlaneTransferData[] Solution;
    };
}