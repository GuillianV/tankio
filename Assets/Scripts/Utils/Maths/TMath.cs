using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TMath 
{
    public static Quaternion GetAngleFromVector2D(Vector2 direction, int offset)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        return Quaternion.AngleAxis(angle+ offset, Vector3.forward);
    }
}
