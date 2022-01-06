using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerMenu : MonoBehaviour
{
    public GameObject menuPlay;
    public GameObject menuMultiplayer;


    public void Back()
    {
        menuPlay.SetActive(true);
        menuMultiplayer.SetActive(false);
    }
}
