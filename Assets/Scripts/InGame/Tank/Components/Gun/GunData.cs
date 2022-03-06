using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tank/Gun")]
public class GunData : TankScriptableObject
{

    [Range(0,10)]
    public float reloadTimeSecond = 1;
    [Range(0,100)]
    public float bulletVelocity = 1;
    [Range(-10,10)]
    public float TowerGunOffset;
    [Range(-10,10)]
    public float GunSpawnOffset;
    [Range(-3, 3)]
    public float GunScaleX;
    [Range(-3, 3)]
    public float GunScaleY;
}
