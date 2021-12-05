using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(PlayerManager))]
class PlayerButtons : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();


        GameManager m_game = GameManager.Instance;
        PlayerManager playerManager = (PlayerManager) target;

        if (GUILayout.Button("Reset All"))
        {
            playerManager.ResetPlayerManager();
        }

        if (GUILayout.Button("Reset Stats"))
        {
            playerManager.ResetStats();
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
    [Header("Player Stats")] public TracksData TracksData;
    public BodyData BodyData;
    public TowerData TowerData;
    public GunData GunData;

    [ReadOnly] public float tracksSpeed;
    [Range(0.01f, 1)] public float coeffTracksSpeedUpgrade = 0.1f;
    [ReadOnly] public float tracksRotationSpeed;
    [Range(0.01f, 1)] public float coeffRotationSpeedUpgrade = 0.1f;
    [ReadOnly] public float health;
    [Range(0.01f, 1)] public float coeffHealthUpgrade = 0.1f;
    [ReadOnly] public float towerRotationSpeed;
    [Range(0.01f, 1)] public float coeffTowerRotationSpeedUpgrade = 0.1f;
    [ReadOnly] public float bulletVelocity;
    [Range(0.01f, 1)] public float coeffBulletVelocityUpgrade = 0.1f;
    [ReadOnly] public float reloadTimeSpeed;
    [Range(0.01f, 1)] public float coeffReloadTimeSpeedUpgrade = 0.1f;

    //Player instancié
    [HideInInspector] public GameObject player;

    private GameManager m_Game;

    private void Awake()
    {
        m_Game = GameManager.Instance;
    }


    //Rebinds stats of tanks when inspector changed
    private void OnValidate()
    {
        if (player != null)
        {
            TankController tankController = player.GetComponent<TankController>();

            if (tankController != null)
            {
                tankController.StatsController.tracksSpeed = tracksSpeed;
                tankController.StatsController.tracksRotationSpeed = tracksRotationSpeed;
                tankController.StatsController.health = health;
                tankController.StatsController.towerRotationSpeed = towerRotationSpeed;
                tankController.StatsController.bulletVelocity = bulletVelocity;
                tankController.StatsController.reloadTimeSpeed = reloadTimeSpeed;
            }
        }
    }


    #region CreatePlayer

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
        player = Instantiate(playerPrefab,
            new Vector3(parentContainer.transform.position.x, parentContainer.transform.position.y, 0),
            new Quaternion(0, 0, 0, 0), parentContainer) as GameObject;
        //Création du player
        ResetStats();
    }

    //Reset all bonus stats and upgrade of tanks
    public void ResetStats()
    {
        if (player != null)
        {
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
        else
        {
            Debug.LogWarning("Cant Find Any Player");
        }
    }

    //Destroy player
    public void DestroyPlayer()
    {
        foreach (Transform child in parentContainer)
            Destroy(child.gameObject);
    }

    //Create new Player
    public void CreatePlayer()
    {
        InsatanciatePlayer();
    }

    //Set camera follow new player
    public void SetCameraFollowPlayer()
    {
        m_Game.Camera.SetGameObjectToFollow(player);
    }

    //Set life of player
    public void SetUiLifeOfPlayer()
    {
        TankController tankController = player.GetComponent<TankController>();
        m_Game.Ui.SetLifeUI(tankController.StatsController.maxHealth, tankController.StatsController.maxHealth);
    }

    #endregion

    #region UpgradePlayer

    public void UpgradeTracks()
    {
        TankController tankController = player.GetComponent<TankController>();

        tankController.StatsController.tracksSpeed = tankController.StatsController.tracksSpeed +
                                                     (coeffTracksSpeedUpgrade * tankController.TracksController.tracks
                                                         .Data.speed);
        tracksSpeed = tankController.StatsController.tracksSpeed;

        tankController.StatsController.tracksRotationSpeed = tankController.StatsController.tracksRotationSpeed +
                                                             (coeffRotationSpeedUpgrade * tankController
                                                                 .TracksController.tracks.Data.rotationSpeed);
        tracksRotationSpeed = tankController.StatsController.tracksRotationSpeed;
    }

    public void UpgradeBody()
    {
        TankController tankController = player.GetComponent<TankController>();

        tankController.StatsController.maxHealth = tankController.StatsController.maxHealth +
                                                   (coeffHealthUpgrade * tankController.BodyController.body.Data.life);

        tankController.StatsController.health = tankController.StatsController.health +
                                                (coeffHealthUpgrade * tankController.BodyController.body.Data.life);
        health = tankController.StatsController.health;


        m_Game.Ui.SetLifeUI(tankController.StatsController.maxHealth, tankController.StatsController.health);
    }

    public void UpgradeTower()
    {
        TankController tankController = player.GetComponent<TankController>();

        tankController.StatsController.towerRotationSpeed = tankController.StatsController.towerRotationSpeed +
                                                            (coeffTowerRotationSpeedUpgrade *
                                                             tankController.TowerController.tower.Data.rotationSpeed);
        towerRotationSpeed = tankController.StatsController.towerRotationSpeed;
    }


    public void UpgradeGun()
    {
        TankController tankController = player.GetComponent<TankController>();

        tankController.StatsController.bulletVelocity = tankController.StatsController.bulletVelocity +
                                                        (coeffBulletVelocityUpgrade * tankController.GunController.gun
                                                            .Data.bulletVelocity);
        bulletVelocity = tankController.StatsController.bulletVelocity;

        tankController.StatsController.reloadTimeSpeed = tankController.StatsController.reloadTimeSpeed -
                                                         (reloadTimeSpeed * tankController.GunController.gun.Data
                                                             .reloadTimeSecond);
        reloadTimeSpeed = tankController.StatsController.reloadTimeSpeed;
    }

    #endregion


    public void ResetPlayerManager()
    {
        DestroyPlayer();
        CreatePlayer();
        SetCameraFollowPlayer();
        SetUiLifeOfPlayer();
        m_Game.Shop.ResetShopManager();
        m_Game.Ui.ResetShopUIManager();
    }
}