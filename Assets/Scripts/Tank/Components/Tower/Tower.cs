using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public TowerData Data { get; private set; }
    private GameManager m_Game;


    private void Awake()
    {
        m_Game = GameManager.Instance;
        
    }
    //Load Base Data of scriptable object
    public void LoadData(TowerData _data)
    {
        Data = _data;
    }
    public ScriptableObject GetBaseData()
    {
        return Data;
    }

  
}
