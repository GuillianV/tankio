using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyManager : MonoBehaviour, ITankManager,IUpgradable, IDamagable
{

    public TankBaseAsset bodyAsset ;
    public TankBaseAnimator bodyAnimator;
    public BodyController bodyController;
    private BodyData bodyData;
    private TankDamage tankDamage;

    void ITankManager.Bind(ScriptableObject data)
    {
        BindData(data);
        tankDamage = GetComponent<TankDamage>();
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

    void IDamagable.TakeDamage(GameObject sender, float damages)
    {
        bodyController.TakeDamage(sender.tag, damages);
        tankDamage.OnDamageTaken(this.gameObject,sender,damages);
        if (bodyController.GetHealt() <= 0)
        {
            Destroy(gameObject);
        }
    }

 
}
