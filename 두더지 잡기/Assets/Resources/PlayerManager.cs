using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;

public class PlayerManager : MonoBehaviourPun
{
    class MolePlayer
    {
        public string name;

        public int myActorNum;

        public int score;

        public int rank;

        public bool loadup_complete;

        public int DB_Score;
    }

    MolePlayer[] players;

    Score score_obj;

    Text scoreboard;

    bool gameStart;

    bool startbuttonActive;

    int localActorNum;

    void Start()
    {
        players = new MolePlayer[4];

        scoreboard = GameObject.Find("Canvas").transform.Find("GameScene").transform.Find("Scoreboard").GetComponent<Text>();

        score_obj = GameObject.Find("GameManager").GetComponent<Score>();

        startbuttonActive = false;
    }

    void Update()
    {
        if (!startbuttonActive && PhotonNetwork.IsMasterClient)
        {
            GameObject.Find("Canvas").transform.Find("Lobby").transform.Find("LobbyRoom").transform.Find("StartButton").gameObject.SetActive(true);
            startbuttonActive = true;
        }
        else if (gameStart)
        {
            photonView.RPC("SendReceiveScore", RpcTarget.All, localActorNum, score_obj.SendScore());

            UpdateScoreboard();
        }
    }

    void UpdateScoreboard()
    {
        UpdateRank();

        List<MolePlayer> rank_board= new List<MolePlayer>();

        rank_board = players.OrderBy(x => x.rank).ToList<MolePlayer>();

        scoreboard.text = "";

        for (int num = 0; num < 4; num++)
        {
            if (rank_board[num].rank != 0)
                scoreboard.text += rank_board[num].rank.ToString() + ". " + rank_board[num].name + "\n점수: " + rank_board[num].score.ToString() + "\n\n";
        }

    }

    public string SendRank()
    {
        string rank_str = "";

        List<MolePlayer> rank_board = new List<MolePlayer>();

        rank_board = players.OrderBy(x => x.rank).ToList<MolePlayer>();

        for (int num = 0; num < 4; num++)
        {
            if (rank_board[num].rank != 0)
                rank_str += rank_board[num].rank.ToString() + "등: " + rank_board[num].name + " - " + rank_board[num].score.ToString() + "점, 월드 랭킹 점수: " + rank_board[num].DB_Score.ToString() + "점\n\n\n";
        }

        return rank_str;
    }

    void UpdateRank()
    {
        int rank;

        for (int num = 0; num < 4; num++)
        {
            if (players[num] != null && players[num].myActorNum != 0)
            {
                rank = 1;

                for (int num_ = 0; num_ < 4; num_++)
                {
                    if (players[num_].myActorNum != 0 && players[num].score < players[num_].score)
                    {
                        rank++;
                    }
                }

                players[num].rank = rank;
            }
        }

    }

    public void InitPlayer()
    {
        localActorNum = PhotonNetwork.LocalPlayer.ActorNumber;

        int max_playercnt = PhotonNetwork.CurrentRoom.PlayerCount;

        for (int num = 0; num < 4; num++)
        {
            players[num] = new MolePlayer();

            if (num < max_playercnt)
            {
                players[num].name = PhotonNetwork.PlayerList[num].NickName;

                players[num].myActorNum = PhotonNetwork.PlayerList[num].ActorNumber;

                players[num].score = 0;

                players[num].rank = 0;

                players[num].loadup_complete = false;

                players[num].DB_Score = GameObject.Find("GameManager").GetComponent<DataBase>().SendRanks(players[num].name);
            }
            else
            {
                players[num].name = "";

                players[num].myActorNum = 0;

                players[num].score = 0;

                players[num].rank = 0;

                players[num].loadup_complete = false;

                players[num].DB_Score = 0;
            }
        }

        gameStart = true;

        GameObject.Find("GameManager").GetComponent<DataBase>().player_cnt = max_playercnt;
    }

    /*
    public void SendReady()
    {
        int num = GameObject.Find("LobbyManager").GetComponent<LobbyManager>().myActorNumber - 1;

        photonView.RPC("ReadyUp", RpcTarget.All, num);
    }

    [PunRPC]
    void ReadyUp(int num)
    {
        GameObject.Find("LobbyManager").GetComponent<LobbyManager>().isReady[num] = true;
    }
    */

