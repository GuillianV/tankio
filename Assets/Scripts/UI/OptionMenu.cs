using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class OptionMenu : MonoBehaviour
{
    public GameObject mainMenu;

    //Quality
    [System.Serializable]
    public struct URPQuality
    {
        public UniversalRenderPipelineAsset urpScriptable;
        public string name;
    }
    
    public List<URPQuality> urpList = new List<URPQuality>();
    public TMP_Dropdown dropdownQuality;
    private List<string> URPQualityString = new List<string>();
    
    public TMP_Dropdown dropdownResolution;
    public List<Resolution> resolutions = new List<Resolution>();
    private List<string> resolutionString = new List<string>();

    public Scrollbar scrollbar;
    
    

    private int deviceWidth;
    private int deviceHeight;
    private int deviceHertz;
    private bool deviceFullScreen;
    
    Resolution[] resolutionss;
    void Start()
    {
        
        SetupFullScreenDropDown();
        SetupResolutionDropDown();
        SetupQualityDropDown();
        
    }

    void OnGUI()
    {
       
        GUILayout.BeginVertical();

        if (resolutionss.Length == 0)
        {
            if (GUILayout.Button(Screen.currentResolution.width +"x"+Screen.currentResolution.height))
            {
                Screen.SetResolution(Screen.currentResolution.width,Screen.currentResolution.height, true, Screen.currentResolution.refreshRate);
             
                
                
            }
        }
        
        for (int i = 0; i < resolutionss.Length; i++)
        {
            if (GUILayout.Button(resolutions[i].width +"x"+resolutions[i].height))
            {
                Screen.SetResolution(resolutions[i].width, resolutions[i].height, true, resolutions[i].refreshRate);
             
                
                
            }
        }
        GUILayout.EndVertical();
    }

    private void Update()
    {
        resolutionss = Screen.resolutions;
        
    }


    private void SetupFullScreenDropDown()
    {
        scrollbar.onValueChanged.AddListener(delegate {
            SetFullScreen(scrollbar.value);
        });
    }
    
    private void SetupQualityDropDown()
    {
        dropdownQuality.ClearOptions();
        urpList.ForEach(urp =>
        {
            URPQualityString.Add(urp.name);
            
        });
        dropdownQuality.AddOptions(URPQualityString);
        dropdownQuality.onValueChanged.AddListener(delegate {
            DropDownQualitySelectedValue(dropdownQuality.value);
        });
    }
    
    private void SetupResolutionDropDown()
    {
        dropdownResolution.ClearOptions();
        resolutions = Screen.resolutions.ToList();
        Resolution res = new Resolution();
        res.width = 500;
        res.height = 300;
        res.refreshRate = 15;
        resolutions.Add(res);
        resolutions.ForEach(res =>
        {
            resolutionString.Add(res.width +"x"+res.height);
            
        });
        dropdownResolution.AddOptions(resolutionString);
        dropdownResolution.onValueChanged.AddListener(delegate {
            DropDownResolutionSelectedValue(dropdownResolution.value);
        });
    }
    


    public void Back()
    {
        mainMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    public void SetFullScreen(float value)
    {
        if (value == 1)
        {
            Screen.fullScreen = true;
            deviceFullScreen = true;
        }
        if (value == 0)
        {
            Screen.fullScreen = false;
            deviceFullScreen = false;
        }
    }

    public void DropDownQualitySelectedValue(Int32 value)
    {
      
      QualitySettings.SetQualityLevel(value, false);
      QualitySettings.renderPipeline = urpList[value].urpScriptable;

    }
    
    
    public void DropDownResolutionSelectedValue(Int32 value)
    {
        deviceWidth = resolutions[value].width;
        deviceHeight = resolutions[value].height;
        Resolution res = new Resolution();
        res.width = 500;
        res.height = 300;
        res.refreshRate = 15;
        Screen.SetResolution(res.width, res.height, deviceFullScreen, res.refreshRate);

    }
    
  
}
