using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextEffect : MonoBehaviour
{
    TextMeshProUGUI text;
    float alpha;

    void Start()
    {
        text = gameObject.GetComponent<TextMeshProUGUI>();

        alpha = 1.0f;

        StartCoroutine("FadingAway");
    }

    void Update()
    {
        if (alpha > 0)
            alpha -= 0.007f;
        else
            alpha = 0;

        text.color =
            new Color(text.color.r, text.color.g, text.color.b, alpha);

        text.transform.Translate(0.0f, 20 * Time.deltaTime, 0.0f);
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
