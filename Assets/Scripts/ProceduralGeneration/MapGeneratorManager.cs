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
    [Header("Base map Options")] public Transform parent;
    public GameObject wallPrefab;
    public GameObject groundPrefab;
}

[System.Serializable]
public class NoiseOptions
{
    [Header("Noise Map options")] public int mapWidth;
    public int mapHeight;
    public float noiseScale;
}

public class MapGeneratorManager : MonoBehaviour
{
    [System.Serializable]
    public struct Obstacle
    {
        public GameObject obstaclePrefab;
        [Range(0, 1)] public float conditionOfSpawnMin;
        [Range(0, 1)] public float conditionOfSpawnMax;
        [Range(0, 100)] public int dropRate;

        public int priority;
        public int radius;
    }

    [System.Serializable]
    public struct Chunk
    {
        public GameObject chunkPrefab;

        [Range(0, 100)] public int dropRate;
        [Range(0, 100)] public int radiusOfChunk;
        [Range(-360, 360)] public int chunkRotation;
    }

    public NoiseOptions noiseOptions;
    public BaseMap baseMap;

    public GameObject testObj;

    const int margin = 2;


    [Header("List")] public List<Obstacle> obstacleListAll = new List<Obstacle>();
    public List<Chunk> chunkList = new List<Chunk>();


    private bool[,] m_mapUsed;
    private float[,] noiseMap;


    private void Awake()
    {
        noiseMap = Noise.GenerateNoiseMap(noiseOptions.mapWidth, noiseOptions.mapHeight, noiseOptions.noiseScale);
        m_mapUsed = new Boolean[noiseMap.GetLength(0), noiseMap.GetLength(1)];
        
        GenerateGround();

        GenerateWall(new Vector3(noiseOptions.mapWidth / 2, 0, 0), new Vector3(0, 0, 0));

        GenerateWall(new Vector3(-1 * (noiseOptions.mapWidth / 2), 0, 0), new Vector3(0, 0, 0));

        GenerateWall(new Vector3(0, (noiseOptions.mapHeight / 2), 0), new Vector3(0, 0, 90));
 
        GenerateWall(new Vector3(0, -1 * (noiseOptions.mapHeight / 2), 0), new Vector3(0, 0, 90));

        
        
       
       
        
    }

    IEnumerator Start()
    {
        GenerateListObstacles();
        yield return new WaitForSeconds(1f);
        GenerateChunks(5, 1000, "Obstacle");
        yield return new WaitForSeconds(0.1f);
        AstarPath.active.data.gridGraph.SetDimensions(noiseOptions.mapWidth,noiseOptions.mapHeight,1);
        AstarPath.active.Scan();
    }


    public void GenerateGround()
    {
        GameObject ground = Instantiate(baseMap.groundPrefab, baseMap.parent.transform.position,
            baseMap.parent.transform.rotation, baseMap.parent);
        SpriteRenderer spriteGround = ground.GetComponent<SpriteRenderer>();
        spriteGround.size = new Vector2(noiseOptions.mapWidth, noiseOptions.mapHeight);
        ground.transform.localPosition =
            new Vector3(baseMap.parent.transform.position.x, baseMap.parent.transform.position.y, 0);
    }

    public void GenerateWall(Vector3 position, Vector3 rotation)
    {
        GameObject wall = Instantiate(baseMap.wallPrefab, baseMap.parent.transform.position,
            baseMap.parent.transform.rotation, baseMap.parent);
        SpriteRenderer spriteWallLeft = wall.GetComponent<SpriteRenderer>();
        BoxCollider2D colliderWallLeft = wall.GetComponent<BoxCollider2D>();

        spriteWallLeft.size = new Vector2(spriteWallLeft.size.x, noiseOptions.mapHeight + margin);
        colliderWallLeft.size = new Vector2(colliderWallLeft.size.x, noiseOptions.mapHeight + margin);
        wall.transform.localPosition = position;
        wall.transform.localRotation = Quaternion.Euler(rotation);
    }


