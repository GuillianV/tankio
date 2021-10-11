using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    
    public GameObject Bullet;
    public GameObject SpawnBullet;

    [HideInInspector]
    public Gun m_gun { get; private set; }
    
    private SpriteRenderer m_spriteRenderer;

    private bool isReloading = false;

    private void Awake()
    {
        m_gun = GetComponent<Gun>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        m_gun.LoadData(m_gun.Data);
        
        if (m_gun != null)
        {
            transform.position = new Vector3(transform.position.x,
                transform.position.y + m_gun.Data.TowerGunOffset,
                transform.position.z);

            SpawnBullet.transform.position = new Vector3(SpawnBullet.transform.position.x,
                SpawnBullet.transform.position.y + m_gun.Data.GunSpawnOffset,
                SpawnBullet.transform.position.z);
            
            m_spriteRenderer.sprite = m_gun.Data.spriteGun;
            m_spriteRenderer.color = m_gun.Data.color;

        }
    }

    private void FixedUpdate()
    {
        if (Input.GetButton("Fire1"))
        {
            if (!isReloading)
            {
                GameObject rocketInstance;
                rocketInstance = Instantiate(Bullet,SpawnBullet.transform.position,SpawnBullet.transform.rotation) as GameObject;
                Rigidbody2D rocketRb = rocketInstance.GetComponent<Rigidbody2D>();
                rocketRb.AddForce(new Vector2(SpawnBullet.transform.up.x *Time.deltaTime* m_gun.Data.bulletVelocity, SpawnBullet.transform.up.y*Time.deltaTime * m_gun.Data.bulletVelocity ));
                isReloading = true;
                StartCoroutine(Reload());
           
            }
            
            
       
            

        }
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(m_gun.Data.reloadTimeSecond);
        isReloading = false;
    }
}
