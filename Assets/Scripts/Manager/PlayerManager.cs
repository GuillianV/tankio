using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



[CustomEditor(typeof(PlayerManager))]
class PlayerButtons : Editor {
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        
        GameManager m_game = GameManager.Instance;
        PlayerManager playerManager = (PlayerManager) target;
        
        if (GUILayout.Button("Reset All"))
        {
            playerManager.ResetPlayerManager();
        }
  
            
    }
}


public class PlayerManager : MonoBehaviour
{

    //Prefab du Player
    public GameObject playerPrefab;

    //Conteneur du parent
    public Transform parentContainer;


    //Scriptable objects pour les tanks
    [Header("Player Stats")]
    public TracksData TracksData;
    public BodyData BodyData;
    public TowerData TowerData;
    public GunData GunData;
    
    [Range(100,2000)]
    public float tracksSpeed;
    [Range(100,2000)]
    public float tracksRotationSpeed;
    [Range(0,10000)]
    public float health;
    [Range(1,100)]
    public float towerRotationSpeed;
    [Range(0,100)]
    public float bulletVelocity;
    [Range(0,10)]
    public float reloadTimeSpeed;
    
    //Player instancié
    [HideInInspector]
    public GameObject player;

    private GameManager m_Game;

    private void Awake()
    {
        m_Game = GameManager.Instance;
        
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
        player = Instantiate(playerPrefab, new Vector3(parentContainer.transform.position.x,parentContainer.transform.position.y,0), new Quaternion(0,0,0,0),parentContainer) as GameObject;
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

        if (tankController.StatsController != null)
        {
            tracksSpeed = tankController.StatsController.tracksSpeed;
            tracksRotationSpeed = tankController.StatsController.tracksRotationSpeed;
            health = tankController.StatsController.health;
            towerRotationSpeed = tankController.StatsController.towerRotationSpeed;
            bulletVelocity = tankController.StatsController.bulletVelocity;
            reloadTimeSpeed = tankController.StatsController.reloadTimeSpeed;

        }
        
        
    }

    private void OnValidate()
    {
        if (player != null)
        {
            TankController tankController = player.GetComponent<TankController>();

            if (tankController != null)
            {
                tankController.StatsController.tracksSpeed = tracksSpeed ;
                tankController.StatsController.tracksRotationSpeed = tracksRotationSpeed ;
                tankController.StatsController.health = health ;
                tankController.StatsController.towerRotationSpeed = towerRotationSpeed;
                tankController.StatsController.bulletVelocity = bulletVelocity;
                tankController.StatsController.reloadTimeSpeed = reloadTimeSpeed;

            }
        }
       
    }


    public void DestroyPlayer()
    {
        foreach (Transform child in parentContainer)
            Destroy(child.gameObject);
    }
    
    public void CreatePlayer()
    {
        InsatanciatePlayer();
    }
    
    public void SetCameraFollowPlayer()
    {
        m_Game.Camera.SetGameObjectToFollow(player);
    }
    
    public void SetUiLifeOfPlayer()
    {
        TankController tankController = player.GetComponent<TankController>();
        m_Game.Ui.SetLifeUI(tankController.StatsController.maxHealth,tankController.StatsController.maxHealth );

    }


    public void ResetPlayerManager()
    {
       DestroyPlayer();
       CreatePlayer();
       SetCameraFollowPlayer();
       SetUiLifeOfPlayer();
    }
    
}
