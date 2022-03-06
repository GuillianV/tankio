using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tank/Tracks")]
public class TracksData : TankScriptableObject
{



    public float speed;
    public float rotationSpeed;
    [Range(-5, 5)]
    public float TracksSpriteScaleX = 2;
    [Range(-5, 5)]
    public float TracksSpriteScaleY = 2;

}
