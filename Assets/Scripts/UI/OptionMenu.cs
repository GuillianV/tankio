using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class OptionMenu : MonoBehaviour
{
    public GameObject mainMenu;

    public void Start()
    {
        
    }

    public void Back()
    {
        mainMenu.SetActive(true);
        gameObject.SetActive(false);
    }
  
  
}
