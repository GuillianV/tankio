using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Pathfinding;
using UnityEditor;
using UnityEngine;


public class EnemyManager : MonoBehaviour
{
 

    //Parent container of enemies
    public Transform parentContainer;
    public event EventHandler<TankEvent> TankDestroyed;
    public event EventHandler<TankEvent> TankCreated;

    public List<Enemy> enemyList;

  

    public List<TankController> enemiesInGame = new List<TankController>();
    public List<GameObject> enemiesInGameGO = new List<GameObject>();
    private GameManager m_Game;

    public void Awake()
    {
        m_Game = GameManager.Instance;
    }

    public void Destroy(object sender, TagEvent args)
    {

        Debug.Log("TankDestroyed");
        TankDestroyed tankDestroyed = sender as TankDestroyed;

        if (tankDestroyed.gameObject)
        {
            TankController tankController = tankDestroyed.GetComponent<TankController>();
            if (!String.IsNullOrEmpty(args.Tag))
            {
                m_Game.Shop.AddGolds(tankController.BodyController.GetGold());
                m_Game.Audio.Play("tank-death-1");
            }
            enemiesInGame.Remove(tankController);
            enemiesInGameGO.Remove(tankDestroyed.gameObject);
            OnTankDestroyed(tankController);
        }
        
     
       
        
    }
    
    public void Created(object sender, EventArgs args)
    {
        Debug.Log("TankCreated");
        TankCreate tankCreate = sender as TankCreate;
        TankController tankController = tankCreate.GetComponent<TankController>();
        enemiesInGame.Add(tankController);
        enemiesInGameGO.Add(tankCreate.gameObject);
        
        m_Game.Projectile.LoadEnemiesShooter(tankCreate.gameObject);
        OnTankCreated(tankController);
    }

    public void OnTankDestroyed(TankController tankController)
    {
        TankDestroyed?.Invoke(this,new TankEvent(tankController));
    }
    
    public void OnTankCreated(TankController tankController)
    {
        TankCreated?.Invoke(this,new TankEvent(tankController));
    }


    [CanBeNull]
    public List<Enemy> GetEnemies(int difficultyLevel)
    {
        return enemyList.Where(enemy => enemy.difficultyLevel == difficultyLevel).ToList();
    }

    public GameObject InstanciateEnemy(Enemy enemyPatern ,Vector3 spawnerPosition)
    {
        GameObject enemy = Instantiate(enemyPatern.enemyPrefab, spawnerPosition, new Quaternion(0, 0, 0, 0), parentContainer.transform) as GameObject;
        TankController tankController = enemy.GetComponent<TankController>();
        AIDestinationSetter aiDestinationSetter = enemy.GetComponent<AIDestinationSetter>();
        TankDestroyed tankDestroyed = enemy.GetComponent<TankDestroyed>();
        TankCreate tankCreate = enemy.GetComponent<TankCreate>();
        
        tankDestroyed.Destroyed += Destroy;
        tankCreate.Created += Created;

        if (GameObject.FindWithTag("Player") != null)
        {
            aiDestinationSetter.target = GameObject.FindWithTag("Player").transform;

        }

        List<ITankComponent> tankComponentsList = tankController.GetComponents<ITankComponent>().ToList();



        if (enemyPatern.tracksData != null)
        {
        
            ITankComponent tracksComponent = tankComponentsList.FirstOrDefault(component => component.ToString().Contains("TracksController"));
            tracksComponent.BindData(enemyPatern.tracksData);
            tracksComponent.BindStats();
        }

        if (enemyPatern.bodyData != null)
        {
            ITankComponent bodyComponent = tankComponentsList.FirstOrDefault(component => component.ToString().Contains("BodyController"));
            bodyComponent.BindData(enemyPatern.bodyData);
            bodyComponent.BindStats();
        }

        if (enemyPatern.towerData != null)
        {
            ITankComponent towerComponent = tankComponentsList.FirstOrDefault(component => component.ToString().Contains("TowerController"));
            towerComponent.BindData(enemyPatern.towerData);
            towerComponent.BindStats();
        }

        if (enemyPatern.gunData != null)
        {
            ITankComponent gunComponent = tankComponentsList.FirstOrDefault(component => component.ToString().Contains("GunController"));
            gunComponent.BindData(enemyPatern.gunData);
            gunComponent.BindStats();

        }

        tankController.BindSprite();
        tankController.BindStats();

        return enemy;
    }
  
}

