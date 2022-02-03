using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : TankBase
{
    
    public GunData Data { get; private set; }
    private GameManager m_Game;
    
    private void Awake()
    {
        m_Game = GameManager.Instance;
        
    }
    //Load Base Data of scriptable object
    public override void LoadData(ScriptableObject _data)
    {
        Data = (GunData) _data;
    }

    public override ScriptableObject GetBaseData()
    {
        return Data;
    }

   
   
}
