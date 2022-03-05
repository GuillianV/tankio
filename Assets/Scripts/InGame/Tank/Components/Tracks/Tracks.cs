using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TankController))]
public class Tracks : BaseData 
{
    
    
    public TracksData Data { get; private set; }
 
    //Load Base Data of scriptable object
    public override void LoadData(ScriptableObject _data)
    {
        Data = (TracksData) _data;

    }

    public override ScriptableObject GetBaseData()
    {
        return Data;
    }


    
}
