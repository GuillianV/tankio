using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankEvent : EventArgs
{
    public TankController TankController { get; private set; }
    
    
    public TankEvent(TankController tankController)
    {
        TankController = tankController;
    }
}
