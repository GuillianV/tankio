using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerBody : PlayerController, IPlayerController
{
    private BodyController m_bodyController;
    private BodyManager m_bodyManager;
    private GameManager m_Game;

  
    protected override void Awake()
    {
        base.Awake();
        m_bodyManager = m_tankController.GetTankManager<BodyManager>();
        m_bodyController = m_bodyManager.bodyController;
        if (m_bodyController == null)
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
