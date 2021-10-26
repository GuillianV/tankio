using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class TankAI : MonoBehaviour
{
    private AIPath m_aiPath;
    private AIDestinationSetter m_aiDestinationSetter;
    private TankController m_tankController;
    private bool isReloading;

    
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
        m_aiPath = GetComponent<AIPath>();
        m_tankController = GetComponent<TankController>();
        m_aiDestinationSetter = GetComponent<AIDestinationSetter>();
    }


    private void Start()
    {
        m_aiPath.maxSpeed = m_tankController.StatsController.tracksSpeed * Time.deltaTime * velocityRate;
        m_aiPath.maxAcceleration = m_tankController.StatsController.tracksSpeed * Time.deltaTime * velocityRate;
        m_aiPath.rotationSpeed = m_tankController.StatsController.tracksRotationSpeed * Time.deltaTime * 100 * velocityRate;
        m_aiPath.repathRate = repathRate;
    }

    private void FixedUpdate()
    {

        Vector3 vectorToTarget = new Vector3(m_aiDestinationSetter.target.position.x,m_aiDestinationSetter.target.position.y,towerTransform.transform.position.z)  - towerTransform.transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle -90 , Vector3.forward);
        towerTransform.transform.rotation = Quaternion.Slerp(towerTransform.transform.rotation, q, Time.deltaTime * m_tankController.StatsController.towerRotationSpeed);

        
        
        RaycastHit2D hit = Physics2D.Raycast( new Vector2(spawnBullet.position.x,spawnBullet.position.y), new Vector2(spawnBullet.transform.up.x,spawnBullet.transform.up.y));
        if (hit != false)
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                if (!isReloading)
                {
                    
                    
                    GameObject ammo = Instantiate(bullet, spawnBullet.transform.position, spawnBullet.transform.rotation) as  GameObject;
                    Projectile_Bullet ammoProjectile = ammo.GetComponent<Projectile_Bullet>();
                    ammoProjectile.velocity = m_tankController.StatsController.bulletVelocity;
                    ammoProjectile.parentUp = spawnBullet.transform.up;
                    ammoProjectile.senderTag = gameObject.tag;
                    isReloading = true;
                    StartCoroutine(Reload());
                }
            }
        }
      
    }
    
    
    IEnumerator Reload()
    {
        yield return new WaitForSeconds(m_tankController.StatsController.reloadTimeSpeed);
        isReloading = false;
    }
    

    
}

