using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpin : MonoBehaviour
{
    private float speed_x, speed_y, speed_z;

    // Start is called before the first frame update
    void Start()
    {
        speed_x = Random.Range(-80.0f, 80.0f);
        speed_y = Random.Range(-80.0f, 80.0f);
        speed_z = Random.Range(-80.0f, 80.0f);

        //print("Start Coroutine (RandomSpinning)");
        StartCoroutine("RandomSpinning");
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(speed_x * Time.deltaTime, speed_y * Time.deltaTime, speed_z * Time.deltaTime);
    }

    IEnumerator RandomSpinning()
    {
        while (true)
        {
            speed_x = Random.Range(-80.0f, 80.0f);
            speed_y = Random.Range(-80.0f, 80.0f);
            speed_z = Random.Range(-80.0f, 80.0f);

            yield return new WaitForSeconds(2.0f);
        }
    }
}
