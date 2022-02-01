using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITankComponent 
{

    void BindData(ScriptableObject obj);

    void BindComponent();

    void BindStats();

}