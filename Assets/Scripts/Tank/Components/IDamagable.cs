using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
  

    void TakeDamage(string senderTag, float damages);
}
