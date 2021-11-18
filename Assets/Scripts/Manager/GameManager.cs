using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update

    public static GameManager Instance;

    public static bool gameIsPaused;
    
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
        
        
    }
    
    //Pause whole game
    void PauseGame ()
    {
        if(gameIsPaused)
        {
            Time.timeScale = 0f;
        }
        else 
        {
            Time.timeScale = 1;
        }
    }
}
