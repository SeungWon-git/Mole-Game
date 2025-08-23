using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    private TextMeshProUGUI timer_txt;
    private float countdown;
    private bool pause;

    public void ResetTimer()
    {
        timer_txt = gameObject.GetComponent<TextMeshProUGUI>();
        countdown = 120.0f;
        pause = false;
    }

    void Start()
    {
        ResetTimer();
    }

    void Update()
    {
        int display_countdown = ((int)System.Math.Floor(countdown));

        if (!pause)
        {
            if (display_countdown <= 0)
            {
                display_countdown = 0;

                pause = true;
            }
            else
            {
                countdown -= Time.deltaTime;

                if (countdown > 60)
                {
                    timer_txt.color = new Color(1, 1, 1, 1);
                }
                else if (countdown <= 60 && countdown > 20)
                {
                    timer_txt.color = new Color(1, 1, 0, 1);
                }
                else if (countdown <= 20)
                {
                    timer_txt.color = new Color(1, 0, 0, 1);
                }
            }

            int min = display_countdown / 60;
            int sec = display_countdown % 60;

            string display_txt;

            if (sec >= 10)
                display_txt = min.ToString() + " : " + sec.ToString();
            else
                display_txt = min.ToString() + " : 0" + sec.ToString();

            timer_txt.text = display_txt;
        }
    }
}
