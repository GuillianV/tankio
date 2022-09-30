using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using Pathfinding;
using UnityEngine;
using Debug = UnityEngine.Debug;

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

    
    private int refreshTick = 10;
    private Transform target;
    private float TowerRotationSpeed;
    private float TrackSpeed;
    private float TrackRotationSpeed;
    private Vector3 spawnBulletUp;
    private Vector3 spawnBulletPos;
    
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
        TowerRotationSpeed = m_towerManager.towerController.GetTowerRotationSpeed();
        TrackRotationSpeed = m_tracksManager.tracksController.GetTrackRotationSpeed();
        TrackSpeed = m_tracksManager.tracksController.GetTrackSpeed();
        spawnBullet = m_gunManager.gunAsset.CallAsset("BulletSpawn").transform;
        m_Game.TimeManager.OnTimeChanged += TimeChangedHandler;
        
      m_tracksManager.tracksAnimator.CallAnimator("Tracks-Left").SetBool("Moving", true);
      m_tracksManager.tracksAnimator.CallAnimator("Tracks-Right").SetBool("Moving", true);

      
      target =  m_aiDestinationSetter.target;
      if (target == null)
      {
          Debug.LogError("TargetNotFound TankAI");
      }

      
        UpdateAstar();
        
        

    }

    public void UpdateAstar()
    {
        
        m_aiPath.maxSpeed = TrackSpeed * Time.deltaTime   * velocityRate;
        m_aiPath.maxAcceleration = TrackSpeed * Time.deltaTime   * velocityRate;
        m_aiPath.rotationSpeed = TrackRotationSpeed * Time.deltaTime   * 100 * velocityRate;
        m_aiPath.repathRate = repathRate;
       
    }


    
    private void FixedUpdate()
    {
        
        //Old 350 ~ 1300
        //New 150
        
        
        if (m_Game.TimeManager.timeScale > 0)
        {
            if (target == null)
            {
                Destroy(this);
                return;
            }
            
            Vector3 targetPos = target?.position ?? Vector3.zero;
            towerTransform.transform.rotation = Quaternion.Slerp(towerTransform.transform.rotation, TMath.GetAngleFromVector2D( targetPos- towerTransform.transform.position, -90), Time.deltaTime * m_Game.TimeManager.timeScale * TowerRotationSpeed);
            
            if (refreshTick <= 0)
            {
                spawnBulletUp = spawnBullet.up;
                spawnBulletPos = spawnBullet.position;
                TowerRotationSpeed = m_towerManager.towerController.GetTowerRotationSpeed();
                TrackRotationSpeed = m_tracksManager.tracksController.GetTrackRotationSpeed();
                
                RaycastHit2D hit = Physics2D.Raycast(new Vector2(spawnBulletPos.x, spawnBulletPos.y), new Vector2(spawnBulletUp.x,spawnBulletUp.y));
                if (hit != false && hit.collider.gameObject.CompareTag("Player") && !isReloading)
                {
                    /*GameObject ammo =*/  m_gunManager.Shoot();
                    isReloading = true;
                    StartCoroutine(Reload());
                  
                }

                refreshTick = 10;
            }

            refreshTick--;
        }
       

    }


    IEnumerator Reload()
    {
        yield return new WaitForSeconds(m_gunManager.gunController.GetReloadTimeSpeed());
        isReloading = false;
    }

    public void TimeChangedHandler(object sender, BoolEventargs boolEventargs)
    {
        //Enable time
        m_aiPath.canMove = boolEventargs.Value;
        m_tracksManager.tracksAnimator.ToggleAllAnimators(boolEventargs.Value);
        if (boolEventargs.Value)
        {
            UpdateAstar();
        }
       
        
    }

 
}

