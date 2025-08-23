using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetResult : MonoBehaviour
{
    Text sb_text;

    int[] result;

    public void ResetScoreBoard()
    {
        result = GameObject.Find("GameManager").GetComponent<Score>().SendTotalScore();

        sb_text.text = "";


        sb_text.text += "{�� ���� �δ��� ��}: " + result[0].ToString() + "����" + "\n\n";

        sb_text.text += "�ѹ��� ���� �δ��� ��: " + result[1].ToString() + "����" + "\n";
        sb_text.text += "�ø�ġ�ϰ� ���� �δ��� ��: " + result[2].ToString() + "����" + "\n";
        sb_text.text += "�ø�ġ�ϰ� ������ ���� �δ��� ��: " + result[3].ToString() + "����" + "\n\n";

        sb_text.text += "���� �δ��� ��: " + result[4].ToString() + "����" + "\n";
        sb_text.text += "�꽺�� Ƚ��: " + result[5].ToString() + "��" + "\n";
        sb_text.text += "��ģ �δ��� ��: " + result[6].ToString() + "����" + "\n\n";

        sb_text.text += "[��������]: " + result[7].ToString() + "��";
    }

    void Awake()
    {
        sb_text = gameObject.GetComponent<Text>();

        result = new int[8];

        result = GameObject.Find("GameManager").GetComponent<Score>().SendTotalScore();

        sb_text.text = "";



        GameObject.Find("GameManager").GetComponent<Score>().In_Game(false);
    }
}
