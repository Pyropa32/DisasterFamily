using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Prototypal
{
    public class SimpleFloorPlane : ThrowawayPrototypeCode, IEquatable<SimpleFloorPlane>
    {// Start is called before the first frame update
        // X and Y vector stored in a matrix.
        public Vector2 BottomLeft => transform.position;
        // only affected by X
        public Vector2 BottomRight => new Vector2(transform.position.x + (uvX.x * extents.x), transform.position.y + uvX.y);
        // only affected by Y
        public Vector2 TopLeft => new Vector2(transform.position.x + (uvY.x * extents.y), transform.position.y + (uvY.y * extents.y));
        public Vector2 TopRight => new Vector2(transform.position.x + (uvX.x * extents.x) + (uvY.x * extents.y),
                                            transform.position.y + (uvY.y * extents.y) + uvX.y /* not broken (yet)*/);

        [SerializeField]
        private Vector2 uvX;
        [SerializeField]
        private Vector2 uvY;
        [SerializeField]
        private Vector2Int extents;
        private HashSet<SimpleFloorPlaneGateway> myGateways = new HashSet<SimpleFloorPlaneGateway>();
        private Vector2 offset;
        private Matrix4x4 orthographicBasis;
        private Matrix4x4 orthographicBasisInverse;
        // Not currently used
        private BitGrid itemGrid;
        public void AddGateway(SimpleFloorPlaneGateway toAdd)
        {
            myGateways.Add(toAdd);
        }

        public void AddGatewayRange(IEnumerable<SimpleFloorPlaneGateway> toAddRange)
        {
            foreach (var gate in toAddRange)
            {
                AddGateway(gate);
            }
        }

        public SimpleFloorPlaneGateway[] GetGateways()
        {
            return myGateways.ToArray();
        }

        public bool TryGetAdjoiningGate(SimpleFloorPlane adjoining, out SimpleFloorPlaneGateway gateway)
        {
            gateway = null;
            // test all gates of mine, and see if it works...
            foreach (var myGate in myGateways)
            {
                if (myGate.ToPlane == adjoining ||
                    myGate.FromPlane == adjoining)
                {
                    gateway = myGate;
                    return true;
                }
            }
            return false;
        }

        void Awake()
        {
            // uvX's Y coordinate doesn't hold any weight.
            // uvY's X coordinate only holds 1x weight regardless of extents.
            orthographicBasis = new Matrix4x4(
                new(uvX.x * extents.x, uvX.y * extents.x, 0f, 0f),
                new(uvY.x * extents.y, uvY.y * extents.y, 0f, 0f),
                new(0f, 0f, 1f, 0f),
                new(0f, 0f, 0f, 1f)
            );

            orthographicBasisInverse = orthographicBasis.inverse;
            offset = transform.position;

            // set up pathfinding grid.
            itemGrid = new BitGrid(extents.x, extents.y);
        }

        public Vector2 ClampLocal(Vector2 localCoordinates)
        {
            return new Vector2(
                Mathf.Clamp(localCoordinates.x, 0f, 1f),
                Mathf.Clamp(localCoordinates.y, 0f, 1f)
            );
        }

        public bool IsPointInRange(Vector2 globalCoordinates)
        {
            var clamped = ClampGlobal(globalCoordinates);
            return ApproximatelyEqualsDelta(clamped.x, globalCoordinates.x, 0.01f) &&
                ApproximatelyEqualsDelta(clamped.y, globalCoordinates.y, 0.01f);
        }

        private bool ApproximatelyEqualsDelta(float a, float b, float delta)
        {
            return Mathf.Abs(a - b) < delta;
        }

        public Vector2 ClampGlobal(Vector2 globalCoordinates)
        {
            return PlaneToScreen(ScreenToPlane(globalCoordinates));
        }

        public Vector2 ScreenToPlane(Vector2 screenCoordinates)
        {
            var result = orthographicBasisInverse * (screenCoordinates - offset);
            return ClampLocal(result);
        }

        public Vector2 PlaneToScreen(Vector2 worldCoordinates)
        {
            var result = orthographicBasis * ClampLocal(worldCoordinates);
            return (Vector2)result + offset;
        }

        public Vector2Int PlaneToGrid(Vector2 localCoordinates)
        {
            return new Vector2Int(
                Mathf.RoundToInt(localCoordinates.x * extents.x),
                Mathf.RoundToInt(localCoordinates.y * extents.y)
            );
        }
        public Vector2 GridToPlane(Vector2Int gridCoordinates)
        {
            return new Vector2(
                gridCoordinates.x / (float)extents.x,
                gridCoordinates.y / (float)extents.y
            );
        }
        void OnDrawGizmos()
        {
            // draw X
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, new(transform.position.x + uvX.x, transform.position.y + uvX.y, 0));
            // draw Y
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, new(transform.position.x + uvY.x, transform.position.y + uvY.y, 0));

            // draw parallelogram
            Gizmos.color = new Color(Color.magenta.r, Color.magenta.g, 0.5f, 0.3f);
            Vector3[] points = new Vector3[4]
            {
                new Vector3(BottomLeft.x, BottomLeft.y, 0f),
                new Vector3(BottomRight.x, BottomRight.y, 0f),
                new Vector3(TopRight.x, TopRight.y, 0f),
                new Vector3(TopLeft.x, TopLeft.y, 0f)
            };
            Gizmos.DrawLineStrip(points, true);
        }

        public bool Equals(SimpleFloorPlane other)
        {
            return this == other;
        }

        public bool Equals(FloorPlane other)
        {
            return this == other;
        }
    }
}