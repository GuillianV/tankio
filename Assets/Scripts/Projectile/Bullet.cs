using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public BulletData Data;
    private Animator m_animator;
    private Rigidbody2D m_rigidBody;
    private PolygonCollider2D m_polyCollider;
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
    }

    private void OnCollisionEnter2D(Collision2D other)
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


