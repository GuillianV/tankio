using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [HideInInspector]

    public Rigidbody2D m_playerRigidbody;
    private TankController m_tankController;
    
    [Range(0,5)]
    public float power = 1;
    
    private Vector2 movement = new Vector2();
    private float m_xForce;
    private float m_yForce;

    public void Awake()
    {
        m_playerRigidbody = GetComponent<Rigidbody2D>();
        m_tankController = GetComponent<TankController>();
    }


    void FixedUpdate()
    {

      
       
        m_xForce = movement.y * 
                   m_tankController.StatsController.tracksSpeed * 
                   Time.deltaTime*
                   power * 
                   transform.up.x; 
        
        m_yForce = movement.y *
                   m_tankController.StatsController.tracksSpeed * Time.deltaTime * 
                   power * 
                   transform.up.y;
        
        transform.Rotate(0,0, 
            -movement.x *
            m_tankController.StatsController.tracksRotationSpeed * 
            Time.deltaTime * 
            power );
            
        
        m_playerRigidbody.velocity = new Vector2(m_xForce,m_yForce);
    }

    public void OnMove(InputValue input)
    {
        Vector2 inputVec = input.Get<Vector2>();

        movement = new Vector2(inputVec.x, inputVec.y);
    }
    
   

}
