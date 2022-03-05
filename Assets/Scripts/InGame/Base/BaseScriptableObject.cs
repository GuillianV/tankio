using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseScriptableObject : ScriptableObject
{
    [Header("Base In game data")]
    public Sprite sprite;
    public Color color;
    public List<BaseAnimatorOverride> animators;
}
