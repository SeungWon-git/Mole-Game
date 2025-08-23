using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateUpDown : MonoBehaviour
{
    int r_speed, t_speed;

    void Start()
    {
        r_speed = Random.Range(50, 100);
        t_speed = Random.Range(1, 3);

        transform.Rotate(0.0f, Random.Range(0.0f, 360.0f), 0.0f);
        transform.Translate(0.0f, Random.Range(0.0f, 4.3f), 0.0f);
    }

    void Update()
    {
        transform.Rotate(0.0f, r_speed * Time.deltaTime, 0.0f);

        float loc_y = transform.position.y + 5.0f;

        if (loc_y >= 4.3f)
        {
            transform.position = new Vector3(transform.position.x, -0.7f, transform.position.z);
            t_speed *= (-1);
        }
        else if (loc_y <= 0.0f)
        {
            transform.position = new Vector3(transform.position.x, -5.0f, transform.position.z);
            t_speed *= (-1);
        }

        transform.Translate(0.0f, t_speed * Time.deltaTime, 0.0f);
    }
}
