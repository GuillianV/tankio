using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyManager : MonoBehaviour, ITankManager,IUpgradable
{

    public TankBaseAsset bodyAsset ;
    public TankBaseAnimator bodyAnimator;
    public BodyController bodyController;
    private BodyData bodyData;
  

    void ITankManager.Bind(ScriptableObject data)
    {
        BindData(data);
        bodyController.BindController(bodyData);
        bodyAsset.BindAssets();
        bodyAnimator.BindAnimators(bodyData.bodyAnimators);
    }

    void IUpgradable.Upgrade()
    {
        bodyController.Upgrade();
    }

    void BindData(ScriptableObject obj)
    {

        if (obj.GetType() == typeof(BodyData))
        {
            bodyData = (BodyData)obj;
        }
        else
        {
            Debug.LogError("bodyManager cannot load bodyData");
        }


    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (bodyController.GetHealt() <= 0)
        {
            Destroy(gameObject);
        }
    }
}
