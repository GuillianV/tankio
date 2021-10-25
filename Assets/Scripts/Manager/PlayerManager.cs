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
    
    
    //Player instancié
    [HideInInspector]
    public GameObject player;

    
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
        //Création du player
        TankController tankController = player.GetComponent<TankController>();

     
        if (TracksData != null)
        {
            tankController.TracksController.tracks.LoadData(TracksData);
        }
        if (BodyData != null)
        {
            tankController.BodyController.body.LoadData(BodyData);
        }
        
        
        if (TowerData != null)
        {
            tankController.TowerController.tower.LoadData(TowerData);
        }
        if (GunData != null)
        {
            tankController.GunController.gun.LoadData(GunData);
        }
        
        tankController.BindSprite();
        tankController.BindStats();

    }
}
