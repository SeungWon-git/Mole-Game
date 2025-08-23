using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreTextChange : MonoBehaviour
{
    Score score_obj;

    TextMeshProUGUI score_txt;

    int score;

    bool minus_score;

    void Start()
    {
        score_obj = GameObject.Find("GameManager").GetComponent<Score>();

        score_txt = gameObject.GetComponent<TextMeshProUGUI>();

        score = 0;

        minus_score = false;
    }
    
    void Update()
    {
        score = score_obj.SendScore();

        if (minus_score)
        {
            score_txt.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        }
        else
        {
            score_txt.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }

        score_txt.text = "Score: " + score.ToString();
    }

    public void MinusScore(bool yn)
    {
        minus_score = yn;
    }
}
