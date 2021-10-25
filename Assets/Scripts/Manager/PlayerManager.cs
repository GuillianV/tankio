using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    //Prefab du Player
    public GameObject playerPrefab;
    //Conteneur du parent
    public Transform parentContainer;
   
    //Point de position du player
    public Vector2 spawnPosition =  Vector2.zero;
    
    //Scriptable objects pour les tanks
    [Header("Player Stats")]
    public TracksData TracksData;
    public BodyData BodyData;
    public TowerData TowerData;
    public GunData GunData;
    
    //Controller du player
    private TankController m_controller;
    
    //Player instancié
    [HideInInspector]
    public GameObject player;
    private void Awake()
    {
        //Création du player
        m_controller = playerPrefab.GetComponent<TankController>();

     
        if (TracksData != null)
        {
            m_controller.tracks.LoadData(TracksData);
        }
        if (BodyData != null)
        {
            m_controller.body.LoadData(BodyData);
        }
        if (TowerData != null)
        {
            m_controller.tower.LoadData(TowerData);
        }
        if (GunData != null)
        {
            m_controller.gun.LoadData(GunData);
        }
        
    }

    //Active le joueur
    public void EnablePlayer()
    {
        player.SetActive(true);
    }
    
    //Desactive le joueur
    public void DisablePlayer()
    {
        player.SetActive(false);
    }

    //Instancie le joueur
    public void InsatanciatePlayer()
    {
        player = Instantiate(playerPrefab, new Vector3(spawnPosition.x,spawnPosition.y,0), new Quaternion(0,0,0,0),parentContainer) as GameObject;
        
    }
}
