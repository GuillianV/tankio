using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
