using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    
    public BulletData Data { get; private set; }

 

    public void LoadData(BulletData _data)
    {
        Data = _data;

    }
}