    public void GenerateListObstacles()
    {
        //Selection tous nous Obstacles
        if (obstacleListAll.Count > 0)
        {
            List<Obstacle> listObstaclesSorted = obstacleListAll.OrderBy(o => o.priority).ToList();


            Dictionary<int, List<Obstacle>> listsOfListsObstacles = new Dictionary<int, List<Obstacle>>();


            listObstaclesSorted.ForEach(so =>
            {
                if (!listsOfListsObstacles.ContainsKey(so.priority))
                {
                    listsOfListsObstacles.Add(so.priority, new List<Obstacle>());
                }
            });


            int localPriority = listObstaclesSorted.First().priority;

            int iterate = 1;
            listObstaclesSorted.ForEach(oneObstacle =>
            {
                //Ajoute a une liste tous les objets de meme priorité
                if (localPriority == oneObstacle.priority)
                {
                    listsOfListsObstacles[localPriority].Add(oneObstacle);
                    if (listObstaclesSorted.Count <= iterate)
                    {
                        GenerateObstacle(listsOfListsObstacles[localPriority]);
                        iterate = 1;
                    }
                }
                else
                {
                    if (listsOfListsObstacles[localPriority].Count > 0)
                    {
                        GenerateObstacle(listsOfListsObstacles[localPriority]);
                    }

                    localPriority = oneObstacle.priority;
                    listsOfListsObstacles[localPriority].Add(oneObstacle);
                    //Si l'obstacle possede une priorité differente, la gene des obstacles de la meme priorité est lancé
                    if (listObstaclesSorted.Count <= iterate)
                    {
                        GenerateObstacle(listsOfListsObstacles[localPriority]);
                        iterate = 1;
                    }

                    iterate = 1;
                }

                iterate++;
            });
        }
        
        
    }

    public void GenerateObstacle(List<Obstacle> obstacles)
    {
        for (int x = 0; x < noiseMap.GetLength(0); x += 3)
        {
            for (int y = 0; y < noiseMap.GetLength(1); y += 3)
            {
                List<Obstacle> obstacleListNoise = new List<Obstacle>();

                obstacles.ForEach(o =>
                {
                    if (noiseMap[x, y] >= o.conditionOfSpawnMin && noiseMap[x, y] <= o.conditionOfSpawnMax)
                    {
                        obstacleListNoise.Add(o);
                    }
                });

                if (obstacleListNoise.Count > 0)
                {
                    Obstacle? obstacleReturned = GetObstacleToSpawn(obstacleListNoise);

                    if (obstacleReturned != null)
                    {
                        if (m_mapUsed[x, y] == false)
                        {
                         
                            //Si une collision n'a pas été detecté avec un obstacle, on instancie le prefab
                            if (!IsObstacleExist(new Vector2(x, y), obstacleReturned.Value.radius,
                                "Obstacle"))
                            {


                              
                                    GameObject obstacle = Instantiate(obstacleReturned.Value.obstaclePrefab,
                                        baseMap.parent.transform.position,
                                        obstacleReturned.Value.obstaclePrefab.transform.rotation, baseMap.parent);
                                    obstacle.transform.localPosition = new Vector3(x - noiseMap.GetLength(0) / 2,
                                        y - noiseMap.GetLength(1) / 2,
                                        obstacleReturned.Value.obstaclePrefab.transform.position.z);

                                    for (int k = -obstacleReturned.Value.radius; k < obstacleReturned.Value.radius; k++)
                                    {
                                        for (int j = -obstacleReturned.Value.radius;
                                            j < obstacleReturned.Value.radius;
                                            j++)
                                        {
                                            int indexX = (x + this.noiseOptions.mapWidth / 2) + k  ;
                                            int indexY = (y + this.noiseOptions.mapHeight / 2) + j  ;
                                            if (indexX <= 0 || indexX >= m_mapUsed.GetLength(0) || indexY <= 0 ||
                                                indexY >= m_mapUsed.GetLength(1))
                                            {
                                            }
                                            else
                                            {
                                                m_mapUsed[indexX, indexY] = true;
                                            }
                                        }
                                    }
                              
                            }
                        }
                    }
                }
            }
        }
    }


