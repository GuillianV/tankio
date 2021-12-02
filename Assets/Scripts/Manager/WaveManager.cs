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

        
        GameManager m_game = GameManager.Instance;
        WaveManager waveManager = (WaveManager) target;
        
        if (GUILayout.Button("Spawn Next Wave"))
        {
            waveManager.NextWave();
        }
        
        if (GUILayout.Button("Add Spawner"))
        {
            m_game.Map.GenerateSpawners(1);
        }
        
        if (GUILayout.Button("Clear Enemies"))
        {
            waveManager.ClearEnemies();
        }
            
    }
}


[RequireComponent(typeof(EnemyManager))]
public class WaveManager : MonoBehaviour
{
    

    [Range(1,300)]
    public int timeBetweenWaves = 30;
    [Range(1,20)]
    public int waveDifficulty = 1;

    private GameManager m_game;
    private bool isSpawned = false;
    private bool isNextWave = false;
    private int actualWave = 1;
   

    private void Awake()
    {
        m_game = GameManager.Instance;
    }

    private void Start()
    {
        SpawnWave(waveDifficulty);
    }

    private void Update()
    {
        
        if (!isSpawned)
        {
            isNextWave = false;
            isSpawned = true;
            SpawnWave(waveDifficulty);
            StartCoroutine(TimerWave());

        }

        if (isNextWave)
        {
           
            isNextWave = false;
            isSpawned = true;
            StopCoroutine(TimerWave());
            SpawnWave(waveDifficulty);
        }
   
    }

    public void NextWave()
    {
        isNextWave = true;

    }

    IEnumerator TimerWave()
    {
        
        yield return new WaitForSeconds(timeBetweenWaves);
        isSpawned = false;

    }


    public void ClearEnemies()
    {
        foreach (Transform child in m_game.Enemys.parentContainer.transform)
            Destroy(child.gameObject);

    }



    

    public void SpawnWave(int difficultyLevel)
    {
        if (actualWave % 5 == 0)
        {
            waveDifficulty++;
            timeBetweenWaves--;
        }
        
        if (actualWave % 10 == 0)
        {
            m_game.Map.GenerateSpawners(1);
        }

      
        
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

                    m_game.Enemys.InstanciateEnemy(E, spawner.transform.position);


                }  else
                {
                 
                    Debug.LogWarning("Aucun Enemy de la difficulté : "+difficultyLevel+" trouvé !");
                }
                
                
            };
        }
        else
        {
            m_game.Map.GenerateSpawners(1);
            SpawnWave(waveDifficulty);
            Debug.LogWarning("Aucun Spawner trouvé");
        }

        actualWave++;
    }
    
}
