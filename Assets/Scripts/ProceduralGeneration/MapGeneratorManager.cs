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
  
        [Range(0,100)]
        public int dropRate;
        [Range(0,100)]
        public int radiusOfChunk;
        [Range(-360,360)]
        public int chunkRotation;


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
    
   
    
  
 
    private void Awake()
    {
        m_mapUsed = new Boolean[noiseOptions.mapWidth, noiseOptions.mapHeight];
        noiseMap = Noise.GenerateNoiseMap(noiseOptions.mapWidth, noiseOptions.mapHeight, noiseOptions.noiseScale);
    }

    IEnumerator  Start()
    {
        GenerateAll(() =>
        {
            
        });
        yield return new WaitForSeconds(0.1f);
        GenerateChunks(5,1000,"Obstacle");
        yield return new WaitForSeconds(0.1f);
        AstarPath.active.data.gridGraph.SetDimensions(noiseOptions.mapWidth,noiseOptions.mapHeight,1);
        AstarPath.active.Scan();
    }


    public void GenerateAll( Action callback)
    {
        GameObject ground = Instantiate(baseMap.groundPrefab, baseMap.parent.transform.position, baseMap.parent.transform.rotation, baseMap.parent);
        SpriteRenderer spriteGround = ground.GetComponent<SpriteRenderer>();
        spriteGround.size = new Vector2(noiseOptions.mapWidth,noiseOptions.mapHeight);
        ground.transform.localPosition = new Vector3(baseMap.parent.transform.position.x, baseMap.parent.transform.position.y, 0);
        
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
                        GameObject obstacle = Instantiate(prefab, baseMap.parent.transform.position,prefab.transform.rotation, baseMap.parent);
                        obstacle.transform.localPosition = obstaclePos;
                    }
                    
                    
                }
                
               
             

 
                
            }
        }

        
        
        
        
        
     

        callback();

    }



    public void GenerateChunks(int securityArea, int tryNumber , string layerNameToAvoid)
    {
        

        //Position Min et Max x de la carte
        int minRangWidth = - noiseOptions.mapWidth / 2 + securityArea;
        int maxRangWidth = noiseOptions.mapWidth / 2 - securityArea;

        //Position Min et Max y de la carte
        int minRangHeight = -noiseOptions.mapHeight / 2 + securityArea;
        int maxRangHeight = noiseOptions.mapHeight / 2 - securityArea;
        
           for (int i = 0; i < Math.Abs(tryNumber); i++)
        {
            //Position choisi par l'ordi
            int x = UnityEngine.Random.Range(minRangWidth,maxRangWidth);
            int y = UnityEngine.Random.Range(minRangHeight,maxRangHeight);
  
            //Check si la position choisi ne rentre pas en conflit avec un autre gameobject
            //On lui passe le centre le centre du raycast qui va etre generé et son radius
            
            List<Chunk> chunksList = new List<Chunk>();
            chunkList.ForEach(C =>
            {
                chunksList.Add(C);
            });
                
            Chunk? chunkReturned = GetChunkToSpawn(chunksList);

            if (chunkReturned != null)
            {
                Collider2D hitColliders = Physics2D.OverlapCircle(new Vector2(x,y),chunkReturned.Value.radiusOfChunk );
            
                //Si une collision n'a pas été detecté avec un obstacle, on instancie le prefab
                if (hitColliders == null || hitColliders.gameObject.layer != LayerMask.NameToLayer(layerNameToAvoid))
                {
            
                    if (chunkReturned.Value.chunkPrefab != null)
                    {
                        
                        //Permet de verifier si il n'y a pas d'autre chunk qui ont été generé proche de celui-ci
                        int mapUsedCoordonateX = x + this.noiseOptions.mapWidth / 2;
                        int mapUsedCoordonateY = y + this.noiseOptions.mapHeight / 2;
                        if (m_mapUsed[mapUsedCoordonateX,mapUsedCoordonateY] == false)
                        {
                            GameObject chunkInstancied =  Instantiate(chunkReturned.Value.chunkPrefab,baseMap.parent.transform.position,chunkReturned.Value.chunkPrefab.transform.rotation  , baseMap.parent);
                            chunkInstancied.transform.localPosition = new Vector3(x, y, chunkReturned.Value.chunkPrefab.transform.position.z);
                            chunkInstancied.transform.localRotation = Quaternion.Euler(0, 0,
                                (chunkReturned.Value.chunkPrefab.transform.rotation.z +
                                 (float) chunkReturned.Value.chunkRotation));
                            
                            
                            for (int k = -chunkReturned.Value.radiusOfChunk; k < chunkReturned.Value.radiusOfChunk; k++)
                            {
                                for (int j = -chunkReturned.Value.radiusOfChunk; j < chunkReturned.Value.radiusOfChunk; j++)
                                {
                                    int indexX = x + k + this.noiseOptions.mapWidth/2;
                                    int indexY = y + j + this.noiseOptions.mapHeight/2;
                                    m_mapUsed[indexX,indexY] = true;
                                }
                            }
                        }
                    
                    }

                }

            }
            
           
        }
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
    
    //Choisi le chunk a spawner en fonction de leur drop rate, parmis la liste des chunks.
    [CanBeNull]
    public Chunk? GetChunkToSpawn( List<Chunk> chunks)
    {
        

        List<Chunk?> gameChunks = new List<Chunk?>();
        
        chunks.ForEach(chunk =>
        {
            for (int i = 1; i <= chunk.dropRate; i++)
            {
                gameChunks.Add(chunk);
            }
        });
        
           
        
   
        if (gameChunks.Count < 100)
        {
            int lackingList = 100 - gameChunks.Count;

            for (int i = 0; i < lackingList; i++)
            {
                gameChunks.Add(null);
            }
        }
        int num = UnityEngine.Random.Range(0, gameChunks.Count);
        return gameChunks[num]??null;
        
    }

    
    public bool IsObstacleExist(Vector2 center, float radius, string layerName)
    {
        Collider2D hitColliders = Physics2D.OverlapCircle(center, radius);

        if (hitColliders == null || hitColliders.gameObject.layer != LayerMask.NameToLayer(layerName))
        {
            return false;
        }
        else
        {
            return true;
        }
          
        
    }
    
    
    
    
}
