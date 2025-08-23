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


        sb_text.text += "{총 나온 두더지 수}: " + result[0].ToString() + "마리" + "\n\n";

        sb_text.text += "한번에 나온 두더지 수: " + result[1].ToString() + "마리" + "\n";
        sb_text.text += "플린치하고 나온 두더지 수: " + result[2].ToString() + "마리" + "\n";
        sb_text.text += "플린치하고 나오지 않은 두더지 수: " + result[3].ToString() + "마리" + "\n\n";

        sb_text.text += "맞춘 두더지 수: " + result[4].ToString() + "마리" + "\n";
        sb_text.text += "헛스윙 횟수: " + result[5].ToString() + "번" + "\n";
        sb_text.text += "놓친 두더지 수: " + result[6].ToString() + "마리" + "\n\n";

        sb_text.text += "[최종점수]: " + result[7].ToString() + "점";
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
