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

    int score;          //����
    //int prev_score;     //���������� ����(������ ���� Ȯ���� ���� ���)

    int hit_cnt;        //���� �δ��� ��
    int hitmiss_cnt;    //�꽺�� Ƚ��
    int missed_cnt;     //��ģ �δ��� ��

    int total_cnt;      //�� ���� �δ��� ��

    int oneup_cnt;      //�ѹ��� ���� �δ��� ��
    int flinchup_cnt;   //�ø�ġ�ϰ� ���� �δ��� ��
    int flinchdown_cnt; //�ø�ġ�ϰ� ������ ���� �δ��� ��

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
        //    print("��������: " + score + "��");
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
        print("<<���� ����>>");
        print("");
        print("{�� ���� �δ��� ��}: " + total_cnt + "����");
        print("�ѹ��� ���� �δ��� ��: " + oneup_cnt + "����");
        print("�ø�ġ�ϰ� ���� �δ��� ��: " + flinchup_cnt + "����");
        print("�ø�ġ�ϰ� ������ ���� �δ��� ��: " + flinchdown_cnt + "����");
        print("");
        print("���� �δ��� ��: " + hit_cnt + "����");
        print("�꽺�� Ƚ��: " + hitmiss_cnt + "��");
        print("��ģ �δ��� ��: " + missed_cnt + "����");
        print("");
        print("[��������]: " + score + "��");
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
