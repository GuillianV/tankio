using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEnemyArrow : MonoBehaviour
{
   public GameObject arrowPrefab;
   private GameObject arrowParentContainer;
   private GameObject arrowCloned;
   private GameObject player;
   public void Start()
   {
      arrowParentContainer = GameObject.FindWithTag("UIEnemyArrowParent");
      player = GameObject.FindWithTag("Player");
      if (arrowParentContainer)
      {
         arrowCloned = Instantiate(arrowPrefab, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0), arrowParentContainer.transform);
      }
      
   }


   private void FixedUpdate()
   {
      
   }

   private void OnDestroy()
   {
      if (arrowCloned)
      {
         Destroy(arrowCloned);
      }
      
      
   }
}
