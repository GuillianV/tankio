using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

[RequireComponent(typeof(ShadowCaster2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class ShadowUpdater2D : MonoBehaviour
{
    private ShadowCaster2D _shadows;
    private SpriteRenderer _renderer;
    private Animator _animator;
   
    void Start()
    {
        _shadows = GetComponent<ShadowCaster2D>();
        _renderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        
        
        if (!_renderer.isVisible)
        {
            ToggleGameObject(false);
        }

     
        
    }

    private void OnBecameInvisible()
    {
        ToggleGameObject(false);
    }

    private void OnBecameVisible()
    {
        ToggleGameObject(true);
    }

    public void ToggleGameObject(bool value)
    {
        //_renderer.enabled = value;
        _shadows.enabled = value;
        
        if (_animator)
        {
            _animator.enabled = value;
        } 
    }
    
}
