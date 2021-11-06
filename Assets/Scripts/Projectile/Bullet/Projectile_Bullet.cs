using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BulletStats
{
    
    
    public float velocity;
    

    public int bounces;

    public int damages;

}

public class Projectile_Bullet : MonoBehaviour
{


    private Rigidbody2D m_projectileRigidbody2D;
    private Animator m_projectileAnimator;
    private CircleCollider2D m_projectileCircleCollider2D;
    private SpriteRenderer m_spriteRenderer;
    
    public BulletStats BulletStats;

    public BulletData bulletData;
    [HideInInspector]
    public string senderTag;
    [HideInInspector]
    public Vector3 parentUp;
    
    public Bullet bullet { get; private set; }
    

    private void Awake()
    {
        this.m_projectileRigidbody2D = GetComponent<Rigidbody2D>();
        this.m_projectileAnimator = GetComponent<Animator>();
        this.m_projectileCircleCollider2D = GetComponent<CircleCollider2D>();
        this.m_spriteRenderer = GetComponent<SpriteRenderer>();
        this.bullet = GetComponent<Bullet>();
        
        LoadData(bulletData);
        


    }

    public void LoadData(BulletData _data)
    {
        bullet.LoadData(_data);
        BindBulletData();
    }

    private void BindBulletData()
    {
        m_spriteRenderer.color = bullet.Data.color;
        BulletStats.bounces = bullet.Data.maxBounce;
        BulletStats.damages = bullet.Data.damage;
        

    }
    

    private void Start()
    {
        this.m_projectileRigidbody2D.AddForce(new Vector2(this.parentUp.x *Time.deltaTime* this.BulletStats.velocity, this.parentUp.y *Time.deltaTime * this.BulletStats.velocity ));
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (BulletStats.bounces <= 0 || other.collider.gameObject.layer == LayerMask.NameToLayer("Destructible"))
        {
            StartDestroyAnim();
        }
        else
        {
            BulletStats.bounces--;
        }
        
        
    }

    
    public void StartDestroyAnim()
    {
        m_projectileAnimator.SetTrigger("IsDead");
        Destroy(this.m_projectileRigidbody2D);
        Destroy(this.m_projectileCircleCollider2D);
    }

    public void EndDestroyAnim()
    {
        Destroy(gameObject);
    }

    
    
}
