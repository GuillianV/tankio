using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBody : MonoBehaviour
{
    private TankController m_tankController;
    private BodyController m_bodyController;
    private GameManager m_Game;

  
    private void Awake()
    {
        m_tankController = GetComponent<TankController>();
        
        m_bodyController = m_tankController.GetTankComponent<BodyController>();
        if (!m_bodyController)
            Debug.LogError("Player Body missing BodyController");

        m_Game = GameManager.Instance;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {

       
            Projectile_Bullet bullet = collision.gameObject.GetComponent<Projectile_Bullet>();
            if (bullet != null)
            {
                m_bodyController.SetHealt(m_bodyController.GetHealt() - bullet.BulletStats.damages);
                m_Game.Ui.SetLifeUI(m_bodyController.GetMaxHealt(),m_bodyController.GetHealt());
            }
      
        
       
    }
}
