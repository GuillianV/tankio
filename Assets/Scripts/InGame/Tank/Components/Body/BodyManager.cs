using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyManager : MonoBehaviour, IManager, IDamagable
{

    public BaseAsset bodyAsset ;
    public BaseAnimator bodyAnimator;
    public BodyController bodyController;

    private BodyData bodyData;
    private TankDamage tankDamage;
    private TankGolds tankGolds;
    private TankCreate tankCreated;
    private TankDestroyed tankDestroyed;
    private TankController tankController;


    void IManager.Bind()
    {
        tankController = GetComponent<TankController>();
        BindData(tankController.GetData<BodyData>());

        tankDamage = GetComponent<TankDamage>();
        tankGolds = GetComponent<TankGolds>();
        tankCreated = GetComponent<TankCreate>();
        tankDestroyed = GetComponent<TankDestroyed>();

        bodyController.BindController(bodyData);
        bodyAsset.BindAssets();
        bodyAnimator.BindAnimators(bodyData.animators);

        tankCreated.OnCreated();
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



    void IDamagable.TakeDamage(string senderTag, float damages)
    {
        bodyController.TakeDamage(senderTag, damages);

        //Send event when damage is taken
        tankDamage.OnDamageTaken(this.gameObject, senderTag, damages);

        if (bodyController.GetHealt() <= 0)
        {
            //Send Golds When tank Destroyed
            tankGolds.OnGoldEarned(bodyController.GetGold());
            tankDestroyed.OnDestroyed(senderTag);
            Destroy(gameObject);
        }
    }



 
}
