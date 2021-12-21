using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class QualityManager : MonoBehaviour
{
    // #if UNITY_EDITOR

    public List<UniversalRenderPipelineAsset> URPQuality = new List<UniversalRenderPipelineAsset>();

    public int deviceWidth;
    public int deviceHeight;
    public int deviceHertz;
    public int deviceFullScreen;
    public int deviceQuality;
    public void Awake()
    {
        deviceWidth = PlayerPrefs.GetInt("Screen_pref_width", Screen.currentResolution.width);
        deviceHeight = PlayerPrefs.GetInt("Screen_pref_height", Screen.currentResolution.height);
        deviceHertz = PlayerPrefs.GetInt("Screen_pref_hz", Screen.currentResolution.refreshRate);
        deviceFullScreen = PlayerPrefs.GetInt("Screen_pref_full", 1);
        deviceQuality = PlayerPrefs.GetInt("Screen_pref_quality", 2);
        QualitySettings.vSyncCount = 0;
        Screen.SetResolution(deviceWidth, deviceHeight, Convert.ToBoolean(deviceFullScreen), deviceHertz);
        Application.targetFrameRate = deviceHertz;
        QualitySettings.SetQualityLevel(deviceQuality, false);
        QualitySettings.renderPipeline = URPQuality[deviceQuality];
    }


    void OnGUI()
    {

        GUILayout.BeginVertical();
        GUILayout.TextField(((int)(1.0f / Time.smoothDeltaTime)).ToString());
        GUILayout.EndVertical();
    }



    // #endif

}
