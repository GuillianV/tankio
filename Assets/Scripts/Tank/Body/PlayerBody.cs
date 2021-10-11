using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBody : MonoBehaviour
{
    [HideInInspector]
    public Body m_body { get; private set; }
    
    private SpriteRenderer m_spriteRender;
    
    // Start is called before the first frame update

    private void Awake()
    {
        m_body = GetComponent<Body>();
        m_spriteRender = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        m_body.LoadData(m_body.Data);
        
        if (m_body.Data != null)
        {
            if (m_spriteRender != null)
            {
                m_spriteRender.sprite = m_body.Data.spriteTrack;
                m_spriteRender.color = m_body.Data.color;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
