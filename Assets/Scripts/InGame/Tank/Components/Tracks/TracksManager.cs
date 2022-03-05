using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TracksManager : MonoBehaviour, IManager , IUpgradable
{
    public BaseAsset tracksAsset ;
    public BaseAnimator tracksAnimator;
    public TracksController tracksController;
    private TracksData tracksData;
  

    void IManager.Bind(ScriptableObject data)
    {
        BindData(data);
        tracksController.BindController(tracksData);
        tracksAsset.BindAssets();
        tracksAnimator.BindAnimators(tracksData.animators);
    }

    void IUpgradable.Upgrade()
    {
        tracksController.Upgrade();
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
