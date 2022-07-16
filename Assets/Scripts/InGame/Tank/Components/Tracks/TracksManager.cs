using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TracksManager : MonoBehaviour, IManager 
{
    public BaseAsset tracksAsset ;
    public BaseAnimator tracksAnimator;
    public TracksController tracksController;
    [TextArea(5, 10)]
    public string description;
    private TracksData tracksData;
    private TankController tankController;
   

    void IManager.Bind()
    {
        tankController = GetComponent<TankController>();
        BindData(tankController.GetData<TracksData>());
        tracksController.BindController(tracksData);
        tracksAsset.BindAssets();
        tracksAnimator.BindAnimators(tracksData.animators);
    }

    string IManager.GetDescription()
    {

        return String.Format(description, tracksData.speed, tracksData.rotationSpeed);


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
