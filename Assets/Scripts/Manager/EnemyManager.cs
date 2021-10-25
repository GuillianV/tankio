using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    public Transform parentContainer;
    
    [System.Serializable]
    public struct Enemys {
        public GameObject enemyPrefab;
        public Vector2 enemyPosition;
        
        [Header("Player Stats")]
        public TracksData TracksData;
        public BodyData BodyData;
        public TowerData TowerData;
        public GunData GunData;
    }
 
    public List<Enemys>  enemyList;
    // Start is called before the first frame update

    private List<GameObject> enemys;
    

    public void EnableEnemys()
    {
        enemys.ForEach(E =>
        {
            E.SetActive(true);
        });
        
    }
    
    public void DisableEnemys()
    {
        enemys.ForEach(E =>
        {
            E.SetActive(false);
        });
        
    }

    public void InsatanciateEnemys()
    {
       
        enemyList.ForEach(E =>
        {
            GameObject enemy = Instantiate(E.enemyPrefab, new Vector3(E.enemyPosition.x,E.enemyPosition.y,0), new Quaternion(0,0,0,0),parentContainer) as GameObject;
            TankController tankController = enemy.GetComponent<TankController>();
            AIDestinationSetter aiDestinationSetter = enemy.GetComponent<AIDestinationSetter>();
            aiDestinationSetter.target = GameObject.FindWithTag("Player").transform;
            
            if (E.TracksData != null)
            {
                tankController.tracks.LoadData(E.TracksData);
            }
            if (E.BodyData != null)
            {
                tankController.body.LoadData(E.BodyData);
            }
            if (E.TowerData != null)
            {
                tankController.tower.LoadData(E.TowerData);
            }
            if (E.GunData != null)
            {
                tankController.gun.LoadData(E.GunData);
            }

            
        });
        
    }

}
