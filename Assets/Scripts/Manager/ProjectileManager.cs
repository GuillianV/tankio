using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{

    public InstanciateProjectile Projectile;

    public List<Bullet> bulletPlayerList = new List<Bullet>();
    public List<Bullet> bulletEnemiesList = new List<Bullet>();

    private void Awake()
    {
        Projectile.BulletShooted += BulletShootedHandler;
    }

    private void BulletShootedHandler(object sender, BulletEvent e)
    {
        if (e.Tag == "Player")
        {
            bulletPlayerList.Add(e.Bullet);
        }

        if (e.Tag == "Enemy")
        {
            bulletEnemiesList.Add(e.Bullet);
        }

    }
}
