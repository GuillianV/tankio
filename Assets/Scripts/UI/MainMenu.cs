using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public GameObject optionsMenu;
    public GameObject mainMenu;
    
      public void PlayGame()
      {
          SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
