using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankCreate : MonoBehaviour
{
    // Start is called before the first frame update
    public event EventHandler<EventArgs> Created;

   

    public void OnCreated()
    {
        Created?.Invoke(this, EventArgs.Empty);
    }
}
