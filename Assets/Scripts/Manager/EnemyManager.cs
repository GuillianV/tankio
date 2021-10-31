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

  

    public List<StatsController> enemiesInGame = new List<StatsController>();
    
    public void Destroy(object sender, EventArgs args)
    {
        Debug.Log("TankDestroyed");
        TankDestroyed tankDestroyed = sender as TankDestroyed;
        TankController tankController = tankDestroyed.GetComponent<TankController>();
        enemiesInGame.Remove(tankController.StatsController);
        OnTankDestroyed(tankController);
    }
    
    public void Created(object sender, EventArgs args)
    {
        Debug.Log("TankCreated");
        TankCreate tankCreate = sender as TankCreate;
        TankController tankController = tankCreate.GetComponent<TankController>();
        
        enemiesInGame.Add(tankController.StatsController);
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
        
        aiDestinationSetter.target = GameObject.FindWithTag("Player").transform;

        if (enemyPatern.tracksData != null)
        {
            tankController.TracksController.tracks.LoadData(enemyPatern.tracksData);
        }

        if (enemyPatern.bodyData != null)
        {
            tankController.BodyController.body.LoadData(enemyPatern.bodyData);
        }

        if (enemyPatern.towerData != null)
        {
            tankController.TowerController.tower.LoadData(enemyPatern.towerData);
        }

        if (enemyPatern.gunData != null)
        {
            tankController.GunController.gun.LoadData(enemyPatern.gunData);
        }

        tankController.BindSprite();
        tankController.BindStats();

        return enemy;
    }
  
}

