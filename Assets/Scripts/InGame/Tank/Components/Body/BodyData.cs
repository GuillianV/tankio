using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Tank/Body")]
public class BodyData : TankScriptableObject
{
    [Header("Additionnal tank data")]
    [Range(0.01f, 1f)]
    public float coefLife = 0.1f;
    public float life;
    [Range(0.01f, 1f)]
    public float coefGold = 0.1f;
    public int golds;
}