using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pathfinding;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
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
        
        if (GUILayout.Button("Clear Spawners"))
        {
            waveManager.ClearSpawners();
        }
        
        if (GUILayout.Button("Reset All"))
        {
            waveManager.ResetWaveManager();
        }
            
    }
}
#endif

[RequireComponent(typeof(EnemyManager))]
public class WaveManager : MonoBehaviour
{
    

    [Range(1,300)]
    public int timeBetweenWaves;

    private int initialTimeBetweenWaves;
    [Range(1,20)]
    public int waveDifficulty = 1;

    private int maxWaveDifficulty;
    
    public float timeBeforeNextWave =0;
    public bool toggleSpawn;
    
    private GameManager m_game;
    private bool isSpawned = false;
    private bool isNextWave = false;
    private int actualWave = 0;
   

    private void Awake()
    {
        m_game = GameManager.Instance;
        initialTimeBetweenWaves = timeBetweenWaves;
        maxWaveDifficulty = m_game.Enemys.enemyList.OrderByDescending(e=>e.difficultyLevel).FirstOrDefault().difficultyLevel;
    }

  

    private void Update()
    {
        
        if (!isSpawned)
        {
            isNextWave = false;
            isSpawned = true;
            SpawnWave(waveDifficulty);
            StartCoroutine(TimerWave(timeBetweenWaves));
            
        }

        if (isNextWave)
        {
           
            isNextWave = false;
            isSpawned = true;
            SpawnWave(waveDifficulty);
        }
   
    }

    public void NextWave()
    {
        isNextWave = true;

    }

    IEnumerator TimerWave(float timeStanding)
    {
        
        yield return new WaitForSeconds(1);
      
        if (timeStanding <= 0)
        {
            isSpawned = false;
        }
        else
        {

            if ( m_game.TimeManager.timeScale > 0)
            {
                float valueToRemove = timeStanding - m_game.TimeManager.timeScale;
                
                
                StartCoroutine(TimerWave(valueToRemove));
            }
            else
            {
                StartCoroutine(TimerWave(timeStanding));
            }
            
        }

        timeBeforeNextWave = timeStanding;
    }


    public void ClearEnemies()
    {
        foreach (Transform child in m_game.Enemys.parentContainer.transform)
            Destroy(child.gameObject);

    }

    public void ClearSpawners()
    {
        foreach (Transform child in m_game.Map.spawnOptions.parentContainer.transform)
            Destroy(child.gameObject);
    }


    public void ResetWaveManager()
    {
        StopAllCoroutines();
        ClearSpawners();
        ClearEnemies();
        actualWave = 0;
        waveDifficulty = 1;
        timeBetweenWaves = initialTimeBetweenWaves;
        StartCoroutine(TimerWave(timeBetweenWaves));
        m_game.Ui.SetWaveUI(actualWave);
    }

    

    public void SpawnWave(int difficultyLevel)
    {

        if (toggleSpawn)
            return;
        
        if ((actualWave + 1) % 5 == 0)
        {

            if (waveDifficulty < maxWaveDifficulty)
            {
                waveDifficulty++;
            }

            if (timeBetweenWaves > 15)
            {
                timeBetweenWaves--;
            }
            
            
        }
        
        if ((actualWave + 1)  % 10 == 0)
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
            
            actualWave++;
            m_game.Ui.SetWaveUI(actualWave);
        }
        else
        {
            m_game.Map.GenerateSpawners(3);
            SpawnWave(waveDifficulty);
            Debug.LogWarning("Aucun Spawner trouvé");
        }

     
    }
    
}
