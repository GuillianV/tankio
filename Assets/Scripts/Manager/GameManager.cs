using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update

    public static GameManager Instance;

    public bool gameIsPaused;
    public bool shopIsOpen;
    public bool isDead;

    public PlayerManager Player { get; private set; }
    public CameraManager Camera { get; private set; }
    public EnemyManager Enemys { get; private set; }
    public ProjectileManager Projectile { get; private set; }
    public WaveManager Waves { get; private set; }
    public MapGeneratorManager Map { get; private set; }

    public UIManager Ui { get; private set; }

    public ShopManager Shop { get; private set; }

    public HelperManager Helper { get; private set; }

    public AudioManager Audio { get; private set; }

    public TimeManager TimeManager { get; private set; }

    #region UnityEvent

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        Player = GetComponent<PlayerManager>();
        Camera = GetComponent<CameraManager>();
        Enemys = GetComponent<EnemyManager>();
        Projectile = GetComponent<ProjectileManager>();
        Waves = GetComponent<WaveManager>();
        Map = GetComponent<MapGeneratorManager>();
        Ui = GetComponent<UIManager>();
        Shop = GetComponent<ShopManager>();
        Audio = GetComponent<AudioManager>();
        TimeManager = GetComponent<TimeManager>();
        Helper = GetComponent<HelperManager>();
        QualitySettings.SetQualityLevel(0, false);
        // QualitySettings.renderPipeline = QualitySettings.GetQualityLevel().
            
    }
    
          
 
    void Start()
    {
        Player.InsatanciatePlayer();
        //Active le joueur
        Player.EnablePlayer();
        //Permet a la camera de suivre le joueur
        Camera.SetGameObjectToFollow(Player.player);

        Audio.StartPlaylist();


    }

    
    
    private void Update()
    {
    

        //20 t
        
        //Pause game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            OpenShop();
        }
        
        
  
    }

    #endregion


    //Pause whole game
    public void PauseGame()
    {
        if (isDead)
            return;

        gameIsPaused = !gameIsPaused;

        if (gameIsPaused)
        {
            Ui.ShowPausedMenu();
            Shop.HideShopMenu();
            TimeManager.DisableTime();
            shopIsOpen = false;
            gameIsPaused = true;
        }
        else
        {
            Ui.HidePausedMenu();
            Shop.HideShopMenu();
            TimeManager.EnableTime();
            shopIsOpen = false;
            gameIsPaused = false;
        }
    }


    public void OpenShop()
    {
        if (isDead)
            return;

        shopIsOpen = !shopIsOpen;

        if (shopIsOpen)
        {
            Ui.HidePausedMenu();
            Shop.ShowShopMenu();
            TimeManager.DisableTime();
            shopIsOpen = true;
            gameIsPaused = false;
        }
        else
        {
            Shop.HideShopMenu();
            Ui.HidePausedMenu();
            TimeManager.EnableTime();
            shopIsOpen = false;
            gameIsPaused = false;
        }
    }

    public void RestartGame()
    {


        Projectile.ResetProjectileManager();
        Waves.ResetWaveManager();
        Player.ResetPlayerManager();
    }

    //GameManagerMobile

#if UNITY_ANDROID


    public void ShopMobile()
    {
        if (isDead)
            return;

        shopIsOpen = true;
        gameIsPaused = false;
        TimeManager.DisableTime();
        Shop.ShowShopMenu();
        Ui.HideInGameMenu();
        Ui.HidePausedMenu();
    }

    public void PauseMobile()
    {
        if (isDead)
            return;

        shopIsOpen = false;
        gameIsPaused = true; 
        TimeManager.DisableTime();
        Shop.HideShopMenu();
        Ui.HideInGameMenu();
        Ui.ShowPausedMenu();
    }

    public void CloseMobile()
    {
        if (isDead)
            return;

        shopIsOpen = false;
        gameIsPaused = false;
        TimeManager.EnableTime();
        Shop.HideShopMenu();
        Ui.ShowInGameMenu();
        Ui.HidePausedMenu();
    }
#endif
}