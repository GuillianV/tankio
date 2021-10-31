using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Tank/Body")]
public class BodyData : ScriptableObject
{
    public float life;
    public int golds;
    public Sprite sprite;
    public Color color;
}
