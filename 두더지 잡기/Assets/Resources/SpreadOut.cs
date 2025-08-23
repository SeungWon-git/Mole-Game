using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadOut : MonoBehaviour
{
    private float speed_x, speed_y, speed_z;

    // Start is called before the first frame update
    void Start()
    {
        speed_x = Random.Range(-5.0f, 5.0f);
        speed_y = Random.Range(-5.0f, 5.0f);
        speed_z = Random.Range(-5.0f, 5.0f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(speed_x * Time.deltaTime, speed_y * Time.deltaTime, speed_z * Time.deltaTime);
    }
}
