using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Sound/MegaClip")]
public class MegaClip : ScriptableObject
{
    public List<MegaClipPart> megaClips;

    public string name;

    [HideInInspector]
    public int actualPriority { get;  set; }
    [HideInInspector]
    public int minPriority { get;  set; }
    [HideInInspector]
    public int maxPriority { get;  set; }

}
