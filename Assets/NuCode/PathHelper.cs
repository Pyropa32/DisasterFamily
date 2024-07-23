using System;
using System.Collections.Generic;
using UnityEngine;
public static class PathHelper
{
    /// <summary>
    /// Combines a list of paths into a bigger array.
    /// </summary>
    /// <param name="paths">however many paths (Vector2[]) you need.</param>
    /// <returns></returns>
    public static Vector2[] CombinePaths(params Vector2[][] paths)
    {
        var pathList = new List<Vector2>();
        var previous = new Vector2(float.MaxValue, float.MaxValue);
        for (int i = 0; i < paths.Length; i++)
        {
            var path = paths[i];
            for (int j = 0; j < path.Length; j++)
            {
                var point = path[j];
                if (AreVectorsApproximatelyEqual(point, previous))
                {
                    // don't add the points.
                    continue;   
                }
                else
                {
                    pathList.Add(point);
                }
                previous = point;
            }
        }
        return pathList.ToArray();
    }
    public static bool AreVectorsApproximatelyEqual(Vector2 a, Vector2 b, float epsilon=float.Epsilon)
    {
        return Mathf.Abs(b.x - a.x) < epsilon && 
               Mathf.Abs(b.y - a.y) < epsilon;
    }
}

