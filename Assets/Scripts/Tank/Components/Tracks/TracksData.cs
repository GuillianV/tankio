using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tank/Tracks")]
public class TracksData : ScriptableObject
{



    [Range(0.01f, 1f)]
    public float coefSpeed = 0.1f;
    [Range(0.01f, 1f)]
    public float coefRotationSpeed = 0.1f;
    public float speed;
    public float rotationSpeed;
    public Sprite spriteTrack;
    [Range(-5, 5)]
    public float TracksSpriteScaleX = 2;
    [Range(-5, 5)]
    public float TracksSpriteScaleY = 2;
    public Color color;
}
