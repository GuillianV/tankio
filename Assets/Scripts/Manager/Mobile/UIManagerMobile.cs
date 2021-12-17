using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_ANDROID
[RequireComponent(typeof(UIManager))]
public class UIManagerMobile : MonoBehaviour
{

    public GameObject UIMobile;
    private UIManager m_uiManager;

    private void Awake()
    {
        m_uiManager = GetComponent<UIManager>();
    }

    void Start()
    {
        UIMobile.SetActive(true);
    }

    
}

#endif