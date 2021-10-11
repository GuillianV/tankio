using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{


    public GameObject player;
    private PlayerController m_controller;
    
    // Start is called before the first frame update

    private void Awake()
    {
        m_controller = player.GetComponent<PlayerController>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
