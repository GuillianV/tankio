using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tank/Tracks")]
public class TracksData : ScriptableObject
{
    
    public float speed;
    public float rotationSpeed;
    public Sprite spriteTrack;
    public Color color;
}
