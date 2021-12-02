using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(ShopManager))]
class ShopButtons : Editor {
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        
        GameManager m_game = GameManager.Instance;
        ShopManager shopManager = (ShopManager) target;
        
        if (GUILayout.Button("Reset All"))
        {
            shopManager.ResetShopManager();
        }
  
            
    }
}

public class ShopManager : MonoBehaviour
{
    private GameManager m_Game;
    public int golds;
    void Awake()
    {
        m_Game = GameManager.Instance;
    }

    public void AddGolds(int golds)
    {
        this.golds += golds;
        m_Game.Ui.SetGoldUI(this.golds);
    }

    public void ResetGolds()
    {
        golds = 0;
    }
    
    public void ResetShopManager()
    {
        ResetGolds();
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
