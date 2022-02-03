using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TracksManager : MonoBehaviour, ITankManager
{
    public TankBaseAsset tracksAsset ;
    public TankBaseAnimator tracksAnimator;
    public TracksController tracksController;
    
  

    void ITankManager.Bind(ScriptableObject data)
    {

        tracksController.BindController(data);
        tracksAsset.BindAssets();
        tracksAnimator.BindAnimators();
    }
}
