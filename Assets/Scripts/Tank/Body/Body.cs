using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour ,IUpgradable
{
    public BodyData Data { get; private set; }

    
    public void LoadData(BodyData _data)
    {
        Data = _data;

    }
    
    void IUpgradable.Upgrade()
    {
        Debug.Log("Body Upgraded");
    }
}
