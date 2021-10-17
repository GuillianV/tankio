using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Projectile/Bullet")]
public class BulletData : ScriptableObject
{
    public float damage;
    public float maxBounce;
    
}
