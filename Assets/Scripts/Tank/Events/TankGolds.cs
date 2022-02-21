using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TankGolds : MonoBehaviour
{
    public event EventHandler<GoldEvent> Golded;


    public void OnGoldEarned(float goldEarned)
    {
        Golded?.Invoke(this, new GoldEvent(goldEarned));
    }
}