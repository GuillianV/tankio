using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{


    public GameObject playerPrefab;
    private TankController m_controller;

    public Vector2 spawnPosition =  Vector2.zero;
    
    [Header("Player Stats")]
    public TracksData TracksData;
    public BodyData BodyData;
    public TowerData TowerData;
    public GunData GunData;
    
    
    [HideInInspector]
    public GameObject player;
    // Start is called before the first frame update

    private void Awake()
    {
        m_controller = playerPrefab.GetComponent<TankController>();

     
        if (TracksData != null)
        {
            m_controller.tracks.LoadData(TracksData);
        }
        if (TracksData != null)
        {
            m_controller.body.LoadData(BodyData);
        }
        if (TracksData != null)
        {
            m_controller.tower.LoadData(TowerData);
        }
        if (TracksData != null)
        {
            m_controller.gun.LoadData(GunData);
        }
        
    }

    public void EnablePlayer()
    {
        player.SetActive(true);
    }
    
    public void DisablePlayer()
    {
        player.SetActive(false);
    }

    public void InsatanciatePlayer()
    {
        player = Instantiate(playerPrefab, new Vector3(spawnPosition.x,spawnPosition.y,0), new Quaternion(0,0,0,0)) as GameObject;
        
    }
}
