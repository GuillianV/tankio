using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;









public class TankController : MonoBehaviour
{
  
    public List<IUpgradable> iUpgradabletList { get ; private set;}  

    public List<ITankManager> iTankManager { get ; private set;}  


    private GameManager m_Game;

    private void Awake()
    {
        m_Game = GameManager.Instance;
        

        iUpgradabletList = new List<IUpgradable>();
        foreach(IUpgradable component in GetComponents<IUpgradable>())
        {
            iUpgradabletList.Add(component);
        }

        
        iTankManager = new List<ITankManager>();
        foreach(ITankManager component in GetComponents<ITankManager>())
        {
            iTankManager.Add(component);
        }
        
        
    }


    
    public T GetTankManager<T>() where T : ITankManager  
    {
        T component =(T) iTankManager.FirstOrDefault(component => component.GetType() == typeof(T));
        if (component != null)
        {
            return component;
        }
        else
        {
            return default(T);
        }
        
    }
    
    

    //Bind tank data to their components (TowerData to TowerController stats and assets)
    public void BindTank(List<ScriptableObject> tankDatas)
    {

        if (tankDatas != null && tankDatas.Count > 0)
        {
            if (iTankManager.Count > 0)
            {
                iTankManager.ForEach(component =>
                {
                    string componentName = component.GetType().FullName.Replace("Manager", String.Empty);
                    ScriptableObject dataFound = tankDatas.FirstOrDefault(data => data.name.Contains(componentName));
                    
                    
                    if (dataFound)
                    {
                        component.Bind(dataFound);
                    }
                    else
                    {
                        Debug.LogError("Tank Controller missing "+componentName+"Data in TankController");
                    }
                    
                    
                    // string componentName = component.GetType().FullName.Replace("Controller", String.Empty);
                    // ScriptableObject dataFound = tankDatas.FirstOrDefault(data => data.name.Contains(componentName));
                    //
                    //
                    // if (dataFound)
                    // {
                    //     component.BindData(dataFound);
                    //     component.BindComponent();
                    //     component.BindStats();
                    // }
                    // else
                    // {
                    //     Debug.LogError("Tank Controller missing "+componentName+"Data in TankController");
                    // }
                    
                });
            }
            else
            {
                Debug.LogError("Tank Controller missing components");
            }
        }
        else
        {
            Debug.LogError("Tank Controller missing tankDatas");
        }
        
    
        
     
    }
    

}