    public void SendReady()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            System.Random rnd = new System.Random();

            photonView.RPC("ReadyUp", RpcTarget.All, rnd.Next(0, 200000));
        }
    }

    public void SetLoadUp()
    {
        photonView.RPC("SendLoadUp", RpcTarget.All, localActorNum);
    }

    public bool CheckAllLoadUp()
    {
        bool result = true;

        for (int num = 0; num < 4; num++)
        {
            if (players[num].myActorNum != 0 && players[num].loadup_complete == false)
                result = false;
        }

        return result;
    }

    public void UpdateDBRanks()
    {
        int fsp_cnt = 0;
        int scp_cnt = 0;
        int tdp_cnt = 0;
        int ftp_cnt = 0;

        int player_cnt = 0;

        for(int num=0;num < 4; num++)
        {
            if(players[num].myActorNum != 0)
            {
                player_cnt++;

                if(players[num].rank == 1)
                {
                    fsp_cnt++;
                }
                else if (players[num].rank == 2)
                {
                    scp_cnt++;
                }
                else if (players[num].rank == 3)
                {
                    tdp_cnt++;
                }
                else if (players[num].rank == 4)
                {
                    ftp_cnt++;
                }
            }
        }

        switch (player_cnt)
        {
            case 1:
                if (fsp_cnt == 1)
                {
                    for (int num = 0; num < 4; num++)
                    {
                        if (players[num].rank == 1)
                        {
                            if (players[num].myActorNum == localActorNum)
                                photonView.RPC("SendUpdatedRanks", RpcTarget.All, num, 0);
                            players[num].DB_Score += 0;
                        }
                    }
                }
                break;

            case 2:
                if (fsp_cnt == 1 && scp_cnt == 1)
                {
                    for (int num = 0; num < 4; num++)
                    {
                        if (players[num].rank == 1)
                        {
                            if (players[num].myActorNum == localActorNum)
                                photonView.RPC("SendUpdatedRanks", RpcTarget.All, num, 1);
                            players[num].DB_Score += 1;
                        }
                        else if (players[num].rank == 2)
                        {
                            if (players[num].myActorNum == localActorNum)
                                photonView.RPC("SendUpdatedRanks", RpcTarget.All, num, -1);
                            players[num].DB_Score += -1;
                        }
                    }
                }
                else if (fsp_cnt == 2)
                {
                    for (int num = 0; num < 4; num++)
                    {
                        if (players[num].rank == 1)
                        {
                            if (players[num].myActorNum == localActorNum)
                                photonView.RPC("SendUpdatedRanks", RpcTarget.All, players[num].name, 0);
                            players[num].DB_Score += 0;
                        }
                    }
                }
                break;

            case 3:
                if (fsp_cnt == 1 && scp_cnt == 1 && tdp_cnt == 1)
                {
                    for (int num = 0; num < 4; num++)
                    {
                        if (players[num].rank == 1)
                        {
                            if (players[num].myActorNum == localActorNum)
                                photonView.RPC("SendUpdatedRanks", RpcTarget.All, num, 2);
                            players[num].DB_Score += 2;
                        }
                        else if (players[num].rank == 2)
                        {
                            if (players[num].myActorNum == localActorNum)
                                photonView.RPC("SendUpdatedRanks", RpcTarget.All, num, 1);
                            players[num].DB_Score += 1;
                        }
                        else if (players[num].rank == 3m)
                        {
                            if (players[num].myActorNum == localActorNum)
                                photonView.RPC("SendUpdatedRanks", RpcTarget.All, num, -1);
                            players[num].DB_Score += -1;
                        }
                    }
                }
                else if (fsp_cnt == 1 && scp_cnt == 2)
                {
                    for (int num = 0; num < 4; num++)
                    {
                        if (players[num].rank == 1)
                        {
                            if (players[num].myActorNum == localActorNum)
                                photonView.RPC("SendUpdatedRanks", RpcTarget.All, num, 2);
                            players[num].DB_Score += 2;
                        }
                        else if (players[num].rank == 2)
                        {
                            if (players[num].myActorNum == localActorNum)
                                photonView.RPC("SendUpdatedRanks", RpcTarget.All, num, -1);
                            players[num].DB_Score += -1;
                        }
                    }
                }
                else if (fsp_cnt == 2 && scp_cnt == 1)
                {
                    for (int num = 0; num < 4; num++)
                    {
                        if (players[num].rank == 1)
                        {
                            if (players[num].myActorNum == localActorNum)
                                photonView.RPC("SendUpdatedRanks", RpcTarget.All, num, 1);
                            players[num].DB_Score += 1;
                        }
                        else if (players[num].rank == 2)
                        {
                            if (players[num].myActorNum == localActorNum)
                                photonView.RPC("SendUpdatedRanks", RpcTarget.All, num, -1);
                            players[num].DB_Score += -1;
                        }
                    }
                }
                else if (fsp_cnt == 3)
                {
                    for (int num = 0; num < 4; num++)
                    {
                        if (players[num].rank == 1)
                        {
                            if (players[num].myActorNum == localActorNum)
                                photonView.RPC("SendUpdatedRanks", RpcTarget.All, num, 0);
                            players[num].DB_Score += 0;
                        }
                    }
                }
                break;

            case 4:
                if (fsp_cnt == 1 && scp_cnt == 1 && tdp_cnt == 1 && ftp_cnt == 1)
                {
                    for (int num = 0; num < 4; num++)
                    {
                        if (players[num].rank == 1)
                        {
                            if (players[num].myActorNum == localActorNum)
                                photonView.RPC("SendUpdatedRanks", RpcTarget.All, num, 3);
                            players[num].DB_Score += 3;
                        }
                        else if (players[num].rank == 2)
                        {
                            if (players[num].myActorNum == localActorNum)
                                photonView.RPC("SendUpdatedRanks", RpcTarget.All, num, 1);
                            players[num].DB_Score += 1;
                        }
                        else if (players[num].rank == 3)
                        {
                            if (players[num].myActorNum == localActorNum)
                                photonView.RPC("SendUpdatedRanks", RpcTarget.All, num, 0);
                            players[num].DB_Score += 0;
                        }
                        else if (players[num].rank == 4)
                        {
                            if (players[num].myActorNum == localActorNum)
                                photonView.RPC("SendUpdatedRanks", RpcTarget.All, num, -1);
                            players[num].DB_Score += -1;
                        }
                    }
                }
                else if (fsp_cnt == 1 && scp_cnt == 1 && tdp_cnt == 2)
                {
                    for (int num = 0; num < 4; num++)
                    {
                        if (players[num].rank == 1)
                        {
                            if (players[num].myActorNum == localActorNum)
                                photonView.RPC("SendUpdatedRanks", RpcTarget.All, num, 3);
                            players[num].DB_Score += 3;
                        }
                        else if (players[num].rank == 2)
                        {
                            if (players[num].myActorNum == localActorNum)
                                photonView.RPC("SendUpdatedRanks", RpcTarget.All, num, 1);
                            players[num].DB_Score += 1;
                        }
                        else if (players[num].rank == 3)
                        {
                            if (players[num].myActorNum == localActorNum)
                                photonView.RPC("SendUpdatedRanks", RpcTarget.All, num, -1);
                            players[num].DB_Score += -1;
                        }
                    }
                }
                else if (fsp_cnt == 1 && scp_cnt == 2 && tdp_cnt == 1)
                {
                    for (int num = 0; num < 4; num++)
                    {
                        if (players[num].rank == 1)
                        {
                            if (players[num].myActorNum == localActorNum)
                                photonView.RPC("SendUpdatedRanks", RpcTarget.All, num, 3);
                            players[num].DB_Score += 3;
                        }
                        else if (players[num].rank == 2)
                        {
                            if (players[num].myActorNum == localActorNum)
                                photonView.RPC("SendUpdatedRanks", RpcTarget.All, num, 0);
                            players[num].DB_Score += 0;
                        }
                        else if (players[num].rank == 3)
                        {
                            if (players[num].myActorNum == localActorNum)
                                photonView.RPC("SendUpdatedRanks", RpcTarget.All, num, -1);
                            players[num].DB_Score += -1;
                        }
                    }
                }
                else if (fsp_cnt == 2 && scp_cnt == 1 && tdp_cnt == 1)
                {
                    for (int num = 0; num < 4; num++)
                    {
                        if (players[num].rank == 1)
                        {
                            if (players[num].myActorNum == localActorNum)
                                photonView.RPC("SendUpdatedRanks", RpcTarget.All, num, 2);
                            players[num].DB_Score += 2;
                        }
                        else if (players[num].rank == 2)
                        {
                            if (players[num].myActorNum == localActorNum)
                                photonView.RPC("SendUpdatedRanks", RpcTarget.All, num, 0);
                            players[num].DB_Score += 0;
                        }
                        else if (players[num].rank == 3)
                        {
                            if (players[num].myActorNum == localActorNum)
                                photonView.RPC("SendUpdatedRanks", RpcTarget.All, num, -1);
                            players[num].DB_Score += -1;
                        }
                    }
                }
                else if (fsp_cnt == 2 && scp_cnt == 2)
                {
                    for (int num = 0; num < 4; num++)
                    {
                        if (players[num].rank == 1)
                        {
                            if (players[num].myActorNum == localActorNum)
                                photonView.RPC("SendUpdatedRanks", RpcTarget.All, num, 1);
                            players[num].DB_Score += 1;
                        }
                        else if (players[num].rank == 2)
                        {
                            if (players[num].myActorNum == localActorNum)
                                photonView.RPC("SendUpdatedRanks", RpcTarget.All, num, 0);
                            players[num].DB_Score += 0;
                        }
                    }
                }
                else if (fsp_cnt == 1 && scp_cnt == 3)
                {
                    for (int num = 0; num < 4; num++)
                    {
                        if (players[num].rank == 1)
                        {
                            if (players[num].myActorNum == localActorNum)
                                photonView.RPC("SendUpdatedRanks", RpcTarget.All, num, 3);
                            players[num].DB_Score += 3;
                        }
                        else if (players[num].rank == 2)
                        {
                            if (players[num].myActorNum == localActorNum)
                                photonView.RPC("SendUpdatedRanks", RpcTarget.All, num, 0);
                            players[num].DB_Score += 0;
                        }
                    }
                }
                else if (fsp_cnt == 3 && scp_cnt == 1)
                {
                    for (int num = 0; num < 4; num++)
                    {
                        if (players[num].rank == 1)
                        {
                            if (players[num].myActorNum == localActorNum)
                                photonView.RPC("SendUpdatedRanks", RpcTarget.All, num, 1);
                            players[num].DB_Score += 1;
                        }
                        else if (players[num].rank == 2)
                        {
                            if (players[num].myActorNum == localActorNum)
                                photonView.RPC("SendUpdatedRanks", RpcTarget.All, num, -1);
                            players[num].DB_Score += -1;
                        }
                    }
                }
                else if (fsp_cnt == 4)
                {
                    for (int num = 0; num < 4; num++)
                    {
                        if (players[num].rank == 1)
                        {
                            if (players[num].myActorNum == localActorNum)
                                photonView.RPC("SendUpdatedRanks", RpcTarget.All, num, 0);
                            players[num].DB_Score += 0;
                        }
                    }
                }
                break;
        }
    }

    public void ResetPlayer()
    {
        startbuttonActive = false;

        gameStart = false;
    }

    [PunRPC]
    void ReadyUp(int rnd_seed)
    {
        for (int num = 0; num < 4; num++)
        {
            GameObject.Find("LobbyManager").GetComponent<LobbyManager>().isReady[num] = true;
        }

        GameObject.Find("Canvas").transform.Find("Lobby").gameObject.SetActive(false);

        //print(rnd_seed);

        GameObject.Find("GameManager").GetComponent<RandomPattern>().GetSeed(rnd_seed);

        GameObject.Find("Main Camera").GetComponent<MoveCamera>().CommandMoveToGameScene();
    }

    [PunRPC]
    void SendReceiveScore(int actorNum, int score)
    {
        for (int num = 0; num < 4; num++)
        {
            if (players[num] != null)
            {
                if (players[num].myActorNum == actorNum)
                {
                    players[num].score = score;
                }
            }
        }
    }

    [PunRPC]
    void SendLoadUp(int actorNum)
    {
        for (int num = 0; num < 4; num++)
        {
            if (players[num].myActorNum == actorNum)
            {
                players[num].loadup_complete = true;
            }
        }
    }

    [PunRPC]
    void SendUpdatedRanks(int num, int mmr)
    {
        if (players[num].myActorNum == localActorNum)
        {
            GameObject.Find("GameManager").GetComponent<DataBase>().UpdateRanks(mmr);
        }
    }
}
