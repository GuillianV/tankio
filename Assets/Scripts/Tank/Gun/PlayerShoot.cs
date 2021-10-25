using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{

    private TankController m_tankController;
    public GameObject projectile;

    private bool isReloading = false;


    private void Awake()
    {
        m_tankController = GetComponent<TankController>();
    }

    private void FixedUpdate()
    {
        if (Input.GetButton("Fire1"))
        {
            if (!isReloading)
            {

                GameObject ammo = Instantiate(projectile, m_tankController.GunController.bulletSpawn.transform.position, m_tankController.GunController.bulletSpawn.transform.rotation) as  GameObject;
                Projectile_Bullet ammoProjectile = ammo.GetComponent<Projectile_Bullet>();
                ammoProjectile.velocity = m_tankController.GunController.gun.Data.bulletVelocity;
                ammoProjectile.parentUp = m_tankController.GunController.bulletSpawn.transform.up;
                ammoProjectile.senderTag = gameObject.tag;
                isReloading = true;
                StartCoroutine(Reload());
            }
            
            
       
            

        }
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(m_tankController.GunController.gun.Data.reloadTimeSecond);
        isReloading = false;
    }
    
  
}
