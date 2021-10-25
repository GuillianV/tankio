using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    
    public GunData Data { get; private set; }
    
        
    public void LoadData(GunData _data)
    {
        Data = _data;
    }
}
