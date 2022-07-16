using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour, IManager
{
   
    public BaseAsset towerAsset ;
    public BaseAnimator towerAnimator;
    public TowerController towerController;
    [TextArea(5, 10)]
    public string description;
    private TowerData towerData;
    private TankController tankController;
  

    void IManager.Bind()
    {
        tankController = GetComponent<TankController>();
        BindData(tankController.GetData<TowerData>());
        towerController.BindController(towerData);
        towerAsset.BindAssets();
        towerAnimator.BindAnimators(towerData.animators);
    }

    string IManager.GetDescription()
    {
         return String.Format(description, towerData.rotationSpeed);

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
