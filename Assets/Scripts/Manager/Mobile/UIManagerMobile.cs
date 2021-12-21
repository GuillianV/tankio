using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(UIManager))]
public class UIManagerMobile : MonoBehaviour
{
    public List<GameObject> uIMobiles;

#if UNITY_ANDROID
    private UIManager m_uiManager;
    private GameManager m_Game; 

    private void Awake()
    {
        m_Game = GameManager.Instance;
    
    }

    void Start()
    {
        uIMobiles.ForEach(g=>g.SetActive(true));
        
    }


    public void Shop()
    {
        m_Game.ShopMobile();
    }
    
    public void Pause()
    {
        m_Game.PauseMobile();
    }
    
    public void Close()
    {
        m_Game.CloseMobile();
    }



#endif





}
