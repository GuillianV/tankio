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

    
    [HideInInspector] public GameObject enemy;


    [Header("Enemy Stats")]
    public TankScriptable tankScriptable;

}




