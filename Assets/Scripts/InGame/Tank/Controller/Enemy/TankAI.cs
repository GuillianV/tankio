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
    private BaseAnimator TracksAnimator;
    private GunManager m_gunManager;
    private GunController GunController;
    private TowerController TowerController;
    private BodyController BodyController;
    private BaseAsset m_gunAssets;
    private BaseAsset m_towerAssets;
    private BaseAnimator m_gunAnimator;
    
    private TankDestroyed m_tankDestroyed;
    private bool isReloading;
    private GameManager m_Game;

    [Range(0.1f, 20f)]
    public float repathRate = 1;
    [Range(0, 10f)]
    public float velocityRate = 1;

    [Header("Aimer Setting")]
    private Transform towerTransform;
    private Transform spawnBullet;
    private GameObject bullet;


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
        m_gunAssets = m_tankController.GetTankManager<GunManager>().gunAsset;
        BodyController =  m_tankController.GetTankManager<BodyManager>().bodyController;
        TowerController = m_tankController.GetTankManager<TowerManager>().towerController;
        m_towerAssets = m_tankController.GetTankManager<TowerManager>().towerAsset;
        TracksController = m_tankController.GetTankManager<TracksManager>().tracksController;
        m_aiDestinationSetter = GetComponent<AIDestinationSetter>();
        m_tankDestroyed = GetComponent<TankDestroyed>();

        TracksAnimator = m_tankController.GetTankManager<TracksManager>().tracksAnimator;
        m_gunAnimator = m_gunManager.gunAnimator;

        towerTransform = m_towerAssets.CallAsset("Tower").transform;
        spawnBullet = m_gunAssets.CallAsset("BulletSpawn").transform;
        bullet = m_gunAssets.CallAsset("Projectile");

       
        TracksAnimator.CallAnimator("Tracks-Left").SetBool("Moving", true);
        TracksAnimator.CallAnimator("Tracks-Right").SetBool("Moving", true);


        UpdateAstar();

    }

    public void UpdateAstar()
    {
        m_aiPath.maxSpeed = TracksController.GetTrackSpeed() * Time.deltaTime * m_Game.TimeManager.timeScale * velocityRate;
        m_aiPath.maxAcceleration = TracksController.GetTrackSpeed() * Time.deltaTime * m_Game.TimeManager.timeScale * velocityRate;
        m_aiPath.rotationSpeed = TracksController.GetTrackRotationSpeed() * Time.deltaTime * m_Game.TimeManager.timeScale * 100 * velocityRate;
        m_aiPath.repathRate = repathRate;
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
                           GameObject ammo =  m_gunManager.Shoot();
                            //IManager imanager = ammoProjectile.GetComponent<IManager>();
                            //if (imanager != null)
                            //    imanager.Bind();


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

