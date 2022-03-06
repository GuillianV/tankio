using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.InputSystem.OnScreen;

public class PlayerAimerShooterMobile : PlayerController 
{



#if UNITY_ANDROID

    private GameObject projectile;
    private Transform towerTransform;

    private TowerManager m_towerManager;
    private TowerController m_towerController;
    private BaseAsset m_towerAsset;
    private GunManager m_gunManager;
    private GunController m_gunController;
    private BaseAsset m_gunAsset;
    private GameManager m_Game;
    private Vector2 vectorToTarget = new Vector2(0,0);
    private bool isReloading = false;
    private bool isFireing = false;
    public InputTank inputTank;
    private OnScreenStickHandler onScreenStickHandler;


    protected override void Awake()
    {
        base.Awake();
        inputTank = new InputTank();
        inputTank.Enable();

        m_gunManager = m_tankController.GetTankManager<GunManager>();
        m_gunController = m_gunManager.gunController;
        m_gunAsset = m_gunManager.gunAsset;
        if (m_gunController==null)
            Debug.LogError("Player Aimer Shooter Mobile missing GunController");
        
        m_towerManager = m_tankController.GetTankManager<TowerManager>();
        m_towerController = m_towerManager.towerController;
        m_towerAsset = m_towerManager.towerAsset;
        if (m_towerController ==null)
            Debug.LogError("Player Aimer Shooter Mobile missing TowerController");

        m_Game = GameManager.Instance;
        inputTank.Tank.FireGameStick.canceled += ctx => Cancelled();
      
    }

    private void Start()
    {
        onScreenStickHandler = GameObject.FindGameObjectWithTag("FireUIMobile").GetComponent<OnScreenStickHandler>();
        onScreenStickHandler.OnTap += Taped;
        towerTransform = m_towerAsset.CallAsset("Tower").transform;
        projectile = m_gunAsset.CallAsset("Projectile");
    }


    void FixedUpdate()
    {

       
            Quaternion q = TMath.GetAngleFromVector2D(vectorToTarget, -90);
            towerTransform.rotation = Quaternion.Slerp(towerTransform.rotation, q, Time.deltaTime  * m_Game.TimeManager.timeScale* m_towerController.GetTowerRotationSpeed());

    }


    public void Update()
    {
        if (isFireing)
        {
             Fire();
        }
    }

    protected void Taped(object sender, EventArgs eventArgs)
    {
        Fire();
    }

    protected void Cancelled()
    {
        isFireing = false;
    }

    
    IEnumerator Reload()
    {
        
       
            yield return new WaitForSeconds(m_gunController.GetReloadTimeSpeed());
            isReloading = false;
        
        
    
    }

    

    
    public void Fire()
    {
        
            if (!isReloading)
            {
    
                if (m_Game.TimeManager.timeScale > 0)
                {
                    m_gunManager.Shoot();
                 
                
                    isReloading = true;
                    m_Game.Audio.Play("tank-shoot-"+ UnityEngine.Random.Range(1, 3));
                
                    StartCoroutine(Reload());
                }
            
    
            }
            
        
       
    }

    
    public void OnFireGameStick(InputValue input)
    {
        Vector2 inputVec = input.Get<Vector2>();

        if (inputVec.magnitude > 0)
        {

            vectorToTarget = inputVec;
        }
        
        if (inputVec.magnitude > 1)
        {

            isFireing = true;
        }
    }

#endif

}
