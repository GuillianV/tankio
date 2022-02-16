using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TracksManager : MonoBehaviour, ITankManager
{
    public TankBaseAsset tracksAsset ;
    public TankBaseAnimator tracksAnimator;
    public TracksController tracksController;
    private TracksData tracksData;
  

    void ITankManager.Bind(ScriptableObject data)
    {
        BindData(data);
        tracksController.BindController(tracksData);
        tracksAsset.BindAssets();
        tracksAnimator.BindAnimators(tracksData.tracksAnimators);
    }

    void BindData(ScriptableObject obj)
    {

        if (obj.GetType() == typeof(TracksData))
        {
            tracksData = (TracksData)obj;
        }
        else
        {
            Debug.LogError("tracksManager cannot load tracksData");
        }


    }


}
