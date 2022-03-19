using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    [Header("In Game Menu")] public TextMeshProUGUI lifeUI;
    public List<TextMeshProUGUI> goldsUI;
    public TextMeshProUGUI waveUI;
    public GameObject inGameParent;

    [Header("Paused Menu")] public GameObject pausedParent;

    [Header("Dead Menu")]
    public GameObject deadParent;
    public Image deadBackgroundImage;
    public TextMeshProUGUI deadButton;
    public TextMeshProUGUI deadTitle;
    private float deadTimeLerp = 0;
    private float fadeSpeed = 0.25f;
    private bool isDead = false;
    
 
    private GameManager m_Game;

    // Start is called before the first frame update

    


 
    
    private void Awake()
    {
        m_Game = GameManager.Instance;
   
       
    }


    void Update()
    {
        
  


        if (isDead)
        {
            SetBackgroundOpacityToBlack();
            SetDeadItemOpacityToWhite();
        }
        
       

    }

  

    #region Pause Menu

    public void TogglePausedMenu()
    {
        if (pausedParent != null)
        {
            if (pausedParent.activeSelf)
            {
                pausedParent.SetActive(false);
            }
            else
            {
                pausedParent.SetActive(true);
            }
        }
        else
        {
            Debug.LogWarning("missing in game parent");
        }
    }
    
    
    public void HidePausedMenu()
    {
        if (pausedParent != null)
        {
            pausedParent.SetActive(false);
        }
        else
        {
            Debug.LogWarning("missing in game parent");
        }
    }
    
    public void ShowPausedMenu()
    {
        if (pausedParent != null)
        {
            pausedParent.SetActive(true);
        }
        else
        {
            Debug.LogWarning("missing in game parent");
        }
    }
    

    #endregion


    #region InGame Menu

    public void HideInGameMenu()
    {
        if (inGameParent != null)
        {
            inGameParent.SetActive(false);
        }
        else
        {
            Debug.LogWarning("missing in game parent");
        }
    }
    
    public void ShowInGameMenu()
    {
        if (inGameParent != null)
        {
            inGameParent.SetActive(true);
        }
        else
        {
            Debug.LogWarning("missing in game parent");
        }
    }
    
    public void ToggleInGameMenu()
    {
        if (inGameParent != null)
        {
            if (inGameParent.activeSelf)
            {
                inGameParent.SetActive(false);
            }
            else
            {
                inGameParent.SetActive(true);
            }
        }
        else
        {
            Debug.LogWarning("missing in game parent");
        }
    }

    public void SetLifeUI(float maxLife, float life)
    {
        if (lifeUI != null)
        {
            lifeUI.text =  (life / maxLife * 100f).ToString("F0") ;
        }
        else
        {
            Debug.LogWarning("Missing Life component");
        }
    }

    public void SetGoldUI(int golds)
    {
        if (goldsUI != null)
        {
            goldsUI.ForEach(ui =>
            {
                ui.text =  golds.ToString();
            });
            
        }
        else
        {
            Debug.LogWarning("Missing Gold component");
        }
    }

    

    public void SetWaveUI(int wave)
    {
        if (waveUI != null)
        {
            waveUI.text = "WAVE   " + wave;
        }
        else
        {
            Debug.LogWarning("Missing wave component");
        }
    }

    #endregion


    #region Dead Menu

    public void ToggleDeadMenu()
    {
        if (deadParent != null)
        {
            if (deadParent.activeSelf)
            {
                deadParent.SetActive(false);
            }
            else
            {
                deadParent.SetActive(true);
            }
        }
        else
        {
            Debug.LogWarning("missing dead parent");
        }
    }
    
    public void HideDeadMenu()
    {
        if (deadParent != null)
        {
            deadParent.SetActive(false);
            deadTimeLerp = 0;
            isDead = false;
            m_Game.isDead = false;
        }
        else
        {
            Debug.LogWarning("missing dead parent");
        }
    }
    
    public void ShowDeadMenu()
    {
        if (deadParent != null)
        {
            deadParent.SetActive(true);
            isDead = true;
            m_Game.isDead = true;
        }
        else
        {
            Debug.LogWarning("missing dead parent");
        }
    }

    public void SetBackgroundOpacityToBlack()
    {
        deadTimeLerp += fadeSpeed * Time.deltaTime  *  m_Game.TimeManager.timeScale;
        
        var color = deadBackgroundImage.color;
        var newColor = new Color(color.r, color.g, color.b,  Mathf.Lerp(0, 1, deadTimeLerp));
        deadBackgroundImage.color = newColor;
       
        if (deadTimeLerp > 1.0f)
        {
            deadTimeLerp = 1;
        }
        

    }
    
    public void SetBackgroundOpacityToWhite()
    {
        deadTimeLerp += fadeSpeed * Time.deltaTime *  m_Game.TimeManager.timeScale;
        
        var color = deadBackgroundImage.color;
        var newColor = new Color(color.r, color.g, color.b,  Mathf.Lerp(1, 0, deadTimeLerp));
        deadBackgroundImage.color = newColor;
       
        if (deadTimeLerp > 1.0f)
        {
            deadTimeLerp = 1;
        }
        

    }

    public void SetDeadItemOpacityToWhite()
    {
        deadTimeLerp += fadeSpeed * Time.deltaTime  *  m_Game.TimeManager.timeScale;

        
        
        var colorButton =deadButton.color;
        var newColorButton = new Color(colorButton.r, colorButton.g, colorButton.b,  Mathf.Lerp(0, 1, deadTimeLerp));
        deadButton.color = newColorButton;
       
        var colorTitle =deadTitle.color;
        var newColorTitle = new Color(colorTitle.r, colorTitle.g, colorTitle.b,  Mathf.Lerp(0, 1, deadTimeLerp));
        deadTitle.color = newColorTitle;

        
        if (deadTimeLerp > 1.0f)
        {
            deadTimeLerp = 1;
        }
        

    }



    #endregion
    
}