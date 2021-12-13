using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class UIEnemyArrow : MonoBehaviour
{
   public GameObject arrowPrefab;
   [Range(1,10)]
   public int redrawRate = 5;
   [Range(1,10)]
   public int redrawSpeed = 2;
   private float m_redrawRate;
   private GameObject m_container;
   private RectTransform m_containerRectTransform;
   private GameObject m_arrowCloned;
   private Quaternion arrowRotation;
   private Vector2 arrowPos;
   private Vector2 m_containerSizeScaled;
 
   private Vector2 offsetPos;
   private bool isVisible = false;


   public void Start()
   {
      m_container = GameObject.FindWithTag("UIEnemyArrowParent");
      m_redrawRate = (float) redrawRate * Time.maximumDeltaTime;
      if (m_container)
      {
         
         m_containerRectTransform = m_container.GetComponent<RectTransform>();
         m_containerSizeScaled = new Vector2((m_containerRectTransform.rect.size.x* m_containerRectTransform.lossyScale.x)/2,(m_containerRectTransform.rect.size.y* m_containerRectTransform.lossyScale.y)/2);
         offsetPos = GetVector(m_container.transform.position);
         arrowPos = new Vector2(m_container.transform.position.x - offsetPos.x, m_container.transform.position.y - offsetPos.y);
        
         Vector3 vectorToTarget = m_container.transform.position  - transform.position;
         float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
         arrowRotation = Quaternion.AngleAxis(angle , Vector3.forward);
         
         m_arrowCloned = Instantiate(arrowPrefab, new Vector3(Mathf.Clamp(this.arrowPos.x , m_container.transform.position.x-m_containerSizeScaled.x,m_container.transform.position.x +m_containerSizeScaled.x), Mathf.Clamp(arrowPos.y,m_container.transform.position.y-m_containerSizeScaled.y,m_container.transform.position.y+m_containerSizeScaled.y), 0), arrowRotation, m_container.transform);
      }
      
      InvokeRepeating("SlowUpdate", 0.0f, m_redrawRate);
      

   }

   private Vector2 GetVector(Vector2 vecPos)
   {
      float valueX = 0;
      float valueY = 0;
      
      if (vecPos.x < transform.position.x)
      {
         valueX = vecPos.x - transform.position.x;
      }
      else
      {
         valueX =(transform.position.x - vecPos.x)*-1  ;
      }
      
     
      if (vecPos.y < transform.position.y)
      {
         valueY = vecPos.y - transform.position.y;
      }
      else
      {
         valueY =(transform.position.y - vecPos.y)*-1  ;
      }

      return new Vector2(valueX, valueY);
   }
   
  
 
 
   void SlowUpdate() {
      if (m_container  && !isVisible)
      {
         
        
         offsetPos = GetVector(m_container.transform.position);
         arrowPos = new Vector2(m_container.transform.position.x - offsetPos.x, m_container.transform.position.y - offsetPos.y);
        
         Vector3 vectorToTarget = m_container.transform.position  - transform.position;
         float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
         arrowRotation = Quaternion.AngleAxis(angle , Vector3.forward);
        
      }
   }
   
  
   private void FixedUpdate()
   {
      m_arrowCloned.transform.position = Vector3.Lerp(  new Vector3(m_arrowCloned.transform.position.x,m_arrowCloned.transform.position.y,0),new Vector3(Mathf.Clamp(this.arrowPos.x , m_container.transform.position.x-m_containerSizeScaled.x,m_container.transform.position.x +m_containerSizeScaled.x), Mathf.Clamp(arrowPos.y,m_container.transform.position.y-m_containerSizeScaled.y,m_container.transform.position.y+m_containerSizeScaled.y), 0),Time.deltaTime *redrawSpeed  );
      m_arrowCloned.transform.rotation = Quaternion.Slerp(m_arrowCloned.transform.rotation, arrowRotation, Time.deltaTime * redrawSpeed  );

      
   }


   

   private void OnDestroy()
   {
      if (m_arrowCloned)
      {
         Destroy(m_arrowCloned);
      }
      
      
   }
}
