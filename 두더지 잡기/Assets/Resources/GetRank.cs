using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetRank : MonoBehaviour
{
    Text rank_text;

    public void ResetRankBoard()
    {
        rank_text.text = GameObject.Find("PlayerManager").GetComponent<PlayerManager>().SendRank();
    }

    void Awake()
    {
        rank_text = gameObject.GetComponent<Text>();
    }
}
