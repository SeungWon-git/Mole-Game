using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCursor : MonoBehaviour
{
    private GameObject hammer;
    private float adjust_z;

    void Start()
    {
        hammer = gameObject;
    }

    void Update()
    {
        Vector3 newPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));

        adjust_z = 20 + newPosition.z;

        hammer.transform.position = new Vector3(newPosition.x, hammer.transform.position.y, newPosition.z + adjust_z);
    }
}
