using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update

    public static GameManager Instance;

    public static bool gameIsPaused;
    public static bool shopIsOpen;

    public PlayerManager Player { get; private set; }
    public CameraManager Camera { get; private set; }
    public EnemyManager Enemys { get; private set; }
    public ProjectileManager Projectile { get; private set; }
    public WaveManager Waves { get; private set; }
    public MapGeneratorManager Map { get; private set; }

    public UIManager Ui { get; private set; }

    public ShopManager Shop { get; private set; }

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

        Audio.Play("BackgroundMusic");


#if UNITY_ANDROID
        StartMobile();

#endif
    }



    
    private void Update()
    {
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
        gameIsPaused = !gameIsPaused;

        if (gameIsPaused)
        {
            Ui.ShowPausedMenu();
            Ui.HideShopMenu();
            TimeManager.timeScale = 0;
            shopIsOpen = false;
            gameIsPaused = true;
        }
        else
        {
            Ui.HidePausedMenu();
            Ui.HideShopMenu();
            TimeManager.timeScale = 1;
            shopIsOpen = false;
            gameIsPaused = false;
        }
    }


    public void OpenShop()
    {
        shopIsOpen = !shopIsOpen;

        if (shopIsOpen)
        {
            Ui.HidePausedMenu();
            Ui.ShowShopMenu();
            TimeManager.timeScale = 0;
            shopIsOpen = true;
            gameIsPaused = false;
        }
        else
        {
            Ui.HideShopMenu();
            Ui.HidePausedMenu();
            TimeManager.timeScale = 1;
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


    public void StartMobile()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

    public void ShopMobile()
    {
        shopIsOpen = true;
        gameIsPaused = false;
        TimeManager.timeScale = 0;
        Ui.ShowShopMenu();
        Ui.HideInGameMenu();
        Ui.HidePausedMenu();
    }

    public void PauseMobile()
    {
        shopIsOpen = false;
        gameIsPaused = true;
        TimeManager.timeScale = 0;
        Ui.HideShopMenu();
        Ui.HideInGameMenu();
        Ui.ShowPausedMenu();
    }

    public void CloseMobile()
    {
        shopIsOpen = false;
        gameIsPaused = false;
        TimeManager.timeScale = 1;
        Ui.HideShopMenu();
        Ui.ShowInGameMenu();
        Ui.HidePausedMenu();
    }
#endif
}