using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BulletEvent : EventArgs
{
    public Bullet Bullet { get; private set; }
    public string Tag { get; private set; }
    
    public BulletEvent(Bullet bullet,string tag)
    {
        Bullet = bullet;
        Tag = tag;
    }
}
