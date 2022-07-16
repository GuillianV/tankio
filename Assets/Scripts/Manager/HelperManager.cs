using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HelperManager : MonoBehaviour
{

    public GameObject helper;
    public TextMeshProUGUI helperText;
    public TextMeshProUGUI helperTitle;

    //public void Awake()
    //{

    //}

    public void ClosePopup()
    {
        helper.SetActive(false);
    }

    public void ShowPopup(string desc, string title)
    {
        helper.SetActive(true);
        helperTitle.text = title;
        helperText.text = desc;
    }


}
