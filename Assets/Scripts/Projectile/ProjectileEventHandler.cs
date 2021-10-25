using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEventHandler : MonoBehaviour
{
    
    public static ProjectileEventHandler Instance;

    public event EventHandler<ProjectileEvent> BulletShooted;
    public event EventHandler<ProjectileEvent> BulletDestroyed;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }else if (Instance != this)
        {
            Destroy(gameObject);
        }

    }

    public void OnBulletShooted(GameObject projectile, string senderTag)
    {
        BulletShooted?.Invoke(this, new ProjectileEvent(projectile,senderTag));
    }
    
    public void OnBulletDestroyed(GameObject projectile, string senderTag)
    {
        BulletDestroyed?.Invoke(this, new ProjectileEvent(projectile,senderTag));
    } 
    
}
