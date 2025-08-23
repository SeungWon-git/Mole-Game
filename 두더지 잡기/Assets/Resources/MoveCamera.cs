using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    enum State
    {
        Stay,
        MoveToLogin,
        MoveToLobby,
        MoveToGame,
        MoveToScoreBoard,
        MoveToRank,
        ReturnToLobby
    }

    State state;

    bool move;

    float accel_speed_x, accel_speed_y, accel_speed_z;

    float accel, accel_init;

    Vector3 target_loc;

    void Start()
    {
        state = State.Stay;

        move = false;

        accel = 0.005f;

        accel_init = 0.1f;
    }

    void Update()
    {
        if (move)
        {
            AccelMove();

            if (target_loc == transform.position)
            {
                if (state == State.MoveToGame)
                {
                    bool result = GameObject.Find("PlayerManager").GetComponent<PlayerManager>().CheckAllLoadUp();

                    if (result) 
                    {
                        StartGame_SetActiveObjects();

                        GameObject.Find("GameManager").GetComponent<DataBase>().SetStart(true);

                        state = State.Stay;

                        move = false;
                    }
                    else
                    {
                        GameObject.Find("PlayerManager").GetComponent<PlayerManager>().SetLoadUp();

                        state = State.MoveToGame;

                        move = true;
                    }
                }
                else
                {
                    if (state == State.MoveToLogin)
                    {
                        Login_SetActiveObjects();
                    }
                    else if (state == State.MoveToLobby)
                    {
                        Lobby_SetActiveObjects();
                    }
                    else if (state == State.MoveToScoreBoard)
                    {
                        ScoreBoard_SetActiveObjects();
                    }
                    else if (state == State.MoveToRank)
                    {
                        Rank_SetActiveObjects();
                    }
                    else if (state == State.ReturnToLobby)
                    {
                        Re_Lobby_SetActiveObjects();
                    }

                    state = State.Stay;

                    move = false;
                }
            }
        }
    }

    void AccelMove()
    {
        Vector3 current_loc = transform.position;

        if (target_loc.x - current_loc.x > 0 && accel_speed_x > 0)
        {
            accel_speed_x += accel;

            transform.position = new Vector3(transform.position.x + accel_speed_x, transform.position.y, transform.position.z);
        }
        else if (target_loc.x - current_loc.x <= 0 && accel_speed_x > 0)
        {
            transform.position = new Vector3(target_loc.x, transform.position.y, transform.position.z);
        }
        else if (target_loc.x - current_loc.x < 0 && accel_speed_x < 0)
        {
            accel_speed_x -= accel;

            transform.position = new Vector3(transform.position.x + accel_speed_x, transform.position.y, transform.position.z);
        }
        else if (target_loc.x - current_loc.x >= 0 && accel_speed_x < 0)
        {
            transform.position = new Vector3(target_loc.x , transform.position.y, transform.position.z);
        }

        if (target_loc.y - current_loc.y > 0 && accel_speed_y > 0)
        {
            accel_speed_y += accel;

            transform.position = new Vector3(transform.position.x, transform.position.y + accel_speed_y, transform.position.z);
        }
        else if (target_loc.y - current_loc.y <= 0 && accel_speed_y > 0)
        {
            transform.position = new Vector3(transform.position.x, target_loc.y, transform.position.z);
        }
        else if (target_loc.y - current_loc.y < 0 && accel_speed_y < 0)
        {
            accel_speed_y -= accel;

            transform.position = new Vector3(transform.position.x, transform.position.y + accel_speed_y, transform.position.z);
        }
        else if (target_loc.y - current_loc.y >= 0 && accel_speed_y < 0)
        {
            transform.position = new Vector3(transform.position.x, target_loc.y, transform.position.z);
        }

        if (target_loc.z - current_loc.z > 0 && accel_speed_z > 0)
        {
            accel_speed_z += accel;

            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + accel_speed_z);
        }
        else if (target_loc.z - current_loc.z <= 0 && accel_speed_z > 0)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, target_loc.z);
        }
        else if (target_loc.z - current_loc.z < 0 && accel_speed_z < 0)
        {
            accel_speed_z -= accel;

            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + accel_speed_z);
        }
        else if (target_loc.z - current_loc.z >= 0 && accel_speed_z < 0)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, target_loc.z);
        }
    }

    void DetermineSpeed()
    {
        Vector3 current_loc = transform.position;

        if (target_loc.x - current_loc.x >= 0)
            accel_speed_x = accel_init;
        else
            accel_speed_x = (-1) * accel_init;

        if (target_loc.y - current_loc.y >= 0)
            accel_speed_y = accel_init;
        else
            accel_speed_y = (-1) * accel_init;

        if (target_loc.z - current_loc.z >= 0)
            accel_speed_z = accel_init;
        else
            accel_speed_z = (-1) * accel_init;
    }

    ///////////////////////////////////////////////////////////////////////////

    public void CommandMoveToLogin()
    {
        state = State.MoveToLogin;

        target_loc = new Vector3(41, 10, 30);

        DetermineSpeed();

        move = true;
    }

    public void CommandMoveToLobby()
    {
        state = State.MoveToLobby;

        target_loc = new Vector3(41, 10, 35);

        DetermineSpeed();

        move = true;
    }

    public void CommandMoveToGameScene()
    {
        state = State.MoveToGame;

        target_loc = new Vector3(0, 10, -16);

        DetermineSpeed();

        move = true;
    }

    public void CommandMoveToScoreBoard()
    {
        state = State.MoveToScoreBoard;

        target_loc = new Vector3(0, 10, -55);

        DetermineSpeed();

        move = true;
    }

    public void CommandMoveToRank()
    {
        state = State.MoveToRank;

        target_loc = new Vector3(0, 10, -48);

        DetermineSpeed();

        move = true;
    }

    public void CommandRetrunToLobby()
    {
        state = State.ReturnToLobby;

        target_loc = new Vector3(41, 10, 35);

        DetermineSpeed();

        move = true;
    }

    ///////////////////////////////////////////////////////////////////////////

    void Login_SetActiveObjects()
    {
        GameObject.Find("Canvas").transform.Find("Lobby").gameObject.SetActive(true);

        GameObject.Find("LobbyManager").GetComponent<LobbyManager>().enabled = true;
    }

    void Lobby_SetActiveObjects()
    {
        GameObject.Find("Canvas").transform.Find("Lobby").transform.Find("LobbyRoom").gameObject.SetActive(true);

        GameObject.Find("PlayerManager").GetComponent<PlayerManager>().enabled = true;
    }

    void StartGame_SetActiveObjects()
    {
        GameObject.Find("Canvas").transform.Find("GameScene").gameObject.SetActive(true);

        GameObject.Find("GameObject").transform.Find("¸ÁÄ¡").GetComponent<SwingHammer>().enabled = true;

        GameObject.Find("GameObject").transform.Find("¸ÁÄ¡").GetComponent<FollowCursor>().enabled = true;

        GameObject.Find("GameManager").GetComponent<Score>().In_Game(true);

        GameObject.Find("GameManager").GetComponent<MoleManager>().Reset();

        GameObject.Find("GameManager").GetComponent<MoleManager>().In_Game(true);

        GameObject.Find("LobbyManager").GetComponent<LobbyManager>().enabled = false;
    }

    void ScoreBoard_SetActiveObjects()
    {
        GameObject.Find("Canvas").transform.Find("EndGame").gameObject.SetActive(true);

        GameObject.Find("ScoreBoardText").GetComponent<GetResult>().ResetScoreBoard();
    }

    void Rank_SetActiveObjects()
    {
        GameObject.Find("Canvas").transform.Find("EndGame").transform.Find("RankBoardText").gameObject.SetActive(true);

        GameObject.Find("RankBoardText").GetComponent<GetRank>().ResetRankBoard();

        GameObject.Find("Canvas").transform.Find("EndGame").transform.Find("BackToLobbyButton").gameObject.SetActive(true);
    }

    void Re_Lobby_SetActiveObjects()
    {
        GameObject.Find("Canvas").transform.Find("Lobby").gameObject.SetActive(true);

        GameObject.Find("Canvas").transform.Find("Lobby").transform.Find("LobbyRoom").gameObject.SetActive(true);

        GameObject.Find("PlayerManager").GetComponent<PlayerManager>().ResetPlayer();

        GameObject.Find("LobbyManager").GetComponent<LobbyManager>().enabled = true;

        GameObject.Find("LobbyManager").GetComponent<LobbyManager>().ResetLobby();
    }
}
