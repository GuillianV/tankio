using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/DataList")]
public class BaseScriptableDataList : ScriptableObject
{
   public List<BaseScriptableObject> scriptableDatas;
}
