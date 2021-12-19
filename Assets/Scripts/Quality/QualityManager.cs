using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class QualityManager : MonoBehaviour
{
    public List<UniversalRenderPipelineAsset> URPQuality = new List<UniversalRenderPipelineAsset>();

    public int deviceWidth;
    public int deviceHeight;
    public int deviceHertz;

    
    public void Awake()
    {
        deviceWidth = Screen.currentResolution.width;
        deviceHeight = Screen.currentResolution.height;
        deviceHertz = Screen.currentResolution.refreshRate;
    }
    
    void OnGUI()
    {
        string[] names = QualitySettings.names;
        GUILayout.BeginVertical();
        for (int i = 0; i < names.Length; i++)
        {
            if (GUILayout.Button(names[i]))
            {
                
                QualitySettings.SetQualityLevel(i, false);
                QualitySettings.renderPipeline = URPQuality[i];
                

                switch (i)
                {
                    case 0:
                        Screen.SetResolution(640, 480, true, 30);
                        break;
                    case 1:
                        Screen.SetResolution(1280, 720, true, 60);
                        break;
                    case 2:
                        Screen.SetResolution(deviceWidth, deviceHeight, true, deviceHertz);
                        break;
                    default:
                        Screen.SetResolution(640, 480, true, 60);
                        break;
                }
                
                
            }
        }
        GUILayout.EndVertical();
    }

    
}
