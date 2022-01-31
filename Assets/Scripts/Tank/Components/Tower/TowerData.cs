using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

[CreateAssetMenu(menuName = "Tank/Tower")]
public class TowerData : ScriptableObject
{
    [Range(1,10)]
    public float rotationSpeed = 1;
    public Sprite spriteTower;
    public Color color;
  
   
}