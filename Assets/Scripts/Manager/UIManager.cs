using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI lifeUI;
    public TextMeshProUGUI goldsUI;

    private GameManager m_Game;
    // Start is called before the first frame update
    void Start()
    {
        m_Game = GameManager.Instance;
        if (lifeUI == null || goldsUI == null)
        {
            Debug.LogError("Missing Life component or Gold Component in UIManager");
        }
        
    }



    public void SetLifeUI(float maxLife, float life)
    {
        if (lifeUI != null)
        {
            lifeUI.text = "LIFE : "+ (life / maxLife * 100f)+"%";
        }
        else
        {
            Debug.LogError("Missing Life component");
        }
    }
    
    public void SetGoldUI(int golds)
    {
        if (goldsUI != null)
        {
            goldsUI.text = "GOLDS : "+ golds;
        }
        else
        {
            Debug.LogError("Missing Gold component");
        }
    }
}
