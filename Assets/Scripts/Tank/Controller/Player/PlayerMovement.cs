using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;




public class PlayerMovement : MonoBehaviour
{

#if UNITY_STANDALONE


    [HideInInspector]

    public Rigidbody2D m_playerRigidbody;
    private TankController m_tankController;
    private TracksController TracksController;
    private GameManager m_Game;
   
    [Range(0,5)]
    public float power = 1;
    public InputTank inputTank;
    
    private Vector2 movement = new Vector2();
    private float m_xForce;
    private float m_yForce;


    public void Awake()
    {
        inputTank = new InputTank();
        inputTank.Enable();
        m_Game = GameManager.Instance;
        m_playerRigidbody = GetComponent<Rigidbody2D>();
        m_tankController = GetComponent<TankController>();
        TracksController = GetComponent<TracksController>();
        inputTank.Tank.Move.canceled += ctx => Cancelled();

    }

    protected void Cancelled()
    {
        movement = Vector2.zero;
    }

    void FixedUpdate()
    {

            m_xForce = movement.y * 
                      TracksController.GetTrackSpeed() * 
                       Time.deltaTime*
                       power * 
                       m_Game.TimeManager.timeScale  * 
                       transform.up.x; 
                
            m_yForce = movement.y *
                      TracksController.GetTrackSpeed()
                       * Time.deltaTime *  m_Game.TimeManager.timeScale*
                       power * 
                       transform.up.y;
                
            transform.Rotate(0,0, 
                -movement.x *
                TracksController.GetTrackRotationSpeed() * 
                Time.deltaTime *  m_Game.TimeManager.timeScale *
                power );
       
   
        m_playerRigidbody.velocity = new Vector2(m_xForce,m_yForce);
    }

    public void OnMove(InputValue input)
    {
 
       
        Vector2 inputVec = input.Get<Vector2>();

        if (inputVec.magnitude > 0)
        {

            movement = inputVec;
        }
        
        
    }
    
#endif

}