    public void GenerateChunks(int securityArea, int tryNumber, string layerNameToAvoid)
    {
        for (int i = 0; i < Math.Abs(tryNumber); i++)
        {
            //Position choisi par l'ordi
            int x = UnityEngine.Random.Range(0, noiseOptions.mapWidth);
            int y = UnityEngine.Random.Range(0, noiseOptions.mapHeight);

            //Check si la position choisi ne rentre pas en conflit avec un autre gameobject
            //On lui passe le centre le centre du raycast qui va etre generé et son radius

            List<Chunk> chunksList = new List<Chunk>();
            chunkList.ForEach(C => { chunksList.Add(C); });

            Chunk? chunkReturned = GetChunkToSpawn(chunksList);

            if (chunkReturned != null)
            {
                if (chunkReturned.Value.chunkPrefab != null)
                {
                    //Si une collision n'a pas été detecté avec un obstacle, on instancie le prefab
                    if (!IsObstacleExist(new Vector2(x, y), chunkReturned.Value.radiusOfChunk, layerNameToAvoid))
                    {
                        //Permet de verifier si il n'y a pas d'autre chunk qui ont été generé proche de celui-ci
                        if (m_mapUsed[x, y] == false)
                        {
                            GameObject chunkInstancied = Instantiate(chunkReturned.Value.chunkPrefab,
                                baseMap.parent.transform.position, chunkReturned.Value.chunkPrefab.transform.rotation,
                                baseMap.parent);
                            chunkInstancied.transform.localPosition = new Vector3(x - noiseOptions.mapWidth / 2,
                                y - noiseOptions.mapHeight / 2, chunkReturned.Value.chunkPrefab.transform.position.z);
                            chunkInstancied.transform.localRotation = Quaternion.Euler(0, 0,
                                (chunkReturned.Value.chunkPrefab.transform.rotation.z +
                                 (float) chunkReturned.Value.chunkRotation));


                            for (int k = -chunkReturned.Value.radiusOfChunk; k < chunkReturned.Value.radiusOfChunk; k++)
                            {
                                for (int j = -chunkReturned.Value.radiusOfChunk;
                                    j < chunkReturned.Value.radiusOfChunk;
                                    j++)
                                {
                                    int indexX = x + k ;
                                    int indexY = y + j ;
                                    if (indexX <= 0 || indexX >= m_mapUsed.GetLength(0)  ||
                                        indexY  <= 0 || indexY >= m_mapUsed.GetLength(1))
                                    {
                                    }
                                    else
                                    {
                                        m_mapUsed[indexX, indexY] = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }


    //Choisi le chunk a spawner en fonction de leur drop rate, parmis la liste des chunks.
    [CanBeNull]
    public Obstacle? GetObstacleToSpawn(List<Obstacle> chunks)
    {
        List<Obstacle?> gameObstacles = new List<Obstacle?>();

        chunks.ForEach(obstacle =>
        {
            for (int i = 1; i <= obstacle.dropRate; i++)
            {
                gameObstacles.Add(obstacle);
            }
        });


        if (gameObstacles.Count < 100)
        {
            int lackingList = 100 - gameObstacles.Count;

            for (int i = 0; i < lackingList; i++)
            {
                gameObstacles.Add(null);
            }
        }

        int num = UnityEngine.Random.Range(0, gameObstacles.Count);
        return gameObstacles[num] ?? null;
    }

    //Choisi le chunk a spawner en fonction de leur drop rate, parmis la liste des chunks.
    [CanBeNull]
    public Chunk? GetChunkToSpawn(List<Chunk> chunks)
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
        return gameChunks[num] ?? null;
    }


    public bool IsObstacleExist(Vector2 center, float radius, string layerName)
    {
        bool collided = false;

        Collider2D[] hitColliders =
            Physics2D.OverlapCircleAll(
                new Vector2(center.x - noiseMap.GetLength(0) / 2, center.y - noiseMap.GetLength(1) / 2), radius);

        if (hitColliders.Length > 0)
        {
            foreach (Collider2D col in hitColliders)
            {
                if (col.gameObject.layer ==
                    LayerMask.NameToLayer(layerName))
                {
                    collided = true;
                }
            }
        }


        return collided;
    }
}