using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Tank/Body")]
public class BodyData : TankScriptableObject
{
    [Header("Additionnal tank data")]
    public float life;
    public int golds;
}
