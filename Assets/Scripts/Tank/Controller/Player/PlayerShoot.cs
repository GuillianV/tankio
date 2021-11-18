using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{

    private TankController m_tankController;
    public GameObject projectile;

    private bool isReloading = false;
    private bool isFireing = false;
    

    private void Awake()
    {
        m_tankController = GetComponent<TankController>();
    }

 

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(m_tankController.StatsController.reloadTimeSpeed);
        isReloading = false;
    }

    public void OnFire()
    {
        if (!isReloading)
        {

            GameObject ammo = Instantiate(projectile, m_tankController.GunController.bulletSpawn.transform.position, m_tankController.GunController.bulletSpawn.transform.rotation) as  GameObject;
            Projectile_Bullet ammoProjectile = ammo.GetComponent<Projectile_Bullet>();
            ammoProjectile.BulletStats.velocity = m_tankController.StatsController.bulletVelocity;
            ammoProjectile.parentUp = m_tankController.GunController.bulletSpawn.transform.up;
            ammoProjectile.senderTag = gameObject.tag;
            m_tankController.TankAnimationController.FireProjectile();
            isReloading = true;
            StartCoroutine(Reload());
        }

    }
  
}
