using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;









public class TankController : MonoBehaviour
{
  
    public List<IManager> iTankManager { get ; private set;}  


    private GameManager m_Game;

    [HideInInspector]
    public TankScriptable tankScriptable;

    private void Awake()
    {
        m_Game = GameManager.Instance;
        


        
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


        T scriptable = (T)component.listScriptableObjectUpgrade.TakeAtIndexOrLast(component.upgradeLevel);

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

                iTankManager.ForEach(component =>
                {

                        component.Bind();
                 
                    
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
    
    public BaseScriptableObjectData Upgrade(string managerName)
    {
        BaseScriptableObjectData scriptableFound = new BaseScriptableObjectData();
        tankScriptable.baseScriptableObjects.ForEach(baseScriptableObject =>
        {
            if (baseScriptableObject.listScriptableObjectUpgrade.First().name.Contains(managerName))
                scriptableFound = baseScriptableObject;
        });
        if (scriptableFound != null)
        {
            if (scriptableFound.listScriptableObjectUpgrade.IsIndexAfter(scriptableFound.upgradeLevel))
            {
                scriptableFound.upgradeLevel = scriptableFound.upgradeLevel + 1;
                BindTank(tankScriptable.baseScriptableObjects);
            } 

            return scriptableFound;
        }
        else
            return null;
          

      
    }

}