using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayMenu : MonoBehaviour
{

    public GameObject menuMain;
    public GameObject menuPlay;
    public GameObject menuMultiplayer;

    private SocketManager m_Socket;


    public void Awake()
    {
        m_Socket = SocketManager.Instance;
    }

  


    public void PlayGame()
    {
        m_Socket.ConnectServer();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        
    }

    public void PlayMulti()
    {
        menuMultiplayer.SetActive(true);
        menuPlay.SetActive(false);
        m_Socket.ConnectServer();
    }


    public void Back()
    {
        menuMain.SetActive(true);
        menuPlay.SetActive(false);
    }
}
