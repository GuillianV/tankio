using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankDamage : MonoBehaviour
{
    public event EventHandler<DamageEvent> DamageTaken;
        

    public void OnDamageTaken(GameObject target, GameObject sender, float damages)
    {
        DamageTaken?.Invoke(this, new DamageEvent(target,sender,damages));
    }
}
