using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public GameObject optionsMenu;
    public GameObject mainMenu;
    public GameObject playMenu;

    public void Play()
    {
        playMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void QuitGame()
      {
          Application.Quit();
      }
      
      public void Options()
      {
          optionsMenu.SetActive(true);
          mainMenu.SetActive(false);
      }


}
