using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public BulletData Data;
    public LayerMask entityLayer;
    
    private Animator m_animator;
    private Rigidbody2D m_rigidBody;
    private PolygonCollider2D m_polyCollider;

  
    
    private float bounce;
    public void LoadData(BulletData _data)
    {
        Data = _data;
    }

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_polyCollider = GetComponent<PolygonCollider2D>();
    }

    private void Start()
    {
        gameObject.SetActive(true);
        bounce = Data.maxBounce;
        
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        
        //Bullet collide with Entity
        if (col.collider.gameObject.layer == entityLayer)
        {
            StartDestroyAnim();
            return;
        }
        
        //Bullet collide with obstacle
        if (bounce <= 0)
        {
            StartDestroyAnim();
        }else
        {
            bounce--;
        }
   
    }


    
    public void StartDestroyAnim()
    {
        m_animator.SetTrigger("IsDead");
        Destroy(m_rigidBody);
        Destroy(m_polyCollider);
    }

    public void EndDestroyAnim()
    {
        Destroy(gameObject);
    }
   
    
 





    
    
}


