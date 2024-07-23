using System.Collections;
using System.Collections.Generic;
using Diego;
using UnityEngine;

public class DirectionToBoxArrow : MonoBehaviour
{
    public GameObject player;
    public GameObject box;

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = box.transform.position - player.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
    }
}
