using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingHammer : MonoBehaviour
{
    private GameObject hammer;
    private float swing_angle;
    private bool swing;
    private bool swing_disable;
    private float swing_disable_start_time;

    void Start()
    {
        hammer = gameObject;
        swing_angle = 120.0f;
        swing = false;
        swing_disable = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !swing_disable)
        {
            swing = true;
        }

        if (swing_disable)
        {
            float delta_time = Time.time - swing_disable_start_time;

            if (delta_time > 2.0f)
            {
                swing_disable = false;

                float x = 150.0f / 255.0f;

                hammer.GetComponent<MeshRenderer>().material.color = new Color(x, x, x, 1.0f);
            }
        }

        if (swing && swing_angle <= 220.0f)
        {
            swing_angle += 4.0f;
            hammer.transform.Rotate(0f, 0f, 4.0f);
        }
        else if (swing && swing_angle > 200.0f)
        {
            swing = false;
        }
        else if (swing_angle >= 120.0f)
        {
            swing_angle -= 2.0f;
            hammer.transform.Rotate(0f, 0f, -2.0f);
        }
    }

    public void DisableSwing()
    {
        swing_disable = true;
        swing_disable_start_time = Time.time;

        hammer.GetComponent<MeshRenderer>().material.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
    }

    public bool ReturnSwing()
    {
        return swing_disable;
    }
}
