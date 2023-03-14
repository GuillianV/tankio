using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEvent : EventArgs
{
    public string Tag { get; private set; }
    public Vector2 Position { get; private set; }

    
    public MapEvent(String tag, Vector2 position)
    {
        Tag = tag;
        Position = position;
    }
}
