using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

[CreateAssetMenu(menuName = "Tank/Tower")]
public class TowerData : TankScriptableObject
{
    [Range(0.01f, 1f)]
    public float coefRotationSpeed = 0.1f;
    [Range(1, 10)]
    public float rotationSpeed = 1;

}
