using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Bullet : MonoBehaviour
{


    private Rigidbody2D m_projectileRigidbody2D;
    private Animator m_projectileAnimator;
    private PolygonCollider2D m_projectilePolygonCollider2D;
    
    [HideInInspector]
    public Vector3 parentUp;
    [HideInInspector]
    public float velocity;
    [HideInInspector]
    public string senderTag;


    private void Awake()
    {
        this.m_projectileRigidbody2D = GetComponent<Rigidbody2D>();
        this.m_projectileAnimator = GetComponent<Animator>();
        this.m_projectilePolygonCollider2D = GetComponent<PolygonCollider2D>();
    }

    private void Start()
    {
        this.m_projectileRigidbody2D.AddForce(new Vector2(this.parentUp.x *Time.deltaTime* this.velocity, this.parentUp.y *Time.deltaTime * this.velocity ));
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        StartDestroyAnim();
    }

    
    public void StartDestroyAnim()
    {
        m_projectileAnimator.SetTrigger("IsDead");
        Destroy(this.m_projectileRigidbody2D);
        Destroy(this.m_projectilePolygonCollider2D);
    }

    public void EndDestroyAnim()
    {
        Destroy(gameObject);
    }

    
    
}
