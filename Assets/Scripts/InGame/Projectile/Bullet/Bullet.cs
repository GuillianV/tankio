using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : BaseData
{
    
    public BulletData Data { get; private set; }

    public override ScriptableObject GetBaseData()
    {
        return Data;
    }


    public override void LoadData(ScriptableObject _data)
    {
        Data =(BulletData) _data;
    }


}
