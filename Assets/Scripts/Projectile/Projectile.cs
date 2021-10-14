using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{

    public abstract void Destroy();

    public abstract void Collided();

    public abstract void Fire();

}
