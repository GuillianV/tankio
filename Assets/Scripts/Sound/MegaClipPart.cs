using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Sound/MegaClipPart")]
public class MegaClipPart : ScriptableObject
{
    public Sound audioClip;
    public int priority;
}
