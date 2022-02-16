using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour, ITankManager,IUpgradable
{
   
    public TankBaseAsset towerAsset ;
    public TankBaseAnimator towerAnimator;
    public TowerController towerController;
    private TowerData towerData;
  

    void ITankManager.Bind(ScriptableObject data)
    {
        BindData(data);
        towerController.BindController(towerData);
        towerAsset.BindAssets();
        towerAnimator.BindAnimators(towerData.towerAnimators);
    }

    void IUpgradable.Upgrade()
    {
        towerController.Upgrade();
    }

    void BindData(ScriptableObject obj)
    {

        if (obj.GetType() == typeof(TowerData))
        {
            towerData = (TowerData)obj;
        }
        else
        {
            Debug.LogError("towerManager cannot load towerData");
        }


    }
}
