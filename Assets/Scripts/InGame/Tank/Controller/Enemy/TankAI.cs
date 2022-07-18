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

    private TracksManager m_tracksManager;
    private GunManager m_gunManager;
    private TowerManager m_towerManager;
 
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
        m_aiDestinationSetter = GetComponent<AIDestinationSetter>();
        m_tankController = GetComponent<TankController>();
        m_tankController.BindTank();
        m_tracksManager =  m_tankController.GetTankManager<TracksManager>();
        m_towerManager = m_tankController.GetTankManager<TowerManager>();
        m_gunManager = m_tankController.GetTankManager<GunManager>();
        towerTransform = m_towerManager.towerAsset.CallAsset("Tower").transform;
        spawnBullet = m_gunManager.gunAsset.CallAsset("BulletSpawn").transform;
   
       
      m_tracksManager.tracksAnimator.CallAnimator("Tracks-Left").SetBool("Moving", true);
      m_tracksManager.tracksAnimator.CallAnimator("Tracks-Right").SetBool("Moving", true);


        UpdateAstar();

    }

    public void UpdateAstar()
    {
        m_aiPath.maxSpeed = m_tracksManager.tracksController.GetTrackSpeed() * Time.deltaTime * m_Game.TimeManager.timeScale * velocityRate;
        m_aiPath.maxAcceleration = m_tracksManager.tracksController.GetTrackSpeed() * Time.deltaTime * m_Game.TimeManager.timeScale * velocityRate;
        m_aiPath.rotationSpeed = m_tracksManager.tracksController.GetTrackRotationSpeed() * Time.deltaTime * m_Game.TimeManager.timeScale * 100 * velocityRate;
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
                towerTransform.transform.rotation = Quaternion.Slerp(towerTransform.transform.rotation, q, Time.deltaTime * m_Game.TimeManager.timeScale * m_towerManager.towerController.GetTowerRotationSpeed());

                RaycastHit2D hit = Physics2D.Raycast(new Vector2(spawnBullet.position.x, spawnBullet.position.y), new Vector2(spawnBullet.transform.up.x, spawnBullet.transform.up.y));
                if (hit != false)
                {
                    if (hit.collider.gameObject.CompareTag("Player"))
                    {
                        if (!isReloading)
                        {
                           GameObject ammo =  m_gunManager.Shoot();
                          
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
        yield return new WaitForSeconds(m_gunManager.gunController.GetReloadTimeSpeed());
        isReloading = false;
    }

 
}

