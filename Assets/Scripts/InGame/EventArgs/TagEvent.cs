using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagEvent : EventArgs
{
    public string Tag { get; private set; }
    
    
    public TagEvent(String tag)
    {
        Tag = tag;
    }
}
