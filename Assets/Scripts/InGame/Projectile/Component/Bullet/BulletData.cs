using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Projectile/Bullet")]
public class BulletData : ProjectileScriptableObject
{
    [Range(0.01f, 1f)]
    public float coefDamage = 0.1f;
    public float damage;
    public int maxBounce;

}
