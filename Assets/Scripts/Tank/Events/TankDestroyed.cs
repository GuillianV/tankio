using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankDestroyed : MonoBehaviour
{

    public event EventHandler<EventArgs> Destroyed;

    private void OnDestroy()
    {
        OnDestroyed();
    }

    private void OnDestroyed()
    {
        Destroyed?.Invoke(this, EventArgs.Empty);
    }
}
