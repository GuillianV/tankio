using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{

    private Projectile m_projectile;


    private void Awake()
    {
        m_projectile = GetComponent<Projectile>();
    }


    private void Start()
    {
       m_projectile.Fire();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        m_projectile.Collided();
        
    }
}
