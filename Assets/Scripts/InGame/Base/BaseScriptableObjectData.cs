using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseScriptableObjectData
{
    [HideInInspector]
    public int upgradeLevel;
    [Header("Upgrades by order")]
    public BaseScriptableDataList dataList;
    [TextArea(5, 10)]
    public string description;
    public List<string> descriptionAttributesName = new List<string>();
}

