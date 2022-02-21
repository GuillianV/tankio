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
    private TracksController TracksController;
    private GunManager m_gunManager;
    private GunController GunController;
    private TowerController TowerController;
    private BodyController BodyController;

    private TankBaseAnimator m_gunAnimator;
    
    private TankDestroyed m_tankDestroyed;
    private bool isReloading;
    private GameManager m_Game;
    public event EventHandler<ProjectileEvent> BulletDestroyed;
    public event EventHandler<ProjectileEvent> BulletCreated;

    [Range(0.1f, 20f)]
    public float repathRate = 1;
    [Range(0, 10f)]
    public float velocityRate = 1;

    [Header("Aimer Setting")]
    public Transform towerTransform;
    public Transform spawnBullet;
    public GameObject bullet;


    private void Awake()
    {
        m_Game = GameManager.Instance;
    
    }


    private void Start()
    {
        m_aiPath = GetComponent<AIPath>();
        m_tankController = GetComponent<TankController>();
        m_gunManager = m_tankController.GetTankManager<GunManager>();
        GunController = m_tankController.GetTankManager<GunManager>().gunController;
        BodyController =  m_tankController.GetTankManager<BodyManager>().bodyController;
        TowerController = m_tankController.GetTankManager<TowerManager>().towerController;
        TracksController = m_tankController.GetTankManager<TracksManager>().tracksController;
        m_aiDestinationSetter = GetComponent<AIDestinationSetter>();
        m_tankDestroyed = GetComponent<TankDestroyed>();

        m_gunAnimator = m_gunManager.gunAnimator;

        UpdateAstar();

    }

    public void UpdateAstar()
    {
        m_aiPath.maxSpeed = TracksController.GetTrackSpeed() * Time.deltaTime * m_Game.TimeManager.timeScale * velocityRate;
        m_aiPath.maxAcceleration = TracksController.GetTrackSpeed() * Time.deltaTime * m_Game.TimeManager.timeScale * velocityRate;
        m_aiPath.rotationSpeed = TracksController.GetTrackRotationSpeed() * Time.deltaTime * m_Game.TimeManager.timeScale * 100 * velocityRate;
        m_aiPath.repathRate = repathRate;
    }


    public void OnBulletDestroyed(object sender, EventArgs args)
    {

        BulletDestroyed bulletDestroyed = sender as BulletDestroyed;
        BulletDestroyedHandler(bulletDestroyed.gameObject, bulletDestroyed.tag);
    }

    public void OnBulletCreated(object sender, EventArgs args)
    {
        BulletCreated bulletCreated = sender as BulletCreated;
        BulletCreatedHandler(bulletCreated.gameObject, bulletCreated.tag);
    }

    public void BulletDestroyedHandler(GameObject bullet, string tag)
    {
        BulletDestroyed?.Invoke(this, new ProjectileEvent(bullet, tag));
    }

    public void BulletCreatedHandler(GameObject bullet, string tag)
    {
        BulletCreated?.Invoke(this, new ProjectileEvent(bullet, tag));
    }



    private void FixedUpdate()
    {
        UpdateAstar();

        if (m_Game.TimeManager.timeScale > 0)
        {
       



            if (m_aiDestinationSetter.target != null)
            {
                Vector3 vectorToTarget = new Vector3(m_aiDestinationSetter.target.position.x, m_aiDestinationSetter.target.position.y, towerTransform.transform.position.z) - towerTransform.transform.position;
                Quaternion q = TMath.GetAngleFromVector2D(vectorToTarget, -90);
                towerTransform.transform.rotation = Quaternion.Slerp(towerTransform.transform.rotation, q, Time.deltaTime * m_Game.TimeManager.timeScale * TowerController.GetTowerRotationSpeed());



                RaycastHit2D hit = Physics2D.Raycast(new Vector2(spawnBullet.position.x, spawnBullet.position.y), new Vector2(spawnBullet.transform.up.x, spawnBullet.transform.up.y));
                if (hit != false)
                {
                    if (hit.collider.gameObject.CompareTag("Player"))
                    {
                        if (!isReloading)
                        {


                            GameObject ammo = Instantiate(bullet, spawnBullet.transform.position, spawnBullet.transform.rotation) as GameObject;
                            BulletDestroyed bulletDestroyed = ammo.GetComponent<BulletDestroyed>();
                            BulletCreated bulletCreated = ammo.GetComponent<BulletCreated>();
                            bulletDestroyed.Destroyed += OnBulletDestroyed;
                            bulletCreated.Created += OnBulletCreated;
                            m_gunAnimator.CallAnimator("BulletSpawn").SetTrigger("Fire");   
                            Projectile_Bullet ammoProjectile = ammo.GetComponent<Projectile_Bullet>();
                            ammoProjectile.BulletStats.velocity = GunController.GetBulletVelocity();
                            ammoProjectile.parentUp = spawnBullet.transform.up;
                            ammoProjectile.senderTag = gameObject.tag;
                            isReloading = true;
                            StartCoroutine(Reload());
                        }
                    }
                }
            }
        }


    }


    IEnumerator Reload()
    {
        yield return new WaitForSeconds(GunController.GetReloadTimeSpeed());
        isReloading = false;
    }

 
}

