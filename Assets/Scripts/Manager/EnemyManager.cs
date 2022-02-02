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

        TankDestroyed tankDestroyed = sender as TankDestroyed;

        if (tankDestroyed.gameObject)
        {
            TankController tankController = tankDestroyed.GetComponent<TankController>();
            BodyController bodyController = tankController.GetTankComponent<BodyController>();
            if (!String.IsNullOrEmpty(args.Tag))
            {
                m_Game.Shop.AddGolds(bodyController.GetGold());
                m_Game.Audio.Play("tank-death-1");
            }
            enemiesInGame.Remove(tankController);
            enemiesInGameGO.Remove(tankDestroyed.gameObject);
            OnTankDestroyed(tankController);
        }
        
     
       
        
    }
    
    public void Created(object sender, EventArgs args)
    {
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

        List<ScriptableObject> enemyData = new List<ScriptableObject>();
        enemyData.Add(enemyPatern.bodyData);
        enemyData.Add(enemyPatern.gunData);
        enemyData.Add(enemyPatern.towerData);
        enemyData.Add(enemyPatern.tracksData);

        tankController.BindTank(enemyData);
        

        return enemy;
    }
  
}

