using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    //Parent container of enemies
    public Transform parentContainer;
    
    
    //Structure de l'enemy
    [System.Serializable]
    public struct Enemys {
        public GameObject enemyPrefab;
        public Vector2 enemyPosition;
        
        [Header("Player Stats")]
        public TracksData tracksData;
        public BodyData bodyData;
        public TowerData towerData;
        public GunData gunData;
        [HideInInspector]
        public GameObject enemy;
    }
 
    public List<Enemys>  enemyList;
  
    

    //Active les enemies 
    public void EnableEnemys()
    {
        enemyList.ForEach(E =>
        {
            E.enemy.SetActive(true);
        });
        
    }
    
    //Desactive les enemies
    public void DisableEnemys()
    {
        enemyList.ForEach(E =>
        {
            E.enemy.SetActive(false);
        });

    }



    //Crée les enemies listés
    public void InsatanciateEnemys()
    {
       
        enemyList.ForEach(E =>
        {
            GameObject enemy = Instantiate(E.enemyPrefab, new Vector3(E.enemyPosition.x,E.enemyPosition.y,0), new Quaternion(0,0,0,0),parentContainer) as GameObject;
            E.enemy = enemy;
            TankController tankController = enemy.GetComponent<TankController>();
            AIDestinationSetter aiDestinationSetter = enemy.GetComponent<AIDestinationSetter>();
            aiDestinationSetter.target = GameObject.FindWithTag("Player").transform;
            
            if (E.tracksData != null)
            {
                tankController.TracksController.tracks.LoadData(E.tracksData);
            }
            if (E.bodyData != null)
            {
                tankController.BodyController.body.LoadData(E.bodyData);
            }
            if (E.towerData != null)
            {
                tankController.TowerController.tower.LoadData(E.towerData);
            }
            if (E.gunData != null)
            {
                tankController.GunController.gun.LoadData(E.gunData);
            }
            
            tankController.BindSprite();
            tankController.BindStats();
            
          
        });

        StartCoroutine(SpawnWave());

    }


    IEnumerator SpawnWave()
    {
        yield return new WaitForSeconds(5f);

        GameObject[] listSpawners = GameObject.FindGameObjectsWithTag("Spawners");
        foreach (GameObject spawner in listSpawners)
        {
            enemyList.ForEach(E =>
            {
                GameObject enemy = Instantiate(E.enemyPrefab, spawner.transform.position, new Quaternion(0,0,0,0),spawner.transform) as GameObject;
                E.enemy = enemy;
                TankController tankController = enemy.GetComponent<TankController>();
                AIDestinationSetter aiDestinationSetter = enemy.GetComponent<AIDestinationSetter>();
                aiDestinationSetter.target = GameObject.FindWithTag("Player").transform;
            
                if (E.tracksData != null)
                {
                    tankController.TracksController.tracks.LoadData(E.tracksData);
                }
                if (E.bodyData != null)
                {
                    tankController.BodyController.body.LoadData(E.bodyData);
                }
                if (E.towerData != null)
                {
                    tankController.TowerController.tower.LoadData(E.towerData);
                }
                if (E.gunData != null)
                {
                    tankController.GunController.gun.LoadData(E.gunData);
                }
            
                tankController.BindSprite();
                tankController.BindStats();
            
          
            });
        }

    }

}
