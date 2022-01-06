using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayMenu : MonoBehaviour
{

    public GameObject menuMain;
    public GameObject menuPlay;
    public GameObject menuMultiplayer;

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void PlayMulti()
    {
        menuMultiplayer.SetActive(true);
        menuPlay.SetActive(false);
    }


    public void Back()
    {
        menuMain.SetActive(true);
        menuPlay.SetActive(false);
    }
}
