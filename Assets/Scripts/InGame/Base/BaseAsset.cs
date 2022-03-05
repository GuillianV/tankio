using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class BaseAsset 
{
    public List<Assets> assetsList = new List<Assets>();
    private Dictionary<string, Assets> assetsDico = new Dictionary<string, Assets>();


    [System.Serializable]
    public struct Assets
    {
        public string _name;
        public GameObject _gameObject;
    }

    public void BindAssets()
    {
        assetsList.ForEach(asset =>
        {
            assetsDico.Add(asset._name, asset);
        });
    }
    

    public GameObject CallAsset(string assetName)
    {
        Debug.Log(assetName);
        return assetsDico[assetName]._gameObject;
    }

}
