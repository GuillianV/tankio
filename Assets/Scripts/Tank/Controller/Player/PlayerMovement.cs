using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [HideInInspector]

    public Rigidbody2D m_playerRigidbody;
    private TankController m_tankController;
    
    [Range(0,5)]
    public float power = 1;
    
    private float m_xForce;
    private float m_yForce;

    public void Awake()
    {
        m_playerRigidbody = GetComponent<Rigidbody2D>();
        m_tankController = GetComponent<TankController>();
    }


    void FixedUpdate()
    {

      
        
        m_xForce = Input.GetAxis("Vertical") * 
                   m_tankController.StatsController.tracksSpeed * 
                   Time.deltaTime*
                   power * 
                   transform.up.x; 
        
        m_yForce = Input.GetAxis("Vertical") *
                   m_tankController.StatsController.tracksSpeed * Time.deltaTime * 
                   power * 
                   transform.up.y;
        
        transform.Rotate(0,0, 
            -Input.GetAxis("Horizontal") *
            m_tankController.StatsController.tracksRotationSpeed * 
            Time.deltaTime * 
            power );
        
        m_playerRigidbody.velocity = new Vector2(m_xForce,m_yForce);
    }
    

}
