using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementMobile : MonoBehaviour
{


#if UNITY_ANDROID
    
    [HideInInspector]

    public Rigidbody2D m_playerRigidbody;
    private TankController m_tankController;
    private GameManager m_Game;
   
    [Range(0,5)]
    public float power = 1;
    public InputTank inputTank;
    
    private Vector2 movement = new Vector2();
    private Vector2 rotation = new Vector2();
    private float m_xForce;
    private float m_yForce;

    public void Awake()
    {
        inputTank = new InputTank();
        inputTank.Enable();
        m_Game = GameManager.Instance;
        m_playerRigidbody = GetComponent<Rigidbody2D>();
        m_tankController = GetComponent<TankController>();



        inputTank.Tank.Move.canceled += ctx => Cancelled();
    }


    protected void Cancelled()
    {
        movement = Vector2.zero;
    }

    void FixedUpdate()
    {
        
        float angle = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle -90, Vector3.forward);
        this.transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime  * m_Game.TimeManager.timeScale* m_tankController.StatsController.towerRotationSpeed);

        
        m_xForce = m_tankController.StatsController.tracksSpeed * 
                   Time.deltaTime*
                   power * 
                   m_Game.TimeManager.timeScale  * 
                   transform.up.x
                   * movement.magnitude; 
        
        m_yForce = m_tankController.StatsController.tracksSpeed
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
        }
        
        
    }
    
#endif
}
