using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    string gameVersion = "1.3";

    public Text connectionInfoText;

    GameObject id_text;

    public Button joinButton;

    bool inLobby;

    int pres_player_cnt;
    int prev_player_cnt;

    public bool[] isReady;

    bool squish;

    GameObject lobby_mole;
    GameObject lobby_hammer;

    void Start()
    {
        PhotonNetwork.GameVersion = gameVersion;

        PhotonNetwork.ConnectUsingSettings();

        joinButton.interactable = false;

        connectionInfoText.text = "������ ������ ������...";

        id_text = GameObject.Find("Canvas").transform.Find("Lobby").transform.Find("LobbyRoom").gameObject;

        lobby_mole = GameObject.Find("GameObject").transform.Find("�δ���").gameObject;
        lobby_hammer = GameObject.Find("GameObject").transform.Find("�κ� ��ġ").gameObject;


        inLobby = false;
        squish = false;

        pres_player_cnt = 0;
        prev_player_cnt = 0;

        isReady = new bool[4];
    }

    public void ResetLobby()
    {
        inLobby = true;

        UpdatePlayerInfo();

        for (int i = 0; i < 4; i++)
            isReady[i] = false;

        squish = false;
    }

    void Update()
    {
        if (inLobby)
        {
            pres_player_cnt = PhotonNetwork.CurrentRoom.PlayerCount;

            if (pres_player_cnt != prev_player_cnt) 
                UpdatePlayerInfo();

            prev_player_cnt = pres_player_cnt;

            if (CheckAllReady())
            {
                GameObject.Find("PlayerManager").GetComponent<PlayerManager>().InitPlayer();

                inLobby = false;

                squish = true;
            }
        }
        else
        {
            if (squish)
            {
                int maxPlayerCount = PhotonNetwork.CurrentRoom.PlayerCount;

                for (int num = 0; num < maxPlayerCount; num++)
                {
                    GameObject mole = lobby_mole.transform.Find("�κ� �δ��� (" + (num + 1).ToString() + ")").gameObject;
                    GameObject hammer = lobby_hammer.transform.Find("�κ� ��ġ (" + (num + 1).ToString() + ")").gameObject;

                    if (mole.transform.localScale.y > 1.0f)
                    {
                        mole.transform.Find("Burrow").GetComponent<SkinnedMeshRenderer>().material.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
                        mole.transform.localScale += new Vector3(0.05f, -0.05f, 0.05f);
                        hammer.transform.Rotate(0f, 0f, 3.0f);
                    }
                    else
                    {
                        mole.transform.Find("Burrow").GetComponent<SkinnedMeshRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                        mole.transform.localScale = new Vector3(3.0f, 3.0f, 3.0f);
                        mole.transform.position = new Vector3(mole.transform.position.x, -5, mole.transform.position.z);
                        hammer.transform.rotation = Quaternion.Euler(new Vector3(10, 80, 120));
                        hammer.SetActive(false);

                        squish = false;
                    }
                }
            }
        }
    }

    public override void OnConnectedToMaster()
    {
        joinButton.interactable = true;

        connectionInfoText.text = "�¶���: ������ ������ �����";
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        joinButton.interactable = false;

        connectionInfoText.text = "��������: ������ ������ ������� ����\n���� ��õ� ��...";

        PhotonNetwork.ConnectUsingSettings();
    }

    public void Connect()
    {
        //joinButton.interactable = false;

        if (PhotonNetwork.IsConnected)
        {
            connectionInfoText.text = "�뿡 ���� ��...";
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            connectionInfoText.text = "��������: ������ ������ ������� ����\n���� ��õ� ��...";
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        connectionInfoText.text = "�� ���� ����, ���ο� �� ����...";
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 });
    }

    public void InputNickName(Text text)
    {
        PhotonNetwork.NickName = text.text.ToString();
    }

    public override void OnJoinedRoom()
    {
        connectionInfoText.text = "�� ���� ����";

        inLobby = true;
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        connectionInfoText.text = "�κ�� ���ƿ�";
    }

    void UpdatePlayerInfo()
    {
        id_text = GameObject.Find("Canvas").transform.Find("Lobby").transform.Find("LobbyRoom").gameObject;

        string player_id_num;

        int num = 1;

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            player_id_num = "PlayerID (" + num.ToString() + ")";

            id_text.transform.Find(player_id_num).GetComponent<Text>().text = player.NickName + "\n" + GameObject.Find("GameManager").GetComponent<DataBase>().SendRanks(player.NickName).ToString() + "��";

            id_text.transform.Find(player_id_num).GetComponent<Text>().color = new Color(1, 1, 1, 1);

            //if (!isReady[num - 1])
            //    id_text.transform.Find(player_id_num).GetComponent<Text>().color = new Color(1, 0, 0, 1);
            //else
            //    id_text.transform.Find(player_id_num).GetComponent<Text>().color = new Color(0, 1, 0, 1);

            GameObject mole = lobby_mole.transform.Find("�κ� �δ��� (" + num.ToString() + ")").gameObject;

            mole.transform.position = new Vector3(mole.transform.position.x, -0.5f, mole.transform.position.z);

            lobby_hammer.transform.Find("�κ� ��ġ (" + num.ToString() + ")").gameObject.SetActive(true);

            num++;
        }
    }

    bool CheckAllReady()
    {
        bool allReady = true;

        for (int i = 0; i < 4; i++)
        {
            if (!isReady[i])
            {
                allReady = false;
            }
        }

        return allReady;
    }
}
