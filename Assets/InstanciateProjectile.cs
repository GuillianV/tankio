using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanciateProjectile : MonoBehaviour
{

    public Transform spawnBullet;
    public TankController tankController;
    public GameObject Projectile;
    public string senderTag;
    public event EventHandler<BulletEvent> BulletShooted;
    

    public void Instanciate()
    {
        GameObject projectileInstance;
        projectileInstance = Instantiate(Projectile,spawnBullet.transform.position,spawnBullet.transform.rotation) as GameObject;
        
        Rigidbody2D projectileInstance_RigidBody2D = projectileInstance.GetComponent<Rigidbody2D>();
        Bullet projectileInstance_Bullet = projectileInstance.GetComponent<Bullet>();
        
        projectileInstance_RigidBody2D.AddForce(new Vector2(spawnBullet.transform.up.x *Time.deltaTime* tankController.gun.Data.bulletVelocity, spawnBullet.transform.up.y*Time.deltaTime * tankController.gun.Data.bulletVelocity ));
        OnBulletShooted(projectileInstance_Bullet);

    }
    
    public void OnBulletShooted(Bullet bullet)
    {
        BulletShooted?.Invoke(this, new BulletEvent(bullet,senderTag));
    }
    
}
