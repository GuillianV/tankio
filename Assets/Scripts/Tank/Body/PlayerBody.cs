using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBody : MonoBehaviour
{
    private TankController m_tankController;


  
    private void Awake()
    {
        m_tankController = GetComponent<TankController>();
    }
    
}
