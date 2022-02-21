using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldEvent : EventArgs
{
    public float Golds { get; private set; }
    public GoldEvent(float _golds)
    {
        Golds = _golds;
    }
}
