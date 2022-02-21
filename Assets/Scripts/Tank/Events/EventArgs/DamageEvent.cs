using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEvent : EventArgs
{
    public GameObject TargetGameObject { get; private set; }
    public GameObject SenderGameObject { get; private set; }
    public float DamageTaken { get; private set; }
    public DamageEvent(GameObject _targetGameObject, GameObject _senderGameObject, float _damageTaken)
    {
        TargetGameObject = _targetGameObject;
        SenderGameObject = _senderGameObject;
        DamageTaken = _damageTaken;
    }
}

