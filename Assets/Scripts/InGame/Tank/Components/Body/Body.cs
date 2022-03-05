using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TankController))]
public class Body  : BaseData
{
    public BodyData Data { get; private set; }


    //Load Base Data of scriptable object
    public override void LoadData(ScriptableObject _data)
    {
        Data = (BodyData) _data;

    }

    public override ScriptableObject GetBaseData()
    {
        return Data;
    }


}
