using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankDamage : MonoBehaviour
{
    public event EventHandler<DamageEvent> Damaged;
        

    public void OnDamageTaken(GameObject target, string sender, float damages)
    {
        Damaged?.Invoke(this, new DamageEvent(target,sender,damages));
    }
}
