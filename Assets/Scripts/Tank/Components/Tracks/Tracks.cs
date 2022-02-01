using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TankController))]
public class Tracks : MonoBehaviour 
{
    
    
    public TracksData Data { get; private set; }
    private GameManager m_Game;

    private void Awake()
    {
       m_Game = GameManager.Instance;
    }
    //Load Base Data of scriptable object
    public void LoadData(TracksData _data)
    {
        Data = _data;

    }

    public ScriptableObject GetBaseData()
    {
        return Data;
    }


    
}
