using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
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
#endif

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
    private TankController tankController;
    private TracksController TracksController;
    private BodyController BodyController;
    private TowerController TowerController;
    private GunController GunController;

    public event EventHandler<TankEvent> OnPlayerDestroyed;
    public event EventHandler<TankEvent> OnPlayerCreated;

    private void Awake()
    {
        m_Game = GameManager.Instance;
    }


    //Rebinds stats of tanks when inspector changed
    private void OnValidate()
    {
        if (player != null)
        {
            tankController = player.GetComponent<TankController>();

            if (tankController != null)
            {
                TracksController = player.GetComponent<TracksController>();
                BodyController = player.GetComponent<BodyController>();
                TowerController = player.GetComponent<TowerController>();
                GunController = player.GetComponent<GunController>();

                TracksController.SetTrackSpeed(tracksSpeed);
                TracksController.SetTrackRotationSpeed(tracksRotationSpeed);
                BodyController.SetHealt(health);
                TowerController.SetTowerRotationSpeed(towerRotationSpeed);
                GunController.SetBulletVelocity(bulletVelocity);
                GunController.SetReloadTimeSpeed(reloadTimeSpeed);
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

    
    public void PlayerDestroy(object sender, TagEvent args)
    {

        TankDestroyed tankDestroyed = sender as TankDestroyed;
        if (tankDestroyed.gameObject)
        {
            TankController currentTankController = tankDestroyed.GetComponent<TankController>();
            OnTankDestroyed(currentTankController);
            m_Game.Ui.ShowDeadMenu();
        }
      
    }
    
    public void PlayerCreated(object sender, EventArgs args)
    {
        TankCreate tankCreate = sender as TankCreate;
        TankController currentTankController = tankCreate.GetComponent<TankController>();
        OnTankCreated(currentTankController);
    }
    
    public void OnTankDestroyed(TankController currentTankController)
    {
        OnPlayerDestroyed?.Invoke(this,new TankEvent(currentTankController));
    }
    
    public void OnTankCreated(TankController currentTankController)
    {
        OnPlayerCreated?.Invoke(this,new TankEvent(currentTankController));
    }

    
    
    //Instancie le joueur
    public void InsatanciatePlayer()
    {
        player = Instantiate(playerPrefab,
            new Vector3(parentContainer.transform.position.x, parentContainer.transform.position.y, 0),
            new Quaternion(0, 0, 0, 0), parentContainer) as GameObject;
        //Création du player
        TankDestroyed tankDestroyed = player.GetComponent<TankDestroyed>();
        TankCreate tankCreate = player.GetComponent<TankCreate>();

        tankDestroyed.Destroyed += PlayerDestroy;
        tankCreate.Created += PlayerCreated;   
        m_Game.Projectile.LoadPlayerShooter();
        ResetStats();
    }

    
    
    
    //Reset all bonus stats and upgrade of tanks
    public void ResetStats()
    {
        if (player != null)
        {
            tankController = player.GetComponent<TankController>();
            TracksController = player.GetComponent<TracksController>();
            BodyController = player.GetComponent<BodyController>();
            TowerController = player.GetComponent<TowerController>();
            GunController = player.GetComponent<GunController>();

            List<ITankComponent> tankComponentsList = tankController.GetComponents<ITankComponent>().ToList();

            if (TracksData != null)
            {
              

                ITankComponent tracksComponent = tankComponentsList.FirstOrDefault(component => component.ToString().Contains("TracksController"));
                tracksComponent.BindData(TracksData);
                tracksComponent.BindStats();
            }

            if (BodyData != null)
            {
              

                ITankComponent bodyComponent = tankComponentsList.FirstOrDefault(component => component.ToString().Contains("BodyController"));
                bodyComponent.BindData(BodyData);
                bodyComponent.BindStats();
            }


            if (TowerData != null)
            {
                ITankComponent towerComponent = tankComponentsList.FirstOrDefault(component => component.ToString().Contains("TowerController"));
                towerComponent.BindData(TowerData);
                towerComponent.BindStats();
            }

            if (GunData != null)
            {
                ITankComponent gunComponent = tankComponentsList.FirstOrDefault(component => component.ToString().Contains("GunController"));
                gunComponent.BindData(GunData);
                gunComponent.BindStats();
            }



            tankController.BindSprite();
            tankController.BindStats();

            if (tankController != null)
            {
                tracksSpeed = TracksController.GetTrackSpeed();
                tracksRotationSpeed = TracksController.GetTrackRotationSpeed();
                health = BodyController.GetHealt();
                towerRotationSpeed = TowerController.GetTowerRotationSpeed();
                bulletVelocity = GunController.GetBulletVelocity();
                reloadTimeSpeed = GunController.GetReloadTimeSpeed();
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
        if(tankController)
             m_Game.Ui.SetLifeUI(BodyController.GetMaxHealt(), BodyController.GetMaxHealt());
    }

    #endregion

    #region UpgradePlayer

    public void UpgradeTracks()
    {

        TracksData initialTracksData = TracksController.GetBaseData();

        TracksController.SetTrackSpeed( TracksController.GetTrackSpeed() +
                                                     (coeffTracksSpeedUpgrade * initialTracksData.speed));

        tracksSpeed = TracksController.GetTrackSpeed();



        TracksController.SetTrackRotationSpeed(TracksController.GetTrackRotationSpeed() +
                                                             (coeffRotationSpeedUpgrade * initialTracksData.rotationSpeed));

        tracksRotationSpeed = TracksController.GetTrackRotationSpeed();
    }

    public void UpgradeBody()
    {

        BodyData initialBodyData = BodyController.GetBaseData();


        BodyController.SetMaxHealt(BodyController.GetMaxHealt() +
                                                   (coeffHealthUpgrade * initialBodyData.life));

        BodyController.SetHealt(BodyController.GetHealt() +
                                                (coeffHealthUpgrade * initialBodyData.life));
        
        health = BodyController.GetHealt();


        m_Game.Ui.SetLifeUI(BodyController.GetMaxHealt(), BodyController.GetHealt());
    }

    public void UpgradeTower()
    {
        TowerData initialTowerData = TowerController.GetBaseData();



        TowerController.SetTowerRotationSpeed(TowerController.GetTowerRotationSpeed() +
                                                            (coeffTowerRotationSpeedUpgrade *
                                                             initialTowerData.rotationSpeed));
       
        towerRotationSpeed = TowerController.GetTowerRotationSpeed();
    }


    public void UpgradeGun()
    {

        GunData initialGunData = GunController.GetBaseData();

        GunController.SetBulletVelocity(GunController.GetBulletVelocity() +
                                                        (coeffBulletVelocityUpgrade * initialGunData.bulletVelocity));
       
        bulletVelocity = GunController.GetBulletVelocity();

        GunController.SetReloadTimeSpeed(GunController.GetReloadTimeSpeed() -
                                                         (reloadTimeSpeed * initialGunData.reloadTimeSecond));


        reloadTimeSpeed = GunController.GetReloadTimeSpeed();
    }

    #endregion


    public void ResetPlayerManager()
    {
        DestroyPlayer();
        CreatePlayer();
        m_Game.Shop.ResetShopManager();
        m_Game.Ui.ResetShopUIManager();
        SetCameraFollowPlayer();
        SetUiLifeOfPlayer();
        m_Game.Ui.HideDeadMenu();
    }
}