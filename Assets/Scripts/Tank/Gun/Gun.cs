using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour, IUpgradable
{
    
    public GunData Data { get; private set; }
    private GameManager m_Game;
    
    private void Awake()
    {
        m_Game = GameManager.Instance;
        
    }
    //Load Base Data of scriptable object
    public void LoadData(GunData _data)
    {
        Data = _data;
    }
    
    //Upgrade Gun Stats when IUpgradable is trigered
    void IUpgradable.Upgrade()
    {
        Debug.Log("Gun Upgraded");
        m_Game.Player.UpgradeGun();
    }
        
   
}
