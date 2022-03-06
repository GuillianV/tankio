using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BulletController 
{





    public SpriteRenderer bulletSprite;
    private Bullet m_bullet = new Bullet();
    private float damage;
    private int maxBounce;
    private int bounce;

    private float velocity;
    private string senderTag;
    private Vector3 direction;

    public void BindController(BulletData bulletData)
    {
        m_bullet.LoadData(bulletData);
        BindComponent();
        BindStats();
    }



    void BindComponent()
    {
        if (m_bullet.Data != null)
        {


            if (bulletSprite != null && m_bullet != null)
            {
                bulletSprite.color = m_bullet.Data.color;
                bulletSprite.sprite = m_bullet.Data.sprite;
            }
        }
        else
        {
            Debug.LogError("BulletController cannot load Data in Bullet");
        }
    }

    void BindStats()
    {
        if (m_bullet.Data != null)
        {
            damage = m_bullet.Data.damage;
            maxBounce = m_bullet.Data.maxBounce;
            bounce = maxBounce;

        }
        else
        {
            Debug.LogError("BulletController cannot load Data in Bullet");
        }
    }


    public void Upgrade()
    {

        if (m_bullet.Data != null)
        {

            SetDamage(GetDamage() + (m_bullet.Data.coefDamage * m_bullet.Data.damage));

        }
    }


    public void SendDamage(IDamagable damagable, string tag)
    {
        damagable.TakeDamage(tag, GetDamage());
    }



        public void SetMaxBounce(int newValue)
    {
        maxBounce = newValue;
    }

    public int GetMaxBounce()
    {
        return maxBounce;
    }



    public void SetBounce(int newValue)
    {
        bounce = newValue;
    }

    public int GetBounce()
    {
        return bounce;
    }



    public void SetDamage(float newValue)
    {
        damage = newValue;
    }

    public float GetDamage()
    {
        return damage;
    }


    public void SetVelocity(float newValue)
    {
        velocity = newValue;
    }

    public float GetVelocity()
    {
        return velocity;
    }

    public void SetSenderTag(string newValue)
    {
        senderTag = newValue;
    }

    public string GetSenderTag()
    {
        return senderTag;
    }

    public void SetDirection(Vector3 newValue)
    {
        direction = newValue;
    }

    public Vector3 GetDirection()
    {
        return direction;
    }

}
