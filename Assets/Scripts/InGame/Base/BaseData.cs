using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseData
{
    
    public ScriptableObject Data { get; private set; }
    
    
    //Load Base Data of scriptable object
    public abstract void LoadData(ScriptableObject _data);

    public abstract ScriptableObject GetBaseData();



}
