using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracks : MonoBehaviour
{

    public TracksData Data { get; private set; }

    
    public void LoadData(TracksData _data)
    {
        Data = _data;

    }
    
}
