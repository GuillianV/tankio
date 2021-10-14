using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Projectile
{
    public BulletData Data;

    public void LoadData(BulletData _data)
    {
        Data = _data;
    }
    
    public override void Fire()
    {
        gameObject.SetActive(true);
        
    }

    public override void Collided()
    {
        Debug.Log("bullet collided");
        Destroy();
    }

    public override void Destroy()
    {
        
        Destroy(gameObject);
    }

   
}


