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
    public GameObject optionsMenu;

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

    public Scrollbar scrollbarFullScreen;

    public Scrollbar scrollbarHertz;
    public TextMeshProUGUI heartzText;
    private int maxHeartz = 240;

    private int deviceWidth;
    private int deviceHeight;
    private int deviceHertz;
    private bool deviceFullScreen;
    private int deviceQuality;


    void Start()
    {
        deviceWidth = PlayerPrefs.GetInt("Screen_pref_width", Screen.currentResolution.width);
        deviceHeight = PlayerPrefs.GetInt("Screen_pref_height", Screen.currentResolution.height);
        deviceHertz = PlayerPrefs.GetInt("Screen_pref_hz", Screen.currentResolution.refreshRate);
        deviceFullScreen = Convert.ToBoolean(PlayerPrefs.GetInt("Screen_pref_full", 1));
        deviceQuality = PlayerPrefs.GetInt("Screen_pref_quality", 2);

        SetupFullScreenScrollBar();
        SetHeratzScrollBar();
        SetupResolutionDropDown();
        SetupQualityDropDown();
        
    }


    private void SetupFullScreenScrollBar()
    {
     
        scrollbarFullScreen.SetValueWithoutNotify(Convert.ToSingle(deviceFullScreen));
        Screen.fullScreen = deviceFullScreen;
        scrollbarFullScreen.onValueChanged.AddListener(delegate {
            SetFullScreen(scrollbarFullScreen.value);
        });
    }

    private void SetHeratzScrollBar()
    {

        scrollbarHertz.SetValueWithoutNotify((float)deviceHertz/ (float)maxHeartz);
        heartzText.text = deviceHertz.ToString();
        scrollbarHertz.onValueChanged.AddListener(delegate {
            SetHeartz(scrollbarHertz.value);
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
        
        dropdownQuality.SetValueWithoutNotify(deviceQuality);
        QualitySettings.renderPipeline = urpList[deviceQuality].urpScriptable;
        dropdownQuality.onValueChanged.AddListener(delegate {
            DropDownQualitySelectedValue(dropdownQuality.value);
        });
    }
    
    private void SetupResolutionDropDown()
    {
        dropdownResolution.ClearOptions();
        resolutions = Screen.resolutions.ToList();


        if(resolutions.Count == 0)
        {
            
            for(int i = 8; i > 1; i--)
            {
                Resolution newRes = new Resolution();
                newRes.width = Display.main.systemWidth/i;
                newRes.height = Display.main.systemHeight/i;
                newRes.refreshRate = Screen.currentResolution.refreshRate;
                resolutions.Add(newRes);
            }

            Resolution native = new Resolution();
            native.width = Display.main.systemWidth;
            native.height = Display.main.systemHeight;
            native.refreshRate = Screen.currentResolution.refreshRate;
            resolutions.Add(native);


        }


        resolutions.ForEach(res =>
        {
            resolutionString.Add(res.width +"x"+res.height);
            
        });
        dropdownResolution.AddOptions(resolutionString);
        dropdownResolution.SetValueWithoutNotify(resolutions.FindIndex(res=>res.width == deviceWidth && res.height == deviceHeight));
        Screen.SetResolution(deviceWidth, deviceHeight, deviceFullScreen, deviceHertz);
        dropdownResolution.onValueChanged.AddListener(delegate {
            DropDownResolutionSelectedValue(dropdownResolution.value);
        });
    }
    


    public void Back()
    {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }

    public void SetFullScreen(float value)
    {
        if (value == 1)
        {
            Screen.fullScreen = true;
            deviceFullScreen = true;
            PlayerPrefs.SetInt("Screen_pref_full",1);
        }
        if (value == 0)
        {
            Screen.fullScreen = false;
            deviceFullScreen = false;
            PlayerPrefs.SetInt("Screen_pref_full",0);
        }
        
        PlayerPrefs.Save();
    
    }

    public void SetHeartz(float value)
    {
        deviceHertz = Convert.ToInt32((float) maxHeartz * value);
        heartzText.text = deviceHertz + " hz";
        PlayerPrefs.SetInt("Screen_pref_hz", deviceHertz);
        PlayerPrefs.Save();
    }

    public void DropDownQualitySelectedValue(Int32 value)
    {
      
      QualitySettings.SetQualityLevel(value, false);
      QualitySettings.renderPipeline = urpList[value].urpScriptable;
      PlayerPrefs.SetInt("Screen_pref_quality",value);
      PlayerPrefs.Save();
    }
    
    
    public void DropDownResolutionSelectedValue(Int32 value)
    {
        deviceWidth = resolutions[value].width;
        deviceHeight = resolutions[value].height;
        
        Screen.SetResolution(deviceWidth,deviceHeight, deviceFullScreen, deviceHertz);
        PlayerPrefs.SetInt("Screen_pref_width", deviceWidth);
        PlayerPrefs.SetInt("Screen_pref_height", deviceHeight);
        PlayerPrefs.Save();

    }
    
  
}
