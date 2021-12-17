using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Random = Unity.Mathematics.Random;


#if !UNITY_ANDROID
public class PlayerShoot : MonoBehaviour
{

    private TankController m_tankController;
    public GameObject projectile;
    private bool isReloading = false;
    private bool isFireing = false;
    private GameManager m_Game;
    public InputTank inputTank;
    public event EventHandler<ProjectileEvent> BulletDestroyed;
    public event EventHandler<ProjectileEvent> BulletCreated;

    private void Awake()
    {
        inputTank = new InputTank();
        inputTank.Enable();
        m_tankController = GetComponent<TankController>();
        m_Game = GameManager.Instance;
        inputTank.Tank.FireGameStick.canceled += ctx => Cancelled();
    }


    public void Update()
    {
        if (isFireing)
        {
             Fire();
        }
    } 

    protected void Cancelled()
    {
        isFireing = false;
    }

    
    IEnumerator Reload()
    {
        yield return new WaitForSeconds(m_tankController.StatsController.reloadTimeSpeed);
        isReloading = false;
    }

    
    public void OnBulletDestroyed(object sender, EventArgs args)
    {
    
        BulletDestroyed bulletDestroyed = sender as BulletDestroyed;
        BulletDestroyedHandler(bulletDestroyed.gameObject,bulletDestroyed.tag);
    }
    
    public void OnBulletCreated(object sender, EventArgs args)
    {
        BulletCreated bulletCreated = sender as BulletCreated;
        BulletCreatedHandler(bulletCreated.gameObject,bulletCreated.tag);
    }
    
    public void BulletDestroyedHandler(GameObject bullet, string tag)
    {
        BulletDestroyed?.Invoke(this,new ProjectileEvent(bullet,tag));
    }
    
    public void BulletCreatedHandler(GameObject bullet, string tag)
    {
        BulletCreated?.Invoke(this,new ProjectileEvent(bullet,tag));
    }

    
    public void Fire()
    {
        if (!isReloading)
        {
    
            if (m_Game.TimeManager.timeScale > 0)
            {
                
                GameObject ammo = Instantiate(projectile, m_tankController.GunController.bulletSpawn.transform.position, m_tankController.GunController.bulletSpawn.transform.rotation) as  GameObject;
                BulletDestroyed bulletDestroyed = ammo.GetComponent<BulletDestroyed>();
                BulletCreated bulletCreated = ammo.GetComponent<BulletCreated>();
                bulletDestroyed.Destroyed += OnBulletDestroyed;
                bulletCreated.Created += OnBulletCreated;  
                
                Projectile_Bullet ammoProjectile = ammo.GetComponent<Projectile_Bullet>();
                ammoProjectile.BulletStats.velocity = m_tankController.StatsController.bulletVelocity;
                ammoProjectile.parentUp = m_tankController.GunController.bulletSpawn.transform.up;
                ammoProjectile.senderTag = gameObject.tag;
                m_tankController.TankAnimationController.FireProjectile();
                isReloading = true;
    
               
                m_Game.Audio.Play("tank-shoot-"+ UnityEngine.Random.Range(1, 3));
                
                StartCoroutine(Reload());
            }
            
    
        }
    
    }
    
    public void OnFire(InputValue input)
    {
        isFireing = input.isPressed;


    }
    
    public void OnFireGameStick(InputValue input)
    {
        
         
        Vector2 inputVec = input.Get<Vector2>();
        if (inputVec.magnitude > 1)
        {

            isFireing = true;
        }
    }
    
  
}
#endif