using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct GunAmmo 
{
    public string name;
    public ProjectileScriptableObject projectileData;
    public GameObject objectInstancied;
    public bool isEquiped;

    public GunAmmo(string _name, ProjectileScriptableObject _projectileData, GameObject _objectInstancied,bool _isEquiped)
    {
        this.name = _name;
        this.projectileData = _projectileData;
        this.objectInstancied = _objectInstancied;
        this.isEquiped = _isEquiped;
    }

}
