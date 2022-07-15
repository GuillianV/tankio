using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HelperManager : MonoBehaviour
{

    public GameObject helper;
    public TextMeshProUGUI helperText;

    //public void Awake()
    //{
       
    //}

    public void ClosePopup()
    {
        helper.SetActive(false);
    }

    public void ShowPopup(string text)
    {
        helper.SetActive(true);
        helperText.text = text;
    }


}
