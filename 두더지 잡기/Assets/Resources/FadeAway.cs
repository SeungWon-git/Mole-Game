using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeAway : MonoBehaviour
{
    private Renderer particle_cube_ren;
    private float alpha;

    // Start is called before the first frame update
    void Start()
    {
        particle_cube_ren = gameObject.GetComponent<Renderer>();

        alpha = 1.0f;

        StartCoroutine("FadingAway");
    }

    // Update is called once per frame
    void Update()
    {
        if (alpha > 0)
            alpha -= 0.007f;
        else
            alpha = 0;

        particle_cube_ren.material.color =
            new Color(particle_cube_ren.material.color.r, particle_cube_ren.material.color.g, particle_cube_ren.material.color.b, alpha);
    }

    IEnumerator FadingAway()
    {
        while (alpha > 0)
        {
            yield return null;
        }

        Destroy(gameObject);
    }
}
