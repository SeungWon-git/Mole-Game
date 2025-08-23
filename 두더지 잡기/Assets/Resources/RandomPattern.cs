using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPattern : MonoBehaviour
{
    enum Mole_Pattern
    {
        OneUp,
        FlinchUp,
        FlinchDown
    }

    private List<Dictionary<int, int>> mole_pattern_list;

    private int seed;

    private bool getSeed;

    void Start()
    {
        getSeed = false;
    }

    void Update()
    {
        if (getSeed)
        {
            //print("시드를 받았습니다. 시드: " + seed);

            MakePattern();

            GameObject.Find("GameManager").GetComponent<Score>().ResetScore();

            getSeed = false;
        }
    }

    public void GetSeed(int ext_seed)
    {
        seed = ext_seed;

        getSeed = true;
    }

    void MakePattern()
    {
        mole_pattern_list = new List<Dictionary<int, int>>();

        int max_mole_out;
        //System.Random rnd = new System.Random();
        System.Random rnd = new System.Random(seed);

        for (int i = 0; i < 8; i++)
        {
            Dictionary<int, int> mole_pattern_dict;
            mole_pattern_dict = new Dictionary<int, int>();

            max_mole_out = rnd.Next(10, 20);

            int time;
            List<int> time_list = new List<int>();
            int pattern;
            List<int> pattern_list = new List<int>();

            for (int j = 0; j < max_mole_out; j++)
            {
                time = rnd.Next(1, 118);
                while (!CheckTimeOk(time_list, time))
                {
                    time = rnd.Next(1, 115 + 1);
                }
                time_list.Add(time);

                pattern = rnd.Next(0, 9 + 1);
                if (pattern <= 4)
                {
                    pattern_list.Add(0);
                }
                else if (pattern <= 7)
                {
                    pattern_list.Add(1);
                }
                else
                {
                    pattern_list.Add(2);
                }
            }

            time_list.Sort();

            for (int j = 0; j < max_mole_out; j++)
            {
                mole_pattern_dict.Add(time_list[j], pattern_list[j]);
            }

            mole_pattern_list.Add(mole_pattern_dict);
        }
    }

    bool CheckTimeOk(List<int> time_list, int time)
    {
        foreach(int t in time_list)
        {
            if (time >= t && time < t + 3)
                return false;
            else if (time > t - 3 && time <= t)
                return false;
        }

        return true;
    }

    void PrintMolePattern()
    {
        for (int i = 0; i < 8; i++)
        {
            print("[" + (i + 1) + "번째 두더지]");
            print("총 나오는 횟수: " + mole_pattern_list[i].Count + "번");

            foreach (var t_pattern in mole_pattern_list[i])
            {
                print(t_pattern.Key + " : " + (Mole_Pattern)t_pattern.Value);
            }

            print("-------------------------------------------------");
        }
    }

    public Dictionary<int, int> ReturnPatternDict(int num)
    {
        try
        {
            return mole_pattern_list[num];
        }
        catch(System.Exception ex)
        {
            print(ex);
            return null;
        }
    }
}
