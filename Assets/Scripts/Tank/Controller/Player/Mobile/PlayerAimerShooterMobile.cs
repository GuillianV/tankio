using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.InputSystem.OnScreen;

public class PlayerAimerShooterMobile : PlayerController 
{
    public Transform towerTransform;
    public GameObject projectile;

 #if UNITY_ANDROID

    private TowerManager m_towerManager;
    private TowerController m_towerController;
    private GunManager m_gunManager;
    private GunController m_gunController;
    
    private GameManager m_Game;
    private Vector2 vectorToTarget = new Vector2(0,0);
    private bool isReloading = false;
    private bool isFireing = false;
    public InputTank inputTank;
    public event EventHandler<ProjectileEvent> BulletDestroyed;
    public event EventHandler<ProjectileEvent> BulletCreated;
    private OnScreenStickHandler onScreenStickHandler;


    protected override void Awake()
    {
        base.Awake();
        inputTank = new InputTank();
        inputTank.Enable();

        m_gunManager = m_tankController.GetTankManager<GunManager>();
        m_gunController = m_gunManager.gunController;
        if (m_gunController==null)
            Debug.LogError("Player Aimer Shooter Mobile missing GunController");
        
        m_towerManager = m_tankController.GetTankManager<TowerManager>();
        m_towerController = m_towerManager.towerController;
        if (m_towerController ==null)
            Debug.LogError("Player Aimer Shooter Mobile missing TowerController");

        m_Game = GameManager.Instance;
        inputTank.Tank.FireGameStick.canceled += ctx => Cancelled();
      
    }

    private void Start()
    {
        onScreenStickHandler = GameObject.FindGameObjectWithTag("FireUIMobile").GetComponent<OnScreenStickHandler>();
        onScreenStickHandler.OnTap += Taped;
    }


    void FixedUpdate()
    {

       
            Quaternion q = TMath.GetAngleFromVector2D(vectorToTarget, -90);
            towerTransform.rotation = Quaternion.Slerp(towerTransform.rotation, q, Time.deltaTime  * m_Game.TimeManager.timeScale* m_towerController.GetTowerRotationSpeed());

    }


    public void Update()
    {
        if (isFireing)
        {
             Fire();
        }
    }

    protected void Taped(object sender, EventArgs eventArgs)
    {
        Fire();
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
                
                    GameObject ammo = Instantiate(projectile, m_gunController.bulletSpawn.transform.position, m_gunController.bulletSpawn.transform.rotation) as  GameObject;
                    BulletDestroyed bulletDestroyed = ammo.GetComponent<BulletDestroyed>();
                    BulletCreated bulletCreated = ammo.GetComponent<BulletCreated>();
                    bulletDestroyed.Destroyed += OnBulletDestroyed;
                    bulletCreated.Created += OnBulletCreated;  
                
                    Projectile_Bullet ammoProjectile = ammo.GetComponent<Projectile_Bullet>();
                    ammoProjectile.BulletStats.velocity = m_gunController.GetBulletVelocity();
                    ammoProjectile.parentUp = m_gunController.bulletSpawn.transform.up;
                    ammoProjectile.senderTag = gameObject.tag;
                    m_gunManager.gunAnimator.CallAnimator("BulletSpawn").SetTrigger("Fire");
                    isReloading = true;
    
               
                    m_Game.Audio.Play("tank-shoot-"+ UnityEngine.Random.Range(1, 3));
                
                    StartCoroutine(Reload());
                }
            
    
            }
            
        
       
    }

    
    public void OnFireGameStick(InputValue input)
    {
        Vector2 inputVec = input.Get<Vector2>();

        if (inputVec.magnitude > 0)
        {

            vectorToTarget = inputVec;
        }
        
        if (inputVec.magnitude > 1)
        {

            isFireing = true;
        }
    }

#endif

}
