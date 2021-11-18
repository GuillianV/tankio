using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBody : MonoBehaviour
{
    private TankController m_tankController;
    private GameManager m_Game;

  
    private void Awake()
    {
        m_tankController = GetComponent<TankController>();
        m_Game = GameManager.Instance;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Projectile_Bullet bullet = collision.gameObject.GetComponent<Projectile_Bullet>();
        if (bullet != null)
        {
            m_tankController.StatsController.health -= bullet.BulletStats.damages;
            m_Game.Ui.SetLifeUI(m_tankController.StatsController.maxHealth,m_tankController.StatsController.health);
        }
    }
}
