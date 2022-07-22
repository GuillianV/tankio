using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : BaseData 
{
    
    
    public GoldData Data { get; private set; }
 
    //Load Base Data of scriptable object
    public override void LoadData(ScriptableObject _data)
    {
        Data = (GoldData) _data;

    }

    public override ScriptableObject GetBaseData()
    {
        return Data;
    }


    
}
