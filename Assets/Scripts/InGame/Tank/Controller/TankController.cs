using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;









public class TankController : MonoBehaviour
{
  
    public List<IUpgradable> iUpgradabletList { get ; private set;}  

    public List<IManager> iTankManager { get ; private set;}  


    private GameManager m_Game;

    [HideInInspector]
    public TankScriptable tankScriptable;

    private void Awake()
    {
        m_Game = GameManager.Instance;
        

        iUpgradabletList = new List<IUpgradable>();
        foreach(IUpgradable component in GetComponents<IUpgradable>())
        {
            iUpgradabletList.Add(component);
        }

        
        iTankManager = new List<IManager>();
        foreach(IManager component in GetComponents<IManager>())
        {
            iTankManager.Add(component);
        }
        
        
    }


    
    public T GetTankManager<T>() where T : IManager  
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


    public T GetData<T>() where T : BaseScriptableObject
    {
        BaseScriptableObjectData component = (BaseScriptableObjectData)tankScriptable.baseScriptableObjects.FirstOrDefault(component => {
            if (component.listScriptableObjectUpgrade.FirstOrDefault().GetType() == typeof(T))
                return true;
            else
                return false;
        } );


        T scriptable = (T)component.listScriptableObjectUpgrade[component.upgradeLevel];

        if (component != null)
        {
            return scriptable;
        }
        else
        {
            return default(T);
        }

    }




    //Bind tank data to their components (TowerData to TowerController stats and assets)
    public void BindTank(List<BaseScriptableObjectData> tankDatas)
    {

        if (tankDatas != null && tankDatas.Count > 0)
        {
            if (iTankManager.Count > 0)
            {

                List<BaseScriptableObject> tankScriptableObjects = new List<BaseScriptableObject>();
                tankDatas.ForEach(baseScriptableObject =>
                {

                        tankScriptableObjects.Add(baseScriptableObject.listScriptableObjectUpgrade[baseScriptableObject.upgradeLevel]);
                    

                });

                iTankManager.ForEach(component =>
                {

                    string componentName = component.GetType().FullName.Replace("Manager", String.Empty);
                    ScriptableObject dataFound = tankScriptableObjects.FirstOrDefault(data => data.name.Contains(componentName));
                    
                    
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
    
    public void Upgrade(string managerName)
    {
        BaseScriptableObjectData scriptableFound = new BaseScriptableObjectData();
        tankScriptable.baseScriptableObjects.ForEach(baseScriptableObject =>
        {
            if (baseScriptableObject.listScriptableObjectUpgrade.First().name.Contains(managerName))
                scriptableFound = baseScriptableObject;
        });
        if(scriptableFound != null)
            scriptableFound.upgradeLevel = scriptableFound.upgradeLevel+1;

        BindTank(tankScriptable.baseScriptableObjects);
    }

}