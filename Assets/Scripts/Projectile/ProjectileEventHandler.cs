using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEventHandler : MonoBehaviour
{
    public event EventHandler<ProjectileEvent> BulletShooted;
    public event EventHandler<ProjectileEvent> BulletDestroyed;

    private string senderTag;
    private GameObject projectile;

    public ProjectileEventHandler(GameObject _projectile,string _senderTag)
    {
        this.senderTag = _senderTag;
        this.projectile = _projectile;
    }
    
    
    public void OnBulletShooted()
    {
        BulletShooted?.Invoke(this, new ProjectileEvent(this.projectile,this.senderTag));
    }
    
    public void OnBulletDestroyed()
    {
        BulletDestroyed?.Invoke(this, new ProjectileEvent(this.projectile,this.senderTag));
    } 
    
}
