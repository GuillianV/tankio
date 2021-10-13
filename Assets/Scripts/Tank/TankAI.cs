using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class TankAI : MonoBehaviour
{
    private AIPath m_aiPath;
    private TankController m_tankController;

    [Range(0.1f,20f)]
    public float repathRate = 1;
    [Range(0,10f)]
    public float velocityRate = 1;
    
    private void Awake()
    {
        m_aiPath = GetComponent<AIPath>();
        m_tankController = GetComponent<TankController>();
    }


    private void Start()
    {
        m_aiPath.maxSpeed = m_tankController.tracks.Data.speed * Time.deltaTime * velocityRate;
        m_aiPath.maxAcceleration = m_tankController.tracks.Data.speed * Time.deltaTime * velocityRate;
        m_aiPath.rotationSpeed = m_tankController.tracks.Data.rotationSpeed * Time.deltaTime * 100 * velocityRate;
        
        m_aiPath.repathRate = repathRate;
    }

    private void FixedUpdate()
    {

    }
}

