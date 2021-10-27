using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Security.Cryptography;
using JetBrains.Annotations;

[System.Serializable]
public class BaseMap
{
    [Header("Base map Options")]
    public Transform parent;
    public GameObject wallPrefab;
    public GameObject groundPrefab;
}
[System.Serializable]
public class NoiseOptions
{
    [Header("Noise Map options")]
    public int mapWidth;
    public int mapHeight;
    public float noiseScale;
}

public class MapGeneratorManager : MonoBehaviour
{
    

    [System.Serializable]
    public struct Obstacle
    {
        public GameObject obstaclePrefab;
        [Range(0,1)]
        public float conditionOfSpawnMin;
        [Range(0,1)]
        public float conditionOfSpawnMax;
        [Range(0,100)]
        public int dropRate;

    }
    
    [System.Serializable]
    public struct Chunk
    {
        public GameObject chunkPrefab;
        [Range(0,1)]
        public float conditionOfSpawnMin;
        [Range(0,1)]
        public float conditionOfSpawnMax;
        [Range(0,100)]
        public int dropRate;
        [Range(0,100)]
        public int radiusSpawn;


    }

    public NoiseOptions noiseOptions;
    public BaseMap baseMap;

    public GameObject testObj;

    const int margin = 2;
    
    
    [Header("List")]
    public List<Obstacle> obstacleList = new List<Obstacle>();
    public List<Chunk> chunkList = new List<Chunk>();

    
    private bool[,] m_mapUsed;
    private float[,] noiseMap;
    
   
    
    public void GenerateNoiseMap(Action callback)
    {
        noiseMap = Noise.GenerateNoiseMap(noiseOptions.mapWidth, noiseOptions.mapHeight, noiseOptions.noiseScale);
        callback();
    }

    public void Generate()
    {
        GenerateNoiseMap(() =>
        {
            GenerateAll(() =>
            {
                
            });


        });
        
        
    }
    

    IEnumerator  Start()
    {
        Generate();
        yield return new WaitForSeconds(0.1f);
        AstarPath.active.data.gridGraph.SetDimensions(noiseOptions.mapWidth,noiseOptions.mapHeight,1);
        AstarPath.active.Scan();
    }


