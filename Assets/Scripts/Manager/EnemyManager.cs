using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Pathfinding;
using UnityEditor;
using UnityEngine;


public class EnemyManager : MonoBehaviour
{
    //Parent container of enemies
    public Transform parentContainer;




    public List<Enemy> enemyList;

    
    //Active les enemies 
    public void EnableEnemys()
    {
        enemyList.ForEach(E => { E.enemy.SetActive(true); });
    }

    //Desactive les enemies
    public void DisableEnemys()
    {
        enemyList.ForEach(E => { E.enemy.SetActive(false); });
    }


    [CanBeNull]
    public List<Enemy> GetEnemies(int difficultyLevel)
    {
        return enemyList.Where(enemy => enemy.difficultyLevel == difficultyLevel).ToList();
    }
    
  
}

