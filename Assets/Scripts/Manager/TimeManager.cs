using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TimeManager : MonoBehaviour
{
    public float timeScale { get; private set; }
    public event EventHandler<BoolEventargs> OnTimeChanged;
    void Start()
    {
        timeScale = 1;
    }
    
    

    public void EnableTime()
    {
        timeScale = 1;
        OnTimeChanged?.Invoke(this,new BoolEventargs(true));
    }

    public void DisableTime()
    { 
        timeScale = 0;
        OnTimeChanged?.Invoke(this,new BoolEventargs(false));
    }
    
    
}
