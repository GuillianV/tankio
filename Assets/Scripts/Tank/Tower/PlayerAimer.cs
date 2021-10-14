using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerAimer : MonoBehaviour
{
    
  
    private Quaternion _lookRotation;
    private Vector2 _direction;
    private SpriteRenderer m_towerSprite;
     
    [HideInInspector]
    public Tower m_tower { get; private set; }

    private void Awake()
    {
        m_tower = GetComponent<Tower>();
        m_towerSprite = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        m_tower.LoadData(m_tower.Data);
        
        if (m_tower.Data != null)
        {
           

            m_towerSprite.sprite = m_tower.Data.spriteTower;
                
                m_towerSprite.color = m_tower.Data.color;
      
        }
    }
    
    void FixedUpdate()
    {

            Vector3 pz = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            Vector3 vectorToTarget = new Vector3(pz.x,pz.y,transform.position.z)  - transform.position;
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.AngleAxis(angle -90, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * m_tower.Data.rotationSpeed);
            
    }
}
