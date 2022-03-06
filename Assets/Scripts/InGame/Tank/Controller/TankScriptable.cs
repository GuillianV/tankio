using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseScriptableObjectData
{
    [HideInInspector]
    public int upgradeLevel;
    [Header("Upgrades by order")]
    public List<BaseScriptableObject> listScriptableObjectUpgrade; 

}



[System.Serializable]
public class TankScriptable
{
   //public List<TankScriptableObject> listTankScriptableObject;

   // public List<ProjectileScriptableObject> listProjectileScriptableObject;

    public List<BaseScriptableObjectData> baseScriptableObjects;


}
