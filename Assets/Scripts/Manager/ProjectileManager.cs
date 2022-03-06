using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{

    public GameObject entityContainer;

    public List<GameObject> entityList = new List<GameObject>();

    private GameManager m_Game;
    private bool isRestarting = false;

    private void Awake()
    {
        m_Game = GameManager.Instance;
      
    }

    private void Start()
    {
        // foreach (Transform child in entityContainer.transform)
        // {
        //     TankAI tankComponent = child.gameObject.GetComponent<TankAI>();
        //     if (tankComponent!=null)
        //     {
        //      
        //     }
        // }
      
    }

    public void LoadPlayerShooter()
    {
        #if UNITY_ANDROID
        GunManager playerShoot = m_Game.Player.player.GetComponent<GunManager>();
#else
        GunManager playerShoot = m_Game.Player.player.GetComponent<GunManager>();
#endif
        playerShoot.BulletCreated += BulletCreatedHandler;
        playerShoot.BulletDestroyed += BulletDestroyedHandler;
    }

    public void LoadEnemiesShooter(GameObject gameObject)
    {

        GunManager enemyShoot = gameObject.GetComponent<GunManager>();
            enemyShoot.BulletCreated += BulletCreatedHandler;
            enemyShoot.BulletDestroyed += BulletDestroyedHandler;
     
    }

    
    public void BulletCreatedHandler(object _projectile,ProjectileEvent _projectileEvent)
    {
        if(!isRestarting)
            entityList.Add(_projectileEvent.Projectile);
    }
    
    public void BulletDestroyedHandler(object _projectile,ProjectileEvent _projectileEvent)
    {
        if (!isRestarting)
            entityList.Remove(_projectileEvent.Projectile);

        
       
    }

    public void ResetProjectileManager()
    {
        
        isRestarting = true;
        entityList.ForEach(P =>
        {
            if (P != null)
            { 
               
                Destroy(P);
              
                
            }
            
           
        });
        isRestarting = false;
    }
    
    

}
