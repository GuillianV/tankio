using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{

    public ProjectileEventHandler ProjectileEventHandler;

    public List<GameObject> ProjectilePlayerList = new List<GameObject>();
    public List<GameObject> ProjectileEnemiesList = new List<GameObject>();
    
    private void Awake()
    {
        ProjectileEventHandler.BulletShooted += BulletShootedHandler;
        ProjectileEventHandler.BulletDestroyed += BulletDestroyedHandler;
    }
    
    private void BulletShootedHandler(object sender, ProjectileEvent e)
    {
        if (e.Tag == "Player")
        {
            ProjectilePlayerList.Add(e.Projectile);
        }
    
        if (e.Tag == "Enemy")
        {
            ProjectileEnemiesList.Add(e.Projectile);
        }
    
    }
    
    
    private void BulletDestroyedHandler(object sender, ProjectileEvent e)
    {
        if (e.Tag == "Player")
        {
            ProjectilePlayerList.Remove(e.Projectile);
        }
    
        if (e.Tag == "Enemy")
        {
            ProjectileEnemiesList.Remove(e.Projectile);
        }
    
    }
}
