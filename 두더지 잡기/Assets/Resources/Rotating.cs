using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotating : MonoBehaviour
{
    void Start()
    {
        gameObject.transform.Rotate(Random.Range(-90.0f, 90.0f), 0, 0);    
    }

    
    void Update()
    {
        gameObject.transform.Rotate(50 * Time.deltaTime, 0, 0);
    }
}
