using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour, ITankManager,IUpgradable
{
   
    public TankBaseAsset towerAsset ;
    public TankBaseAnimator towerAnimator;
    public TowerController towerController;
    
  

    void ITankManager.Bind(ScriptableObject data)
    {

        towerController.BindController(data);
        towerAsset.BindAssets();
        towerAnimator.BindAnimators();
    }

    void IUpgradable.Upgrade()
    {
        towerController.Upgrade();
    }
}
