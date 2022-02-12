using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;




public class PlayerMovement : PlayerController
{

#if UNITY_STANDALONE


    [HideInInspector]

    private Rigidbody2D m_playerRigidbody;
    private TracksController TracksController;
    private GameManager m_Game;
    private TankBaseAnimator m_TracksAnimator;
    
    [Range(0,5)]
    public float power = 1;
    public InputTank inputTank;
    
    private Vector2 movement = new Vector2();
    private float m_xForce;
    private float m_yForce;


    protected override void Awake()
    {
        base.Awake();
        inputTank = new InputTank();
        inputTank.Enable();
        m_Game = GameManager.Instance;
        m_playerRigidbody = GetComponent<Rigidbody2D>();
        TracksController = m_tankController.GetTankManager<TracksManager>().tracksController;
        m_TracksAnimator =  m_tankController.GetTankManager<TracksManager>().tracksAnimator;
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
            m_TracksAnimator.CallAnimator("Tracks-Left").SetBool("Moving",true);
            m_TracksAnimator.CallAnimator("Tracks-Right").SetBool("Moving",true);
            movement = inputVec;
        }
        else
        {
            m_TracksAnimator.CallAnimator("Tracks-Left").SetBool("Moving",false);
            m_TracksAnimator.CallAnimator("Tracks-Right").SetBool("Moving",false);
        }
        
        
    }
    
#endif

}
