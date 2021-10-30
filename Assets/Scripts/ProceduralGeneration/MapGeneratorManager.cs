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
    [Header("Noise Map options")] 
    [Range(20,400)]
    public int mapWidth;
    [Range(20,400)]
    public int mapHeight;
    [Range(0.1f,400)]
    public float noiseScale;
}

[System.Serializable]
public class SpawnOptions
{
    [Header("Spawn enemies option")] [Range(0, 50)]
    public int radius;

    public Transform parentContainer;
    public string tag;
    public int numberSpawners;
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

        [Range(0, 3)] public int priority;
        [Range(0, 50)] public int radius;
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
    public SpawnOptions spawnOptions;

    
    [Header("Options")] public string layerToAvoid = "Obstacle";
    [Range(100, 1000)] public int iterateOfSpawn = 500;


    public List<Obstacle> obstacleListAll = new List<Obstacle>();
    public List<Chunk> chunkListAll = new List<Chunk>();


    private bool[,] m_mapUsed;
    private float[,] noiseMap;


    private void Awake()
    {
        //Genere la noismap
        noiseMap = Noise.GenerateNoiseMap(noiseOptions.mapWidth, noiseOptions.mapHeight, noiseOptions.noiseScale);
        //Genere la map used
        m_mapUsed = new Boolean[noiseMap.GetLength(0), noiseMap.GetLength(1)];
    }

    void Start()
    {
        StartCoroutine(GenerateMap());
    }

    IEnumerator GenerateMap()
    {
        //Genere le sol et les murs
        GenerateGround();
        GenerateWall(new Vector3(noiseOptions.mapWidth / 2, 0, 0), new Vector3(0, 0, 0));
        GenerateWall(new Vector3((-1 * (noiseOptions.mapWidth / 2)), 0, 0), new Vector3(0, 0, 0));
        GenerateWall(new Vector3(0, (noiseOptions.mapHeight / 2), 0), new Vector3(0, 0, 90));
        GenerateWall(new Vector3(0, (-1 * (noiseOptions.mapHeight / 2)), 0), new Vector3(0, 0, 90));
        GenerateListObstacles();
        yield return new WaitForSeconds(0.2f);
        //Genere les chunks
        GenerateChunks();
        yield return new WaitForSeconds(0.2f);
        GenerateSpawners();
        AstarPath.active.data.gridGraph.SetDimensions(noiseOptions.mapWidth, noiseOptions.mapHeight, 1);
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
        wall.transform.localPosition = new Vector3(position.x + baseMap.parent.position.x,
            position.y + baseMap.parent.position.y, position.z + baseMap.parent.position.z);
        wall.transform.localRotation = Quaternion.Euler(rotation);
        spriteWallLeft.size = new Vector2(spriteWallLeft.size.x, noiseOptions.mapHeight + 2);
        colliderWallLeft.size = new Vector2(colliderWallLeft.size.x, noiseOptions.mapHeight + 2);
    }


    public void GenerateListObstacles()
    {
        //Selection tous nous Obstacles
        if (obstacleListAll.Count > 0)
        {
            Dictionary<int, List<Obstacle>> listsOfListsObstacles = new Dictionary<int, List<Obstacle>>();

            //List toutes les priorités des obstacles
            List<int> listOfPriority = new List<int>();
            obstacleListAll.ForEach(so =>
            {
                if (!listsOfListsObstacles.Keys.Contains(so.priority))
                {
                    listOfPriority.Add(so.priority);
                    listsOfListsObstacles.Add(so.priority, new List<Obstacle>());
                }
            });

            listOfPriority.ForEach(priority =>
            {
                foreach (Obstacle obstacle in obstacleListAll.Where(o => o.priority == priority))
                {
                    listsOfListsObstacles[priority].Add(obstacle);
                }
            });

            listsOfListsObstacles = listsOfListsObstacles.OrderBy(pair => pair.Key)
                .ToDictionary(obj => obj.Key, obj => obj.Value);

            foreach (KeyValuePair<int, List<Obstacle>> keyValuePair in listsOfListsObstacles)
            {
                GenerateObstacle(keyValuePair.Value);
            }
        }
    }

