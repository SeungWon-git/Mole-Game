using System;
using System.Collections.Generic;
using System.Net; 

using UnityEngine;
using UnityEngine.UI;

using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;

public class DataBase : MonoBehaviour
{
    class Player
    {
        public string name;
        public string PW;
        public int Score;
        public bool isLogin;
        public bool isStart;

        public void UpdateScore(int mmr)
        {
            Score += mmr;

            if (Score < 0)
            {
                Score = 0;
            }
        }
    }

    
    private const string BasePath = "https://mole-game-30671-default-rtdb.firebaseio.com/";
    private const string FirebaseSecret = "ElBJqRCZs4bQMKlxiea9ZqrfvQWkRGRuvtw9TuNS";
    private static FirebaseClient _client;


    Player myInfo;

    public Text ID;
    public InputField PassWord;

    public Text Check_Result;

    public int player_cnt;


    void Start()
    {
        player_cnt = 0;

        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = FirebaseSecret,
            BasePath = BasePath
        };

        _client = new FirebaseClient(config);
    }

    void OnDestroy()
    {
        if (myInfo != null)
        {
            string copy_name = myInfo.name;

            myInfo.isLogin = false;
            myInfo.name = null;

            if (myInfo.isStart)
            {
                myInfo.isStart = false;

                if (player_cnt == 1)
                {
                    myInfo.UpdateScore(0);
                }
                else
                {
                    myInfo.UpdateScore(-1);
                }

                _client.Update(copy_name, myInfo);
            }
            else
            {
                myInfo.isStart = false;

                _client.Update(copy_name, myInfo);
            }
        }
    }

    public void Login()
    {
        string input_id = ID.text;
        string input_pw = PassWord.text;

        FirebaseResponse response = _client.Get(input_id);

        Player tmp = response.ResultAs<Player>();

        if (tmp != null)
        {
            if(tmp.PW == input_pw)
            {
                if (!tmp.isLogin)
                {
                    GameObject parent = GameObject.Find("Canvas").transform.Find("Lobby").transform.Find("Login").gameObject;

                    parent.transform.Find("InputField_ID").gameObject.GetComponent<InputField>().readOnly = true;
                    parent.transform.Find("InputField_ID").gameObject.GetComponent<InputField>().interactable = false;

                    parent.transform.Find("InputField_PW").gameObject.GetComponent<InputField>().readOnly = true;
                    parent.transform.Find("InputField_PW").gameObject.GetComponent<InputField>().interactable = false;

                    parent.transform.Find("JoinMembershipButton").gameObject.GetComponent<Button>().interactable = false;
                    parent.transform.Find("LoginButton").gameObject.GetComponent<Button>().interactable = false;

                    GameObject.Find("LobbyManager").GetComponent<LobbyManager>().InputNickName(ID);

                    Check_Result.text = "로그인 성공!";
                    Check_Result.color = new Color(0, 1, 0, 1);

                    parent.transform.Find("JoinButton").gameObject.SetActive(true);

                    myInfo = tmp;
                    myInfo.isLogin = true;
                    _client.Update(input_id, myInfo);

                    myInfo.name = input_id;
                    myInfo.isStart = false;
                }
                else
                {
                    Check_Result.text = "이미 사용중인 계정입니다.";
                    Check_Result.color = new Color(1, 0, 0, 1);
                }
            }
            else
            {
                Check_Result.text = "비밀번호가 일치하지 않습니다.";
                Check_Result.color = new Color(1, 0, 0, 1);
            }
        }
        else
        {
            Check_Result.text = "존재하지 않는 아이디 입니다.";
            Check_Result.color = new Color(1, 0, 0, 1);
        }
        
    }

    public void Resister()
    {
        string input_id = ID.text;
        string input_pw = PassWord.text;

        FirebaseResponse response = _client.Get(input_id);

        Player tmp = response.ResultAs<Player>();

        if (tmp == null)
        {
            if (input_pw == "")
            {
                Check_Result.text = "가입실패!\n비밀번호를 최소 한자리 이상 설정해주세요.";
                Check_Result.color = new Color(1, 0, 0, 1);
            }
            else
            {
                Player new_player = new Player
                {
                    PW = PassWord.text,
                    Score = 1,
                    isLogin = false
                };

                _client.Update(ID.text, new_player);

                PassWord.text = "";

                Check_Result.text = "가입 완료!\n가입정보에 맞게 로그인을 해주세요.";
                Check_Result.color = new Color(0, 1, 0, 1);
            }
        }
        else
        {
            Check_Result.text = "이미 아이디가 존재합니다.\n다른 아이디를 입력해주세요.";
            Check_Result.color = new Color(1, 0, 0, 1);
        }

    }

    public int SendRanks(string name)
    {
        FirebaseResponse response = _client.Get(name);

        Player tmp = response.ResultAs<Player>();

        if (tmp != null)
        {
            return tmp.Score;
        }

        return 0;
    }

    public void SetStart(bool set_)
    {
        if (myInfo != null)
        {
            myInfo.isStart = set_;

            string tmp = myInfo.name;

            myInfo.name = null;

            _client.Update(tmp, myInfo);

            myInfo.name = tmp;
        }
    }

    public void UpdateRanks(int mmr)
    {
        myInfo.UpdateScore(mmr);

        string tmp = myInfo.name;

        myInfo.name = null;

        _client.Update(tmp, myInfo);

        myInfo.name = tmp;
    }
}