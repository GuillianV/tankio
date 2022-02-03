using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Random = Unity.Mathematics.Random;



public class PlayerShoot : PlayerController
{
    public GameObject projectile;

#if UNITY_STANDALONE
    
    private GunManager m_gunManager;
    private GunController m_gunController;
    private TankBaseAnimator m_gunAnimator;
   
    private bool isReloading = false;
    private bool isFireing = false;
    private GameManager m_Game;
    public InputTank inputTank;
    public event EventHandler<ProjectileEvent> BulletDestroyed;
    public event EventHandler<ProjectileEvent> BulletCreated;

    protected override void Awake()
    {
        base.Awake();
        inputTank = new InputTank();
        inputTank.Enable();
        m_Game = GameManager.Instance;
        inputTank.Tank.FireGameStick.canceled += ctx => Cancelled();

        m_gunManager = m_tankController.GetTankManager<GunManager>();

        m_gunController = m_gunManager.gunController;
        if (m_gunController == null)
            Debug.LogError("Player Shoot missing GunController");

        m_gunAnimator = m_gunManager.gunAnimator;
        if ( m_gunAnimator == null)
            Debug.LogError("Player Shoot missing GunAnimator");

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
     
            yield return new WaitForSeconds(m_gunController.GetReloadTimeSpeed());
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
                if (m_gunController != null)
                {

                    GameObject ammo = Instantiate(projectile,
                        m_gunController.bulletSpawn.transform.position,
                        m_gunController.bulletSpawn.transform.rotation) as GameObject;
                    BulletDestroyed bulletDestroyed = ammo.GetComponent<BulletDestroyed>();
                    BulletCreated bulletCreated = ammo.GetComponent<BulletCreated>();
                    bulletDestroyed.Destroyed += OnBulletDestroyed;
                    bulletCreated.Created += OnBulletCreated;

                    Projectile_Bullet ammoProjectile = ammo.GetComponent<Projectile_Bullet>();
                    ammoProjectile.BulletStats.velocity = m_gunController.GetBulletVelocity();
                    ammoProjectile.parentUp = m_gunController.bulletSpawn.transform.up;
                    ammoProjectile.senderTag = gameObject.tag;
                    m_gunAnimator.CallAnimator("BulletSpawn").SetTrigger("Fire");   
                    isReloading = true;


                    m_Game.Audio.Play("tank-shoot-" + UnityEngine.Random.Range(1, 3));

                    StartCoroutine(Reload());

                }
                else
                {
            
                    Debug.LogError("Player Shoot missing GunController");
                }
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
    
#endif
}
