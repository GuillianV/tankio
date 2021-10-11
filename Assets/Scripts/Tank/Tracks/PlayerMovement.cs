using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [HideInInspector]
    public Tracks m_tracks { get; private set; }
    
    public SpriteRenderer trackRightSprite;
    public SpriteRenderer trackLeftSprite;

    public Rigidbody2D playerRigidbody;
    public GameObject player;

    [Range(0,5)]
    public float power = 1;
    
    private float m_xForce;
    private float m_yForce;
    private void Awake()
    {
        m_tracks = GetComponent<Tracks>();
    }

    void Start()
    {

        m_tracks.LoadData(m_tracks.Data);
  
        if (m_tracks.Data != null)
        {
            if (trackLeftSprite != null)
            {
                trackLeftSprite.sprite = m_tracks.Data.spriteTrack;
                trackLeftSprite.color = m_tracks.Data.color;
            }
        
            if (trackRightSprite != null)
            {
                trackRightSprite.sprite = m_tracks.Data.spriteTrack;
                trackRightSprite.color = m_tracks.Data.color;
            }

        }
    }
    
    void FixedUpdate()
    {

        m_xForce = Input.GetAxis("Vertical") * 
                   m_tracks.Data.speed * 
                   Time.deltaTime*
                   power * 
                   player.transform.up.x; 
        
        m_yForce = Input.GetAxis("Vertical") *
                   m_tracks.Data.speed * Time.deltaTime * 
                   power * 
                   player.transform.up.y;

        
        
 
        player.transform.Rotate(0,0, 
            -Input.GetAxis("Horizontal") *
            m_tracks.Data.rotationSpeed * 
            Time.deltaTime * 
            power );
        
        playerRigidbody.velocity = new Vector2(m_xForce,m_yForce);
    }
    

}
