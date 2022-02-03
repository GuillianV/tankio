using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;



public class PlayerAimer : PlayerController
{

    
    private TowerManager m_towerManager;
    private TowerController m_towerController;
    public Transform towerTransform;


#if UNITY_STANDALONE

    private GameManager m_Game;
    
     
    protected override void Awake()
    {
        base.Awake();
        
        m_Game = GameManager.Instance;
        
        m_towerManager = m_tankController.GetTankManager<TowerManager>();
        m_towerController = m_towerManager.towerController;
        
        if(m_towerController == null)
            Debug.LogError("Player Aimer missing TowerController");

    }


    void FixedUpdate()
    {   
            Vector3 pz = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 vectorToTarget = new Vector3(pz.x,pz.y,towerTransform.position.z)  - towerTransform.position;     
            Quaternion q = TMath.GetAngleFromVector2D(vectorToTarget, -90);
            towerTransform.rotation =  Quaternion.Slerp(towerTransform.rotation, q, Time.deltaTime  * m_Game.TimeManager.timeScale* m_towerController.GetTowerRotationSpeed());

    }


#endif

}
