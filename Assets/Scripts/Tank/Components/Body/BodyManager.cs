using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyManager : MonoBehaviour, ITankManager
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
}
