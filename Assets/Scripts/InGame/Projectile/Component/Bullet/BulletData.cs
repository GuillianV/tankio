using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Projectile/Bullet")]
public class BulletData : ProjectileScriptableObject
{
    public float damage;
    public int maxBounce;

}
