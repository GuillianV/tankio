using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



[System.Serializable]
public class Enemy 
{
    public GameObject enemyPrefab;

    [Range(1, 20)] public int difficultyLevel;

    [Header("Player Stats")] public TracksData tracksData;
    public BodyData bodyData;
    public TowerData towerData;
    public GunData gunData;
    [HideInInspector] public GameObject enemy;
    
    
}