    public void GenerateObstacle(List<Obstacle> obstacles)
    {
        for (int x = 0; x < noiseMap.GetLength(0); x += 1)
        {
            for (int y = 0; y < noiseMap.GetLength(1); y += 1)
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
                            if (!IsObstacleExist(new Vector2(x, y), obstacleReturned.Value.radius))
                            {
                                float posX = (x - (noiseMap.GetLength(0) / 2) + baseMap.parent.transform.position.x);
                                float poxY = (y - (noiseMap.GetLength(1) / 2) + baseMap.parent.transform.position.y);


                                GameObject obstacle = Instantiate(obstacleReturned.Value.obstaclePrefab,
                                    baseMap.parent.transform.position,
                                    obstacleReturned.Value.obstaclePrefab.transform.rotation, baseMap.parent);

                                obstacle.transform.localPosition =
                                    new Vector3(posX, poxY, baseMap.parent.transform.position.z);


                                for (int k = -obstacleReturned.Value.radius; k < obstacleReturned.Value.radius; k++)
                                {
                                    for (int j = -obstacleReturned.Value.radius; j < obstacleReturned.Value.radius; j++)
                                    {
                                        int indexX = x + k;
                                        int indexY = y + j;
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


    public void GenerateChunks()
    {
        for (int i = 0; i < Math.Abs(iterateOfSpawn); i++)
        {
            //Position choisi par l'ordi
            int x = UnityEngine.Random.Range(0, noiseOptions.mapWidth);
            int y = UnityEngine.Random.Range(0, noiseOptions.mapHeight);

            //Check si la position choisi ne rentre pas en conflit avec un autre gameobject
            //On lui passe le centre le centre du raycast qui va etre generé et son radius


            Chunk? chunkReturned = GetChunkToSpawn(chunkListAll);

            if (chunkReturned != null)
            {
                if (chunkReturned.Value.chunkPrefab != null)
                {
                    //Permet de verifier si il n'y a pas d'autre chunk qui ont été generé proche de celui-ci
                    if (m_mapUsed[x, y] == false)
                    {
                        //Si une collision n'a pas été detecté avec un obstacle, on instancie le prefab
                        if (!IsObstacleExist(new Vector2(x, y), chunkReturned.Value.radiusOfChunk))
                        {
                            float posX = (x - (noiseMap.GetLength(0) / 2) + baseMap.parent.transform.position.x);
                            float poxY = (y - (noiseMap.GetLength(1) / 2) + baseMap.parent.transform.position.y);


                            GameObject chunkInstancied = Instantiate(chunkReturned.Value.chunkPrefab,
                                baseMap.parent.transform.position,
                                chunkReturned.Value.chunkPrefab.transform.rotation, baseMap.parent);

                            chunkInstancied.transform.localPosition =
                                new Vector3(posX, poxY, baseMap.parent.transform.position.z);
                            chunkInstancied.transform.localRotation = Quaternion.Euler(0, 0,
                                baseMap.parent.transform.position.z + chunkReturned.Value.chunkRotation +
                                chunkReturned.Value.chunkPrefab.transform.rotation.z);


                            for (int k = -chunkReturned.Value.radiusOfChunk; k < chunkReturned.Value.radiusOfChunk; k++)
                            {
                                for (int j = -chunkReturned.Value.radiusOfChunk;
                                    j < chunkReturned.Value.radiusOfChunk;
                                    j++)
                                {
                                    int indexX = x + k;
                                    int indexY = y + j;
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


    public void GenerateSpawners()
    {

        int spawnerRestants = spawnOptions.numberSpawners;

        while (spawnerRestants > 0)
        {
            
            if (spawnerRestants <= 0)
            {
                return;
            }
            
            //Position choisi par l'ordi
            int x = UnityEngine.Random.Range(0, noiseOptions.mapWidth);
            int y = UnityEngine.Random.Range(0, noiseOptions.mapHeight);

        

            //Permet de verifier si il n'y a pas d'autre chunk qui ont été generé proche de celui-ci
            if (m_mapUsed[x, y] == false)
            {
                //Si une collision n'a pas été detecté avec un obstacle, on instancie le prefab
                if (!IsObstacleExist(new Vector2(x, y), spawnOptions.radius))
                {
                    float posX = (x - (noiseMap.GetLength(0) / 2) + baseMap.parent.transform.position.x);
                    float poxY = (y - (noiseMap.GetLength(1) / 2) + baseMap.parent.transform.position.y);

                    spawnerRestants--;

                    GameObject spawnerInstancied = Instantiate(new GameObject(), baseMap.parent.transform.position, new Quaternion(0,0,0,0), spawnOptions.parentContainer);
                    spawnerInstancied.gameObject.tag = spawnOptions.tag;
                    
                    spawnerInstancied.transform.localPosition =  new Vector3(posX, poxY, baseMap.parent.transform.position.z);
                    spawnerInstancied.name = "Spawner " + spawnerRestants;

                    for (int k = -spawnOptions.radius; k < spawnOptions.radius; k++)
                    {
                        for (int j = -spawnOptions.radius; j < spawnOptions.radius; j++)
                        {
                            int indexX = x + k;
                            int indexY = y + j;
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

    //Choisi le chunk a spawner en fonction de leur drop rate, parmis la liste des obstacles.
    [CanBeNull]
    public Obstacle? GetObstacleToSpawn(List<Obstacle> obstacles)
    {
        if (obstacles.Count > 0)
        {
            Obstacle? returnedObject = null;

            int maxDrop = obstacles.Sum(obstacle => obstacle.dropRate);
            int maxCap = 100;

            int numberChoosed = 0;

            if (maxDrop > maxCap)
            {
                numberChoosed = UnityEngine.Random.Range(0, maxDrop);
            }
            else
            {
                numberChoosed = UnityEngine.Random.Range(0, maxCap);
            }

            int dropsMin = 0;
            int dropsMax = 0;
            obstacles.ForEach(obstacle =>
            {
                dropsMin = dropsMax;
                dropsMax = dropsMin + obstacle.dropRate;

                if (Enumerable.Range(dropsMin, dropsMax).Contains(numberChoosed))
                {
                    returnedObject = obstacle;
                }
            });

            return returnedObject;
        }
        else
        {
            return null;
        }
    }


    //Choisi le chunk a spawner en fonction de leur drop rate, parmis la liste des obstacles.
    [CanBeNull]
    public Chunk? GetChunkToSpawn(List<Chunk> chunks)
    {
        if (chunks.Count > 0)
        {
            Chunk? returnedObject = null;

            int maxDrop = chunks.Sum(chunks => chunks.dropRate);
            int maxCap = 100;

            int numberChoosed = 0;

            if (maxDrop > maxCap)
            {
                numberChoosed = UnityEngine.Random.Range(0, maxDrop);
            }
            else
            {
                numberChoosed = UnityEngine.Random.Range(0, maxCap);
            }

            int dropsMin = 0;
            int dropsMax = 0;
            chunks.ForEach(chunk =>
            {
                dropsMin = dropsMax;
                dropsMax = dropsMin + chunk.dropRate;

                if (Enumerable.Range(dropsMin, dropsMax).Contains(numberChoosed))
                {
                    returnedObject = chunk;
                }
            });

            return returnedObject;
        }
        else
        {
            return null;
        }
    }


    public bool IsObstacleExist(Vector2 center, float radius)
    {
        bool collided = false;

        float posX = (center.x - (noiseMap.GetLength(0) / 2 - baseMap.parent.transform.position.x));
        float poxY = (center.y - (noiseMap.GetLength(1) / 2 - baseMap.parent.transform.position.y));

        Collider2D[] hitColliders =
            Physics2D.OverlapCircleAll(
                new Vector2(posX, poxY), radius);

        if (hitColliders.Length > 0)
        {
            foreach (Collider2D col in hitColliders)
            {
                if (col.gameObject.layer == LayerMask.NameToLayer(layerToAvoid))
                {
                    collided = true;
                }
            }
        }


        return collided;
    }
}