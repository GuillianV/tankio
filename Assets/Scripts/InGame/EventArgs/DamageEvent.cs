using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEvent : EventArgs
{
    public GameObject TargetGameObject { get; private set; }
    public string SenderTag { get; private set; }
    public float DamageTaken { get; private set; }
    public DamageEvent(GameObject _targetGameObject, string _senderTag, float _damageTaken)
    {
        TargetGameObject = _targetGameObject;
        SenderTag = _senderTag;
        DamageTaken = _damageTaken;
    }
}

