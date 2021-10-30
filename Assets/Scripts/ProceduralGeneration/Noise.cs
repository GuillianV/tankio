using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise 
{

    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, float scale)
    {
        
        float randModifier = Mathf.Clamp( Random.Range(0, 254),0,1);
        
        float[,] noiseMap = new float[mapWidth, mapHeight];

        if (scale <= 0)
        {
            scale = 0.001f;
        }
        
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                float sampleX = (float)x /scale * randModifier;
                float sampleY =(float)y / scale * randModifier;

                float perlinValue = Mathf.PerlinNoise(sampleX * 1f, sampleY *1f);
                noiseMap[x, y] = perlinValue;
            }
        }

        return noiseMap;
    }
    
}
