using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Diego
{
    public class CameraToScreenspaceConverter
    {
        public static Transform GetFromScreenSpace(Vector2 pos)
        {
            Vector3 gameSpace = new Vector3(pos.x / Screen.width - 0.5f, pos.y / Screen.height - 0.5f, 0);
            gameSpace.x *= 16;
            gameSpace.y *= 10;
            gameSpace += new Vector3(1.2f, 0.75f, 0);
            gameSpace.x *= 16 / 13.6f;
            gameSpace.y *= 10 / 8.5f;
            RaycastHit2D hit = Physics2D.GetRayIntersection(new Ray(GameObject.FindWithTag("MainCamera").transform.position + gameSpace, GameObject.FindWithTag("MainCamera").transform.forward));
            return hit.transform;
        }
        public static Vector2 GetGameSpaceFromScreenSpace(Vector2 pos)
        {
            Vector2 gameSpace = new Vector2(pos.x / Screen.width - 0.5f, pos.y / Screen.height - 0.5f);
            gameSpace.x *= 16;
            gameSpace.y *= 10;
            gameSpace += new Vector2(1.2f, 0.75f);
            gameSpace.x *= 16 / 13.6f;
            gameSpace.y *= 10 / 8.5f;
            return gameSpace;
        }
    }
}
