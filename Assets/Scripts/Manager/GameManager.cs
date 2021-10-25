using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update

    public static GameManager Instance;
   
    public PlayerManager Player { get; private set; }
    public CameraManager Camera { get; private set; }
    
    public EnemyManager Enemys { get; private set; }
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
    }

    void Start()
    {
        Player.InsatanciatePlayer();
        Player.EnablePlayer();
        Camera.SetGameObjectToFollow(Player.player);
        Enemys.InsatanciateEnemys();
        Enemys.EnableEnemys();
        
    }
    

}
