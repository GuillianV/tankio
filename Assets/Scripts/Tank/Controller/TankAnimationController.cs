using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAnimationController : MonoBehaviour
{
    
    public Animator m_bulletSpawn_animator;
    private TankController m_tankController;

    public void Awake()
    {
        m_tankController = GetComponent<TankController>();
        if (m_tankController == null)
        {
            Debug.LogWarning("TankController manquant dans tank Animation controller");
        }
        
        if (m_bulletSpawn_animator == null)
        {
            Debug.LogWarning("SpawnBullet animator manquant dans tank Animation controller");
        }
    }
    
    public void FireProjectile()
    {
        m_bulletSpawn_animator.SetTrigger("Fire");
    }
    
}
