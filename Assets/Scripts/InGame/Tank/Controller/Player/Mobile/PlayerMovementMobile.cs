using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementMobile : PlayerController
{


#if UNITY_ANDROID
    
    [HideInInspector]

    public Rigidbody2D m_playerRigidbody;
    private GameManager m_Game;
    private TracksManager m_tracksManager;
    private TracksController m_tracksController;
    private BaseAnimator m_TracksAnimator;
    [Range(0,5)]
    public float power = 1;
    public InputTank inputTank;
    
    private Vector2 movement = new Vector2();
    private Vector2 rotation = new Vector2();
    private float m_xForce;
    private float m_yForce;

    protected override void Awake()
    {
        base.Awake();
        inputTank = new InputTank();
        inputTank.Enable();
        m_Game = GameManager.Instance;
        m_playerRigidbody = GetComponent<Rigidbody2D>();
        m_tracksManager = m_tankController.GetTankManager<TracksManager>();
        m_TracksAnimator = m_tracksManager.tracksAnimator;
        m_tracksController = m_tracksManager.tracksController;

        if (m_tracksController == null)
            Debug.LogError("Player Movement Mobile missing TracksController");

        inputTank.Tank.Move.canceled += ctx => Cancelled();
    }

    private void OnDestroy()
    {
        inputTank.Tank.Move.canceled -= ctx => Cancelled();
    }


    protected void Cancelled()
    {
        movement = Vector2.zero;
        m_TracksAnimator?.CallAnimator("Tracks-Left")?.SetBool("Moving", false);
        m_TracksAnimator?.CallAnimator("Tracks-Right")?.SetBool("Moving", false);
    }

    void FixedUpdate()
    {
        
            Quaternion q = TMath.GetAngleFromVector2D(rotation, -90);
            this.transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime  * m_Game.TimeManager.timeScale* m_tracksController.GetTrackRotationSpeed()/100);

        
            m_xForce = m_tracksController.GetTrackSpeed() * 
                       Time.deltaTime*
                       power * 
                       m_Game.TimeManager.timeScale  * 
                       transform.up.x
                       * movement.magnitude; 
        
            m_yForce = m_tracksController.GetTrackSpeed()
                       * Time.deltaTime *  m_Game.TimeManager.timeScale*
                       power * 
                       transform.up.y
                       * movement.magnitude; 
        
   
            m_playerRigidbody.velocity = new Vector2(m_xForce,m_yForce);
        

      
    }

    public void OnMove(InputValue input)
    {

        Vector2 inputVec = input.Get<Vector2>();

        if (inputVec.magnitude > 0)
        {

            movement = inputVec;
            rotation = inputVec;

            if(m_Game.TimeManager.timeScale > 0)
            {
                m_TracksAnimator.CallAnimator("Tracks-Left").SetBool("Moving", true);
                m_TracksAnimator.CallAnimator("Tracks-Right").SetBool("Moving", true);
            }
        
        }
        
        
    }
    
#endif
}
