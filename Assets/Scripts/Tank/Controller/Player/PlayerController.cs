using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
   protected TankController m_tankController;

    protected virtual void Awake()
   {
       m_tankController = GetComponent<TankController>();
   }
}
