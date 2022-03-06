using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBulletManager : IManager
{
    void AdditionalBulletData(float Velocity, string senderTag, Vector3 baseDirection,int upgradeLevel);

}
