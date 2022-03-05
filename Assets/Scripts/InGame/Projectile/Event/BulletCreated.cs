using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCreated : MonoBehaviour
{
    public event EventHandler<EventArgs> Created;

    private void Start()
    {
        OnCreated();
    }

    private void OnCreated()
    {
        Created?.Invoke(this, EventArgs.Empty);
    }
}
