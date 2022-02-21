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
    private GameManager m_Game;
    public BulletStats BulletStats;

    public BulletData bulletData;
    [HideInInspector]
    public string senderTag;
    [HideInInspector]
    public Vector3 parentUp;
    
    public Bullet bullet { get; private set; }

    
    private Vector2 direction;
    private void Awake()
    {
        m_Game = GameManager.Instance;
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
        direction = parentUp;

            

       
    }

    private void FixedUpdate()
    {
        if (this.m_projectileRigidbody2D != null)
        {
            this.m_projectileRigidbody2D.velocity = new Vector2(this.direction.x * Time.deltaTime * m_Game.TimeManager.timeScale * this.BulletStats.velocity * 100,
                this.direction.y * Time.deltaTime  * m_Game.TimeManager.timeScale * this.BulletStats.velocity * 100);

        }
       
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (BulletStats.bounces <= 0 || other.collider.gameObject.layer == LayerMask.NameToLayer("Destructible"))
        {
            IDamagable damagable = other.gameObject.GetComponent<IDamagable>();
            if (damagable != null)
                damagable.TakeDamage(gameObject.tag, BulletStats.damages);

            StartDestroyAnim();
        }
        else
        {
            Bounce(other.contacts[0].normal);
            BulletStats.bounces--;
        }
        
        
    }
    
    private void Bounce(Vector3 collisionNormal)
    {
        
        direction = Vector2.Reflect(direction.normalized, collisionNormal);
        gameObject.transform.rotation = TMath.GetAngleFromVector2D(collisionNormal,-90);
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
