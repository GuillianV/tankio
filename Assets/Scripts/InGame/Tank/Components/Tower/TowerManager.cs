using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour, IManager,IUpgradable
{
   
    public BaseAsset towerAsset ;
    public BaseAnimator towerAnimator;
    public TowerController towerController;
    private TowerData towerData;
  

    void IManager.Bind(ScriptableObject data)
    {
        BindData(data);
        towerController.BindController(towerData);
        towerAsset.BindAssets();
        towerAnimator.BindAnimators(towerData.animators);
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
