using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public TowerData Data { get; private set; }
    
        
    public void LoadData(TowerData _data)
    {
        Data = _data;
    }
}
