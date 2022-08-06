using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoolEventargs : EventArgs
{
    public bool Value { get; private set; }
    public BoolEventargs(bool _value)
    {
        Value = _value;
    }
}