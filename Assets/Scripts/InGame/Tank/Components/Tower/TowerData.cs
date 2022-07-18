using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

[CreateAssetMenu(menuName = "Tank/Tower")]
public class TowerData : TankScriptableObject
{
    [Range(1, 30)]
    public float rotationSpeed = 1;

}
