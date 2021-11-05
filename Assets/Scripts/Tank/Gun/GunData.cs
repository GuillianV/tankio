using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tank/Gun")]
public class GunData : ScriptableObject
{
    
    [Range(0,10)]
    public float reloadTimeSecond = 1;
    [Range(0,100)]
    public float bulletVelocity = 1;
    public Sprite spriteGun;
    [Range(-10,10)]
    public float TowerGunOffset;
    [Range(-10,10)]
    public float GunSpawnOffset;
    public Color color;
}
