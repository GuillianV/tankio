using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour, ITankManager, IUpgradable
{
    public TankBaseAsset gunAsset ;
    public TankBaseAnimator gunAnimator;
    public GunController gunController;
    
  

    void ITankManager.Bind(ScriptableObject data)
    {

        gunController.BindController(data);
        gunAsset.BindAssets();
        gunAnimator.BindAnimators();
    }

    void IUpgradable.Upgrade()
    {
        gunController.Upgrade();
    }
    
}
