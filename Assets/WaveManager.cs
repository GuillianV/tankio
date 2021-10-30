using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(WaveManager))]
class SpawnWave : Editor {
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        
        
        WaveManager enemyManager = (WaveManager) target;
        
        if (GUILayout.Button("Spawn Wave"))
        {
            enemyManager.SpawnWave(enemyManager.waveDifficulty);
        }
            
    }
}


[RequireComponent(typeof(EnemyManager))]
public class WaveManager : MonoBehaviour
{
    
    
    [Range(1,20)]
    public int waveDifficulty = 1;

    private GameManager m_game;

    private void Awake()
    {
        m_game = GameManager.Instance;
    }

    public void SpawnWave(int difficultyLevel)
    {

        List<Enemy> enemys = m_game.Enemys.GetEnemies(difficultyLevel);
        
        GameObject[] listSpawners = GameObject.FindGameObjectsWithTag("Spawners");
        if (listSpawners.Length > 0)
        {
            foreach (GameObject spawner in listSpawners)
            {
             

                if (enemys.Count > 0)
                {
                    var random = new System.Random();
                    int index = random.Next(enemys.Count);
                    Enemy E = enemys[index];

                    GameObject enemy = Instantiate(E.enemyPrefab, spawner.transform.position, new Quaternion(0, 0, 0, 0), spawner.transform) as GameObject;
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
                }  else
                {
                    Debug.LogWarning("Aucun Enemy de la difficulté : "+difficultyLevel+" trouvé !");
                }
                
                
            };
        }
        else
        {
            Debug.LogWarning("Aucun Spawner trouvé");
        }
      
    }
    
}
