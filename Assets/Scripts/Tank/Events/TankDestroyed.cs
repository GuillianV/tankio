using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankDestroyed : MonoBehaviour
{

    public event EventHandler<TagEvent> Destroyed;

    private void OnDestroy()
    {
        OnDestroyed("");
    }

    public void OnDestroyed(string tag)
    {
        Destroyed?.Invoke(this, new TagEvent(tag));
    }
    
    
}