    public void GenerateAll( Action callback)
    {
        GameObject ground = Instantiate(baseMap.groundPrefab, baseMap.parent.transform.position, baseMap.parent.transform.rotation, baseMap.parent);
        SpriteRenderer spriteGround = ground.GetComponent<SpriteRenderer>();
        spriteGround.size = new Vector2(noiseOptions.mapWidth,noiseOptions.mapHeight);
        ground.transform.localPosition = new Vector3(noiseOptions.mapWidth / 2, noiseOptions.mapHeight / 2, 0);
        
        GameObject wallLeft = Instantiate(baseMap.wallPrefab, baseMap.parent.transform.position, baseMap.parent.transform.rotation, baseMap.parent);
        SpriteRenderer spriteWallLeft = wallLeft.GetComponent<SpriteRenderer>();
        BoxCollider2D colliderWallLeft = wallLeft.GetComponent<BoxCollider2D>();

        spriteWallLeft.size = new Vector2(spriteWallLeft.size.x,noiseOptions.mapHeight+margin);
        colliderWallLeft.size = new Vector2(colliderWallLeft.size.x,noiseOptions.mapHeight+margin);
        wallLeft.transform.localPosition = new Vector3(noiseOptions.mapWidth / 2, 0,0);
        
        GameObject wallRight = Instantiate(baseMap.wallPrefab, baseMap.parent.transform.position, baseMap.parent.transform.rotation, baseMap.parent);
        SpriteRenderer spriteWallRight = wallRight.GetComponent<SpriteRenderer>();
        BoxCollider2D colliderWallRight = wallRight.GetComponent<BoxCollider2D>();
        
        spriteWallRight.size = new Vector2(spriteWallRight.size.x,noiseOptions.mapHeight+margin);
        colliderWallRight.size = new Vector2(colliderWallRight.size.x,noiseOptions.mapHeight+margin);
        wallRight.transform.localPosition = new Vector3(-1*(noiseOptions.mapWidth  / 2), 0,0);
        
        GameObject wallTop = Instantiate(baseMap.wallPrefab, baseMap.parent.transform.position, baseMap.parent.transform.rotation,  baseMap.parent);
        SpriteRenderer spriteWallTop = wallTop.GetComponent<SpriteRenderer>();
        BoxCollider2D colliderWallTop = wallTop.GetComponent<BoxCollider2D>();
        
        spriteWallTop.size = new Vector2(noiseOptions.mapWidth+margin,spriteWallTop.size.y);
        colliderWallTop.size = new Vector2(noiseOptions.mapWidth+margin,colliderWallTop.size.y);
        wallTop.transform.localPosition = new Vector3(0, (noiseOptions.mapHeight / 2),0);
        
        GameObject wallBottom = Instantiate(baseMap.wallPrefab, baseMap.parent.transform.position, baseMap.parent.transform.rotation, baseMap.parent);
        SpriteRenderer spriteWallBottom  = wallBottom.GetComponent<SpriteRenderer>();
        BoxCollider2D colliderWallBottom  = wallBottom.GetComponent<BoxCollider2D>();
        
        spriteWallBottom.size = new Vector2(noiseOptions.mapWidth+margin,spriteWallBottom.size.y);
        colliderWallBottom.size = new Vector2(noiseOptions.mapWidth+margin,colliderWallBottom.size.y);
        wallBottom.transform.localPosition = new Vector3(0, -1*(noiseOptions.mapHeight / 2),0);

        
        float[,] noiseMap = Noise.GenerateNoiseMap(noiseOptions.mapWidth, noiseOptions.mapHeight, noiseOptions.noiseScale);
        for (int y = 0; y < noiseOptions.mapHeight; y=y+2)
        {
            for (int x = 0; x < noiseOptions.mapWidth; x=x+2)
            {

                Dictionary<GameObject, int> dicoOfSpawn = new Dictionary<GameObject, int>();
   

                obstacleList.ForEach(obstacleStruct =>
                {
                    
                    if (noiseMap[x,y] >= obstacleStruct.conditionOfSpawnMin &&  noiseMap[x,y] <= obstacleStruct.conditionOfSpawnMax)
                    {
                        dicoOfSpawn.Add(obstacleStruct.obstaclePrefab,obstacleStruct.dropRate);
                    }
                    
                });

                if (dicoOfSpawn.Count > 0)
                {
                    GameObject? prefab = GetObstacleToSpawn(dicoOfSpawn );

                    if (prefab != null)
                    {
                        Vector3 obstaclePos = new Vector3(-noiseOptions.mapWidth / 2 + x, noiseOptions.mapHeight / 2 - y, 0);
                        GameObject obstacle = Instantiate(prefab, baseMap.parent.transform.position, baseMap.parent.transform.rotation, baseMap.parent);
                        obstacle.transform.localPosition = obstaclePos;
                    }
                    
                    
                }
                
               
             

 
                
            }
        }

        
        m_mapUsed = new Boolean[noiseOptions.mapWidth, noiseOptions.mapHeight];
        for (int y = 0; y < noiseOptions.mapHeight; y++)
        {
            for (int x = 0; x < noiseOptions.mapWidth; x++)
            {
                RaycastHit2D hit = Physics2D.Raycast( new Vector2(baseMap.parent.transform.position.x +(-noiseOptions.mapWidth / 2 + x), baseMap.parent.transform.position.y +(noiseOptions.mapHeight / 2 - y)), new Vector2(0.001f,-0.001f));
                if (hit != false && hit.distance <= 1)
                {
                    if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
                    {
                        m_mapUsed[x, y] = true;
                     
                     
                    }
                    else
                    {
                        m_mapUsed[x, y] = false;
                    }
                }
                else
                {
                    m_mapUsed[x, y] = false;
                }

            }
        }
        
        
        
        for (int i = 0; i < 100; i++)
        {
           
            
            
            int x = UnityEngine.Random.Range(5, noiseOptions.mapWidth-5);
            int y = UnityEngine.Random.Range(5, noiseOptions.mapHeight-5);
            // noiseMapBool[width, height]
            Vector3 positionAbsolute = new Vector3(this.baseMap.parent.transform.position.x - (x - noiseOptions.mapWidth / 2),
                this.baseMap.parent.transform.position.y - (y - noiseOptions.mapWidth / 2),
                baseMap.parent.transform.position.z);

            if (!IsObstacleExist(new Vector2(positionAbsolute.x,positionAbsolute.y), 20, "Obstacle"))
            { 
               
                Dictionary<GameObject, int> dicoOfSpawn = new Dictionary<GameObject, int>();
                chunkList.ForEach(C =>
                {
                    dicoOfSpawn.Add(C.chunkPrefab,C.dropRate);
                });
        
                GameObject? prefab = GetObstacleToSpawn(dicoOfSpawn);
 
                if (prefab != null)
                {
                    Instantiate(testObj,positionAbsolute,baseMap.parent.transform.rotation, baseMap.parent);
                    for (int k = 0; k < 10; k++)
                    {
                        for (int j = 0; j < 10; j++)
                        {
                            int indexX = x + k - 5;
                            int indexY = y + k - 5;
                            m_mapUsed[indexX,indexY] = true;
                        }
                    }
                }
               
              
            }
           

        }

        callback();

    }
    
 

    

    [CanBeNull]
    public GameObject GetObstacleToSpawn( Dictionary<GameObject,int> obstacles)
    {
        GameObject? obstacleChoosen = null;

        List<GameObject> gameObjectList = new List<GameObject>();
        
        foreach (KeyValuePair<GameObject, int> kvp in obstacles)
        {
            for (int i = 1; i <= kvp.Value; i++)
            {
                gameObjectList.Add(kvp.Key);
            }
        }
   
        if (gameObjectList.Count < 100)
        {
            int lackingList = 100 - gameObjectList.Count;

            for (int i = 0; i < lackingList; i++)
            {
                gameObjectList.Add(null);
            }
        }
        int num = UnityEngine.Random.Range(0, gameObjectList.Count);
        obstacleChoosen = gameObjectList[num]??null;
        
        return obstacleChoosen;
    }
    
    
    public bool IsObstacleExist(Vector2 center, float radius, string layerName)
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(center, radius);

        if (hitColliders != null)
        {
            foreach (Collider2D hitCollider in hitColliders)
            {
                if (hitCollider.gameObject.layer == LayerMask.NameToLayer(layerName))
                {
                    return true;
                }
                
            }

            return false;

        }
        else
        {
            return true;
        }
          
        return true;
        
    }
    
    
    
    
}
