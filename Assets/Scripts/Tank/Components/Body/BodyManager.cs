using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyManager : MonoBehaviour, ITankManager,IUpgradable
{

    public TankBaseAsset bodyAsset ;
    public TankBaseAnimator bodyAnimator;
    public BodyController bodyController;
    
  

    void ITankManager.Bind(ScriptableObject data)
    {

        bodyController.BindController(data);
        bodyAsset.BindAssets();
        bodyAnimator.BindAnimators();
    }

    void IUpgradable.Upgrade()
    {
        bodyController.Upgrade();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (bodyController.GetHealt() <= 0)
        {
            Destroy(gameObject);
        }
    }
}
