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
    
    public WaveManager Waves { get; private set; }
    public MapGeneratorManager Map { get; private set; }
    
    public UIManager Ui { get; private set; }
    
    public ShopManager Shop { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }else if (Instance != this)
        {
            Destroy(gameObject);
        }

        Player = GetComponent<PlayerManager>();
        Camera = GetComponent<CameraManager>();
        Enemys = GetComponent<EnemyManager>();
        Waves = GetComponent<WaveManager>();
        Map = GetComponent<MapGeneratorManager>();
        Ui = GetComponent<UIManager>();
        Shop = GetComponent<ShopManager>();
    }

    void Start()
    {
        
        Player.InsatanciatePlayer();
        //Active le joueur
        Player.EnablePlayer();
        //Permet a la camera de suivre le joueur
        Camera.SetGameObjectToFollow(Player.player);
        
        
    }
 
    
    private void Update()
    {
        //Pause game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameIsPaused = !gameIsPaused;
            PauseGame();
            
        }
        
        if (Input.GetKeyDown(KeyCode.I))
        {
            shopIsOpen = !shopIsOpen;
            OpenShop();
            
        }
        
        
        
    }
    
    //Pause whole game
    void PauseGame ()
    {

        if(gameIsPaused)
        {

            Ui.ShowPausedMenu();
            Ui.HideShopMenu();
            Time.timeScale = 0;
            shopIsOpen = false;
            gameIsPaused = true;

        }
        else 
        {
            
            
            Ui.HidePausedMenu();
            Ui.HideShopMenu();
            Time.timeScale = 1;
            shopIsOpen = false;
            gameIsPaused = false;
        }
        
    }
    
    void OpenShop()
    {
        
         
        if(shopIsOpen)
        {

            
            Ui.HidePausedMenu();
            Ui.ShowShopMenu();
            Time.timeScale = 0;
            shopIsOpen = true;
            gameIsPaused = false;
         
        }
        else 
        {
            
            Ui.HideShopMenu();
            Ui.HidePausedMenu();
            Time.timeScale = 1;
            shopIsOpen = false;
            gameIsPaused = false;
        }
        
    
    }
    
}
