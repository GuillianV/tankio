using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{

    private Bullet m_bullet;


    private void Awake()
    {
        m_bullet = GetComponent<Bullet>();
    }

    
    private void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject);
    }
}
