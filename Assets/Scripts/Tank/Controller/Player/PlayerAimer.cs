using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerAimer : MonoBehaviour
{
    
  

    private TankController m_tankController;
    public Transform towerTransform;

    private Vector3 mousePosition;
    private GameManager m_Game;
    private void Awake()
    {
        m_Game = GameManager.Instance;
        m_tankController = GetComponent<TankController>();
    }


    void FixedUpdate()
    {

            Vector3 pz = Camera.main.ScreenToWorldPoint(mousePosition);
            
            Vector3 vectorToTarget = new Vector3(pz.x,pz.y,towerTransform.position.z)  - towerTransform.position;
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.AngleAxis(angle -90, Vector3.forward);
            towerTransform.rotation = Quaternion.Slerp(towerTransform.rotation, q, Time.deltaTime  * m_Game.TimeManager.timeScale* m_tankController.StatsController.towerRotationSpeed);
          
    }


    public void OnMouseMove(InputValue mousePositionReference)
    {


        mousePosition = new Vector3(mousePositionReference.Get<Vector2>().x, mousePositionReference.Get<Vector2>().y,
            0);

    }
}
