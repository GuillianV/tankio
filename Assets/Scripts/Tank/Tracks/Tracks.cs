using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracks : MonoBehaviour , IUpgradable
{

    public TracksData Data { get; private set; }

    
    public void LoadData(TracksData _data)
    {
        Data = _data;

    }

    void IUpgradable.Upgrade()
    {
        Debug.Log("Tracks Upgraded");
    }
    
}
