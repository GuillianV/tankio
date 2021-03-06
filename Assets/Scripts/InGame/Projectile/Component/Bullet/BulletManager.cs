using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class BulletManager : MonoBehaviour, IBulletManager
{



    public BaseAsset bulletAsset;
    public BaseAnimator bulletAnimator;
    public BulletController bulletController;

    private BulletData bulletData;

    private Rigidbody2D m_projectileRigidbody2D;
    private CircleCollider2D m_projectileCircleCollider2D;
    private GameManager m_Game;
    private TankController tankController;

    private float _BulletVelocity;
    private Vector3 _BulletDirection;
    void IManager.Bind()
    {
       
        if (bulletData)
        {
            m_Game = GameManager.Instance;
            m_projectileRigidbody2D = GetComponent<Rigidbody2D>();
            m_projectileCircleCollider2D = GetComponent<CircleCollider2D>();


            bulletController.BindController(bulletData);
            bulletAsset.BindAssets();
            bulletAnimator.BindAnimators(bulletData.animators);

            _BulletVelocity = bulletController.GetVelocity();
            _BulletDirection = bulletController.GetDirection();
        }

    }

    void IBulletManager.AdditionalBulletData(ScriptableObject data, float Velocity, string senderTag, Vector3 baseDirection)
    {
        BindData(data);

        bulletController.SetDirection(baseDirection);
        bulletController.SetSenderTag(senderTag);
        bulletController.SetVelocity(Velocity);

     

    }


    void BindData(ScriptableObject obj)
    {

        if (obj.GetType() == typeof(BulletData))
        {
            bulletData = (BulletData)obj;
        }


    }

 
  
    private void FixedUpdate()
    {
       
        //41 t
        //new 15 t
        if (m_projectileRigidbody2D != null)
        {
            m_projectileRigidbody2D.velocity = new Vector2(_BulletDirection.x * Time.deltaTime * m_Game.TimeManager.timeScale * _BulletVelocity * 100,
               _BulletDirection.y * Time.deltaTime * m_Game.TimeManager.timeScale * _BulletVelocity * 100);

        }


      
    }


  

    private void OnCollisionEnter2D(Collision2D elementCollided)
    {
        IDamagable damagable = elementCollided.gameObject.GetComponent<IDamagable>();
        if ( damagable != null)
        {
            bulletController.SendDamage(damagable, gameObject.tag);
            StartDestroyAnim();
        }
        else if(bulletController.GetBounce() <= 0)
        {
            StartDestroyAnim();
        }
        else
        {
            Bounce(elementCollided.contacts[0].normal);
            bulletController.SetBounce(bulletController.GetBounce() - 1);
            _BulletVelocity = bulletController.GetVelocity();
            _BulletDirection = bulletController.GetDirection();
        }


    }

    private void Bounce(Vector3 collisionNormal)
    {

        bulletController.SetDirection(Vector2.Reflect(bulletController.GetDirection().normalized, collisionNormal));
        gameObject.transform.rotation = TMath.GetAngleFromVector2D(collisionNormal, -90);
    }


    public void StartDestroyAnim()
    {
        bulletAnimator.CallAnimator("Bullet").SetTrigger("IsDead");
        Destroy(this.m_projectileRigidbody2D);
        Destroy(this.m_projectileCircleCollider2D);
    }

    public void EndDestroyAnim()
    {
        Destroy(gameObject);
    }




}
