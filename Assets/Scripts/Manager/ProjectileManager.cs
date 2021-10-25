using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{

    public GameObject entityContainer;

    public List<GameObject> entityList = new List<GameObject>();
    private void Start()
    {
        foreach (Transform child in entityContainer.transform)
        {
            TankAI tankComponent = child.gameObject.GetComponent<TankAI>();
            if (tankComponent!=null)
            {
             
            }
        }
    }

    public void BulletShootedHandler(object _projectile,ProjectileEvent _projectileEvent)
    {
        
        entityList.Add(_projectileEvent.Projectile);
    }

  
    
    

}
