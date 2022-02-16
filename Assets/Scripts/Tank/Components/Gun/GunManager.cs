using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour, ITankManager, IUpgradable
{
    public TankBaseAsset gunAsset ;
    public TankBaseAnimator gunAnimator;
    public GunController gunController;
    private GunData gunData;
  

    void ITankManager.Bind(ScriptableObject data)
    {
        BindData(data);
        gunController.BindController(gunData);
        gunAsset.BindAssets();
        gunAnimator.BindAnimators(gunData.gunAnimators);
    }

    void IUpgradable.Upgrade()
    {
        gunController.Upgrade();
    }


    void BindData(ScriptableObject obj)
    {

        if (obj.GetType() == typeof(GunData))
        {
            gunData = (GunData)obj;
        }
        else
        {
            Debug.LogError("gunManager cannot load gunData");
        }


    }

}
