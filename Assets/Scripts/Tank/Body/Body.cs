using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TankController))]
public class Body : MonoBehaviour ,IUpgradable
{
    public BodyData Data { get; private set; }
    private GameManager m_Game;
    
    private void Awake()
    {
        m_Game = GameManager.Instance;
        
    }

    //Load Base Data of scriptable object
    public void LoadData(BodyData _data)
    {
        Data = _data;

    }
    
    //Upgrade Body Stats when IUpgradable is trigered
    void IUpgradable.Upgrade()
    {
        Debug.Log("Body Upgraded");
        m_Game.Player.UpgradeBody();
    }
}
