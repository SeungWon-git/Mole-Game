using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateParticle : MonoBehaviour
{
    private GameObject particle;

    // Start is called before the first frame update
    void Start()
    {
        particle = Resources.Load("ParticleCube") as GameObject;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CreateParticles(Vector3 vec)
    {
        for (int i = 0; i < 10; i++) 
        {
            GameObject one_particle = Instantiate(particle);

            Transform tmp = GameObject.Find("GameObject").transform;

            one_particle.transform.SetParent(tmp);

            one_particle.transform.position = vec;

            one_particle.GetComponent<Renderer>().material.color = Random.ColorHSV();
        }
    }
}
