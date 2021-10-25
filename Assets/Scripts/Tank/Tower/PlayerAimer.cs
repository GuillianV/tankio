using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerAimer : MonoBehaviour
{
    
  

    private TankController m_tankController;
    public Transform towerTransform;
     

    private void Awake()
    {

        m_tankController = GetComponent<TankController>();
    }


    void FixedUpdate()
    {

            Vector3 pz = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            Vector3 vectorToTarget = new Vector3(pz.x,pz.y,towerTransform.position.z)  - towerTransform.position;
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.AngleAxis(angle -90, Vector3.forward);
            towerTransform.rotation = Quaternion.Slerp(towerTransform.rotation, q, Time.deltaTime * m_tankController.TowerController.tower.Data.rotationSpeed);
            
    }
}
