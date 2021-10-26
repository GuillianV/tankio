using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MapGeneratorManager : MonoBehaviour
{
    public int mapWidth;
    public int mapHeight;
    public float noiseScale;


    public Transform parent;
    public GameObject obstaclePrefab;
    public GameObject groundPrefab;
    public GameObject anglePrefab;
    
    private AstarPath path;
    
    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, noiseScale);
    }

    public void GenerateSampleMap()
    {
        GenerateGround();
        
    }

    private void Awake()
    {
        path = GetComponent<AstarPath>();
        GenerateSampleMap();
        GenerateWalls();
        AddObstacles();
        

    }

    IEnumerator  Start()
    {
        yield return new WaitForSeconds(0.1f);
        AstarPath.active.data.gridGraph.SetDimensions( mapWidth,mapHeight,1);
        AstarPath.active.Scan();
    }

    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.L))
    //     {
    //         
    //     }
    //     
    //     
    // }

    public void GenerateGround()
    {
        GameObject ground = Instantiate(groundPrefab, parent.transform.position, parent.transform.rotation, parent);
        SpriteRenderer spriteGround = ground.GetComponent<SpriteRenderer>();
        spriteGround.size = new Vector2(mapWidth,mapHeight);
        
    }
    
    public void GenerateWalls()
    {
        int margin = 2;
        
        GameObject wallLeft = Instantiate(obstaclePrefab, parent.transform.position, parent.transform.rotation, parent);
        SpriteRenderer spriteWallLeft = wallLeft.GetComponent<SpriteRenderer>();
        BoxCollider2D colliderWallLeft = wallLeft.GetComponent<BoxCollider2D>();

        spriteWallLeft.size = new Vector2(spriteWallLeft.size.x,mapHeight+margin);
        colliderWallLeft.size = new Vector2(colliderWallLeft.size.x,mapHeight+margin);
        wallLeft.transform.localPosition = new Vector3(mapWidth / 2, 0,0);
        
        GameObject wallRight = Instantiate(obstaclePrefab, parent.transform.position, parent.transform.rotation, parent);
        SpriteRenderer spriteWallRight = wallRight.GetComponent<SpriteRenderer>();
        BoxCollider2D colliderWallRight = wallRight.GetComponent<BoxCollider2D>();
        
        spriteWallRight.size = new Vector2(spriteWallRight.size.x,mapHeight+margin);
        colliderWallRight.size = new Vector2(colliderWallRight.size.x,mapHeight+margin);
        wallRight.transform.localPosition = new Vector3(-1*(mapWidth  / 2), 0,0);
        
        GameObject wallTop = Instantiate(obstaclePrefab, parent.transform.position, parent.transform.rotation, parent);
        SpriteRenderer spriteWallTop = wallTop.GetComponent<SpriteRenderer>();
        BoxCollider2D colliderWallTop = wallTop.GetComponent<BoxCollider2D>();
        
        spriteWallTop.size = new Vector2(mapWidth+margin,spriteWallTop.size.y);
        colliderWallTop.size = new Vector2(mapWidth+margin,colliderWallTop.size.y);
        wallTop.transform.localPosition = new Vector3(0, (mapHeight / 2),0);
        
        GameObject wallBottom = Instantiate(obstaclePrefab, parent.transform.position, parent.transform.rotation, parent);
        SpriteRenderer spriteWallBottom  = wallBottom.GetComponent<SpriteRenderer>();
        BoxCollider2D colliderWallBottom  = wallBottom.GetComponent<BoxCollider2D>();
        
        spriteWallBottom.size = new Vector2(mapWidth+margin,spriteWallBottom.size.y);
        colliderWallBottom.size = new Vector2(mapWidth+margin,colliderWallBottom.size.y);
        wallBottom.transform.localPosition = new Vector3(0, -1*(mapHeight / 2),0);
        
    }


    public void AddObstacles()
    {

    
        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, noiseScale);
        for (int y = 0; y < mapHeight; y=y+2)
        {
            for (int x = 0; x < mapWidth; x=x+2)
            {
                
                if (noiseMap[x,y] >= 0.85)
                {
                    Vector3 obstaclePos = new Vector3(-mapWidth / 2 + x, mapHeight / 2 - y, 0);
                    GameObject obstacle = Instantiate(obstaclePrefab, parent.transform.position, parent.transform.rotation, parent);
                    obstacle.transform.localPosition = obstaclePos;

                    
                }

 
                
            }
        }
        
    }
    
}
