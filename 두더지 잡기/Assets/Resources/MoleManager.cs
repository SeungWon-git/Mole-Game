using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleManager : MonoBehaviour
{
    enum Mole_Pattern
    {
        OneUp,
        FlinchUp,
        FlinchDown
    }

    enum Mole_State
    {
        NotHitable,
        NotHitable_Up,
        NotHitable_Down,
        Flinch,
        Hitable
    }

    class Mole
    {
        public GameObject mole;

        public Dictionary<int, int> mole_pattern_dict;

        public Mole_Pattern mole_pattern;
        public Mole_State mole_state;

        public float wait_start_time;
        public bool button_click;
        public bool hit;
    }


    private Mole[] moles;

    private bool swing;
    private float swing_start_time;

    private float startTime;
    private float mole_speed;
    private float flinch_wait_time;
    private float hittable_wait_time;
    private bool[] speed_up_lv;

    private bool in_game;


    public void Reset()
    {
        Cursor.visible = false;

        swing = false;
        swing_start_time = 0.0f;

        mole_speed = 10.0f;

        flinch_wait_time = 0.5f;
        hittable_wait_time = 1.0f;

        speed_up_lv = new bool[2];
        speed_up_lv[0] = false;
        speed_up_lv[1] = false;

        moles = new Mole[8];

        for (int i = 0; i < 8; i++)
        {
            moles[i] = new Mole();

            moles[i].mole = GameObject.Find("두더지 (" + (i + 1) + ")");

            moles[i].mole_pattern_dict = gameObject.GetComponent<RandomPattern>().ReturnPatternDict(i);

            moles[i].mole_state = Mole_State.NotHitable;

            moles[i].button_click = false;
        }

        //print("<<게임 시작>>");
        StartCoroutine("Timer");
    } 

    void Start()
    {
        in_game = false;  
    }

    public void In_Game(bool yn)
    {
        in_game = yn;
    }

    void Update()
    {
        if (in_game)
        {
            SwingCheck();
            Action();
        }
    }

    IEnumerator Timer()
    {
        startTime = Time.time;

        while (true)
        {
            float deltaTime = Time.time - startTime;

            if (deltaTime >= 60.0f && deltaTime < 100.0f)
            {
                SpeedUp(1);
            }
            else if (deltaTime >= 100.0f && deltaTime < 123.0f)
            {
                SpeedUp(2);
            }
            else if (deltaTime >= 123.0f)
            {
                Cursor.visible = true;

                GameObject.Find("Canvas").transform.Find("GameScene").transform.Find("ShowResultButton").gameObject.SetActive(true);

                GameObject.Find("GameObject").transform.Find("망치").GetComponent<SwingHammer>().enabled = false;

                GameObject.Find("GameObject").transform.Find("망치").GetComponent<FollowCursor>().enabled = false;

                GameObject.Find("GameManager").GetComponent<DataBase>().SetStart(false);

                GameObject.Find("PlayerManager").GetComponent<PlayerManager>().UpdateDBRanks();

                break;
            }


            MolePatternCheck(deltaTime);
            MoleStateCheck();


            yield return null;
        }

        in_game = false;
    }

    void MolePatternCheck(float time)
    {
        for (int i = 0; i < 8; i++)
        {
            foreach (int time_line in moles[i].mole_pattern_dict.Keys)
            {
                if (time >= time_line)
                {
                    moles[i].mole_pattern_dict.TryGetValue(time_line, out int value);
                    moles[i].mole_pattern = (Mole_Pattern)value;

                    moles[i].mole_state = Mole_State.NotHitable_Up;

                    moles[i].mole_pattern_dict.Remove(time_line);
                    break;
                }
            }
        }
    }

    void SpeedUp(int level)
    {
        switch (level)
        {
            case 1:
                if (speed_up_lv[0] == false)
                {
                    speed_up_lv[0] = true;
                    //print("[스피드업 - LV1]");
                }

                mole_speed = 15;

                flinch_wait_time = 0.4f;
                hittable_wait_time = 0.8f;
                break;

            case 2:
                if (speed_up_lv[1] == false)
                {
                    speed_up_lv[1] = true;
                    //print("[스피드업 - LV2]");
                }

                mole_speed = 30;

                flinch_wait_time = 0.2f;
                hittable_wait_time = 0.5f;
                break;
        }
    }

    void MoleStateCheck()
    {
        float delta_time;
        float game_deltaTime = Time.time - startTime;

        for (int i = 0; i < 8; i++)
        {
            switch (moles[i].mole_state)
            {
                case Mole_State.NotHitable:
                    break;

                case Mole_State.NotHitable_Up:
                    moles[i].mole.transform.Translate(0.0f, mole_speed * Time.deltaTime, 0.0f);

                    if (moles[i].mole_pattern == Mole_Pattern.OneUp && moles[i].mole.transform.position.y >= -2.5f)
                    {
                        moles[i].mole_state = Mole_State.Hitable;
                        moles[i].wait_start_time = Time.time;
                    }
                    else if (moles[i].mole.transform.position.y >= -2.5f)
                    {
                        moles[i].mole_state = Mole_State.Flinch;
                        moles[i].wait_start_time = Time.time;
                    }

                    break;

                case Mole_State.NotHitable_Down:
                    if (moles[i].hit)
                    {
                        moles[i].mole.transform.Translate(0.0f, (-1) * (mole_speed / 2) * Time.deltaTime, 0.0f);
                    }
                    else
                    {
                        moles[i].mole.transform.Translate(0.0f, (-1) * mole_speed * Time.deltaTime, 0.0f);
                    }

                    if (moles[i].mole.transform.position.y <= -5.0f)
                    {
                        if (moles[i].hit == false && moles[i].mole_pattern != Mole_Pattern.FlinchDown)
                        {
                            //print((i + 1) + "번째 두더지 미스.." + "  * 남은시간 " + (120.0f - game_deltaTime).ToString() + "초");

                            gameObject.GetComponent<Score>().MissedCountUp();
                        }

                        moles[i].mole_state = Mole_State.NotHitable;
                    }

                    break;

                case Mole_State.Flinch:
                    delta_time = Time.time - moles[i].wait_start_time;

                    if (delta_time >= flinch_wait_time)
                    {
                        if (moles[i].mole_pattern == Mole_Pattern.FlinchUp)
                        {
                            moles[i].mole_state = Mole_State.Hitable;
                            moles[i].wait_start_time = Time.time;
                        }
                        else
                        {
                            moles[i].mole_state = Mole_State.NotHitable_Down;
                        }
                    }

                    break;

                case Mole_State.Hitable:
                    delta_time = Time.time - moles[i].wait_start_time;

                    if (moles[i].mole.transform.position.y < -1.0f)
                    {
                        moles[i].mole.transform.Translate(0.0f, mole_speed * Time.deltaTime, 0.0f);
                    }
                    else if (moles[i].mole.transform.position.y >= -1.0f && delta_time >= hittable_wait_time)
                    {
                        moles[i].mole_state = Mole_State.NotHitable_Down;
                    }

                    break;
            }
        }
    }

    void SwingCheck()
    {
        bool swing_disable = GameObject.Find("망치").GetComponent<SwingHammer>().ReturnSwing();

        if (Input.GetMouseButtonDown(0) && !swing_disable)
        {
            swing = true;

            if (swing_start_time == 0.0f)
            {
                swing_start_time = Time.time;
            }
        }
    }

    bool SwingHitCheck()
    {
        bool result = false;
        float deltaTime = Time.time - startTime;

        for (int i = 0; i < 8; i++)
        {
            if (moles[i].button_click && swing)
            {
                if (moles[i].mole_state == Mole_State.Hitable)
                {
                    //print((i + 1) + "번째 두더지 히트!" + "  * 남은시간 " + (120.0f - deltaTime).ToString() + "초");

                    Vector3 vec_tmp = new Vector3(moles[i].mole.transform.position.x, 2.5f, moles[i].mole.transform.position.z);
                    gameObject.GetComponent<CreateParticle>().CreateParticles(vec_tmp);

                    gameObject.GetComponent<Score>().HitCountUp();

                    moles[i].hit = true;
                    moles[i].mole_state = Mole_State.NotHitable_Down;
                    swing = false;
                    swing_start_time = 0.0f;
                }

                moles[i].button_click = false;
            }
        }

        float delta_time = Time.time - swing_start_time;

        if (swing == false)
        {
            result = true;
        }
        else if (swing == true && result == false && delta_time <= 0.25f)
        {
            result = true;
        }
        else if (swing == true && result == false && delta_time > 0.25f)
        {
            swing = false;
            swing_start_time = 0.0f;
        }

        return result;
    }

    void Action()
    {
        float deltaTime = Time.time - startTime;

        if (SwingHitCheck())
        {
            for (int i = 0; i < 8; i++)
            {
                if (moles[i].hit)
                {
                    SquishMole(i);
                }
            }
        }
        else
        {
            //print("미스.." + "  * 남은시간 " + (120.0f - deltaTime).ToString() + "초");

            gameObject.GetComponent<Score>().HitMissCountUp();

            GameObject.Find("망치").GetComponent<SwingHammer>().DisableSwing();
        }
    }

    void SquishMole(int num)
    {
        if (moles[num].mole.transform.position.y > -5.0f)
        {
            moles[num].mole.transform.Find("Burrow").GetComponent<SkinnedMeshRenderer>().material.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
            moles[num].mole.transform.localScale += new Vector3(5 * Time.deltaTime, (-5) * Time.deltaTime, 5 * Time.deltaTime);
        }
        else
        {
            moles[num].mole.transform.Find("Burrow").GetComponent<SkinnedMeshRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            moles[num].mole.transform.localScale = new Vector3(3.0f, 3.0f, 3.0f);

            moles[num].hit = false;
        }
    }

    public void ButtonClick(int num)
    {
        moles[num].button_click = true;
    }

    void PrintMolePattern()
    {
        for (int i = 0; i < 8; i++)
        {
            print("[" + (i + 1) + "번째 두더지]");
            print("총 나오는 횟수: " + moles[i].mole_pattern_dict.Count + "번");

            foreach (var t_pattern in moles[i].mole_pattern_dict)
            {
                print(t_pattern.Key + " : " + (Mole_Pattern)t_pattern.Value);
            }

            print("-------------------------------------------------");
        }
    }
}
