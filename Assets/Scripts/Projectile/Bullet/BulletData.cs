using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Projectile/Bullet")]
public class BulletData : ScriptableObject
{
    public int damage;
    public int maxBounce;
    public Color color;
}
