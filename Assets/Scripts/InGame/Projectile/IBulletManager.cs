using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBulletManager : IManager
{
    void AdditionalBulletData(ScriptableObject data, float Velocity, string senderTag, Vector3 baseDirection);

}
