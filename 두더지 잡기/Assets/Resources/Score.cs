using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    enum Mole_Pattern
    {
        OneUp,
        FlinchUp,
        FlinchDown
    }

    GameObject score_up, score_down_1, score_down_2;

    int score;          //점수
    //int prev_score;     //이전프레임 점수(점수의 변동 확인을 위해 사용)

    int hit_cnt;        //맞춘 두더지 수
    int hitmiss_cnt;    //헛스윙 횟수
    int missed_cnt;     //놓친 두더지 수

    int total_cnt;      //총 나온 두더지 수

    int oneup_cnt;      //한번에 나온 두더지 수
    int flinchup_cnt;   //플린치하고 나온 두더지 수
    int flinchdown_cnt; //플린치하고 나오지 않은 두더지 수

    bool in_game;

    public void ResetScore()
    {
        score = 0;
        //prev_score = 0;

        hit_cnt = 0;
        hitmiss_cnt = 0;
        missed_cnt = 0;

        oneup_cnt = 0;
        flinchup_cnt = 0;
        flinchdown_cnt = 0;

        Dictionary<int, int> temp;

        for (int i = 0; i < 8; i++)
        {
            temp = gameObject.GetComponent<RandomPattern>().ReturnPatternDict(i);

            foreach (int val in temp.Values)
            {
                switch (val)
                {
                    case 0:
                        oneup_cnt++;
                        break;

                    case 1:
                        flinchup_cnt++;
                        break;

                    case 2:
                        flinchdown_cnt++;
                        break;
                }
            }
        }

        total_cnt = oneup_cnt + flinchup_cnt + flinchdown_cnt;
    }

    void Start()
    {
        in_game = false;

        score_up = Resources.Load("ScoreUpText") as GameObject;
        score_down_1 = Resources.Load("ScoreDownText (1)") as GameObject;
        score_down_2 = Resources.Load("ScoreDownText (2)") as GameObject;
    }

    public void In_Game(bool yn)
    {
        in_game = yn;
    }

    void Update()
    {
        if (in_game)
        {
            score = hit_cnt * 3 + hitmiss_cnt * (-2) + missed_cnt * (-1);

            if (score < 0)
            {
                score = 0;

                GameObject.Find("Canvas").transform.Find("GameScene").transform.Find("ScoreText").GetComponent<ScoreTextChange>().MinusScore(true);
            }
            else
            {
                GameObject.Find("Canvas").transform.Find("GameScene").transform.Find("ScoreText").GetComponent<ScoreTextChange>().MinusScore(false);
            }
        }

        //if(prev_score != score)
        //{
        //    print("현재점수: " + score + "점");
        //}
        
        //prev_score = score;
    }

    public void HitCountUp()
    {
        hit_cnt++;

        TextEffect(score_up);
    }

    public void HitMissCountUp()
    {
        hitmiss_cnt++;

        TextEffect(score_down_2);
    }

    public void MissedCountUp()
    {
        missed_cnt++;

        TextEffect(score_down_1);
    }

    void TextEffect(GameObject g_obj)
    {
        GameObject text_effect = Instantiate(g_obj);

        Transform tmp = GameObject.Find("Canvas").transform.Find("GameScene").transform;

        text_effect.transform.SetParent(tmp);

        text_effect.transform.localPosition = new Vector3(80.0f, -280.0f, 0.0f);
    }

    public void PrintTotalScore()
    { 
        print("<<최종 점수>>");
        print("");
        print("{총 나온 두더지 수}: " + total_cnt + "마리");
        print("한번에 나온 두더지 수: " + oneup_cnt + "마리");
        print("플린치하고 나온 두더지 수: " + flinchup_cnt + "마리");
        print("플린치하고 나오지 않은 두더지 수: " + flinchdown_cnt + "마리");
        print("");
        print("맞춘 두더지 수: " + hit_cnt + "마리");
        print("헛스윙 횟수: " + hitmiss_cnt + "번");
        print("놓친 두더지 수: " + missed_cnt + "마리");
        print("");
        print("[최종점수]: " + score + "점");
    }

    public int SendScore()
    {
        return score;
    }

    public int[] SendTotalScore()
    {
        int[] stats = new int[8];

        stats[0] = total_cnt;
        stats[1] = oneup_cnt;
        stats[2] = flinchup_cnt;
        stats[3] = flinchdown_cnt;
        stats[4] = hit_cnt;
        stats[5] = hitmiss_cnt;
        stats[6] = missed_cnt;
        stats[7] = score;

        return stats;
    }
}
