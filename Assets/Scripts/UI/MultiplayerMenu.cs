using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerMenu : MonoBehaviour
{
    public GameObject menuPlay;
    public GameObject menuMultiplayer;
    public GameObject menuCreate;
    public GameObject menuJoin;

    private SocketManager m_Socket;


    public void Awake()
    {
        m_Socket = SocketManager.Instance;
    }

    public void Back()
    {
        menuPlay.SetActive(true);
        menuMultiplayer.SetActive(false);
    }


    public void Create()
    {
        m_Socket.Send(S_Creation.ToJson(m_Socket.socketID));
        menuCreate.SetActive(true);
        menuMultiplayer.SetActive(false);
    }

    public void Join()
    {
        menuJoin.SetActive(true);
        menuMultiplayer.SetActive(false);
    }
}
