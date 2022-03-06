using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ProjectileEvent : EventArgs
{
    public GameObject Projectile { get; private set; }
    public string Tag { get; private set; }
    
    public ProjectileEvent(GameObject projectile,string tag)
    {
        Projectile = projectile;
        Tag = tag;
    }
}
