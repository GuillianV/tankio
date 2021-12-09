using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Pathfinding;
using UnityEngine;

public class TankAI : MonoBehaviour
{
    private AIPath m_aiPath;
    private AIDestinationSetter m_aiDestinationSetter;
    private TankController m_tankController;
    private TankDestroyed m_tankDestroyed;
    private bool isReloading;
    private GameManager m_Game;
    public event EventHandler<ProjectileEvent> BulletDestroyed;
    public event EventHandler<ProjectileEvent> BulletCreated;
    
    [Range(0.1f,20f)]
    public float repathRate = 1;
    [Range(0,10f)]
    public float velocityRate = 1;

    [Header("Aimer Setting")]
    public Transform towerTransform;
    public Transform spawnBullet;
    public GameObject bullet;
    
    
    private void Awake()
    {
        m_Game = GameManager.Instance;
        m_aiPath = GetComponent<AIPath>();
        m_tankController = GetComponent<TankController>();
        m_aiDestinationSetter = GetComponent<AIDestinationSetter>();
        m_tankDestroyed = GetComponent<TankDestroyed>();
    }


    private void Start()
    {
        UpdateAstar();

    }

    public void UpdateAstar()
    {
        m_aiPath.maxSpeed = m_tankController.StatsController.tracksSpeed * Time.deltaTime * m_Game.TimeManager.timeScale * velocityRate;
        m_aiPath.maxAcceleration = m_tankController.StatsController.tracksSpeed * Time.deltaTime * m_Game.TimeManager.timeScale  * velocityRate;
        m_aiPath.rotationSpeed = m_tankController.StatsController.tracksRotationSpeed * Time.deltaTime * m_Game.TimeManager.timeScale  * 100 * velocityRate;
        m_aiPath.repathRate = repathRate;
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

    
    
    private void FixedUpdate()
    {
        UpdateAstar();


        if (m_tankController.StatsController.health <= 0)
        {
            Destroy(gameObject);
        }

        if (m_aiDestinationSetter.target != null)
        {
            Vector3 vectorToTarget = new Vector3(m_aiDestinationSetter.target.position.x,m_aiDestinationSetter.target.position.y,towerTransform.transform.position.z)  - towerTransform.transform.position;
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.AngleAxis(angle -90 , Vector3.forward);
            towerTransform.transform.rotation = Quaternion.Slerp(towerTransform.transform.rotation, q, Time.deltaTime  * m_Game.TimeManager.timeScale  * m_tankController.StatsController.towerRotationSpeed);

        
        
            RaycastHit2D hit = Physics2D.Raycast( new Vector2(spawnBullet.position.x,spawnBullet.position.y), new Vector2(spawnBullet.transform.up.x,spawnBullet.transform.up.y));
            if (hit != false)
            {
                if (hit.collider.gameObject.tag == "Player")
                {
                    if (!isReloading)
                    {
                    
                    
                        GameObject ammo = Instantiate(bullet, spawnBullet.transform.position, spawnBullet.transform.rotation) as  GameObject;
                        BulletDestroyed bulletDestroyed = ammo.GetComponent<BulletDestroyed>();
                        BulletCreated bulletCreated = ammo.GetComponent<BulletCreated>();
                        bulletDestroyed.Destroyed += OnBulletDestroyed;
                        bulletCreated.Created += OnBulletCreated; 
                        m_tankController.TankAnimationController.FireProjectile();
                        Projectile_Bullet ammoProjectile = ammo.GetComponent<Projectile_Bullet>();
                        ammoProjectile.BulletStats.velocity = m_tankController.StatsController.bulletVelocity;
                        ammoProjectile.parentUp = spawnBullet.transform.up;
                        ammoProjectile.senderTag = gameObject.tag;
                        isReloading = true;
                        StartCoroutine(Reload());
                    }
                }
            }
        }
        
       
      
    }
    
    
    IEnumerator Reload()
    {
        yield return new WaitForSeconds(m_tankController.StatsController.reloadTimeSpeed);
        isReloading = false;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        Projectile_Bullet bullet = col.gameObject.GetComponent<Projectile_Bullet>();
        if (bullet != null)
        {
            m_tankController.StatsController.health -= bullet.BulletStats.damages;

            if (m_tankController.StatsController.health <= 0)
            {
                m_tankDestroyed.OnDestroyed(bullet.senderTag);
            }
        }
    }
}

