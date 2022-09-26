using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Sound/Playlist")]
public class PlayList : ScriptableObject
{
    public List<MegaClip> clips;
    [Range(0,1)]
    public float volume = 0.5f;
    [Range(-1,1)]
    public float pitch = 0;
    [HideInInspector]
    public AudioSource source;
    public bool isRandom;
}