using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;









public class TankController : MonoBehaviour
{
  
    public List<ITankComponent> iTankComponentList { get ; private set;}  
    public List<IUpgradable> iUpgradabletList { get ; private set;}  
    
    public List<ITankAnimator> ITankAnimatorsList { get ; private set;}  


    private GameManager m_Game;

    private void Awake()
    {
        m_Game = GameManager.Instance;

        iTankComponentList = new List<ITankComponent>();
        foreach(ITankComponent component in GetComponents<ITankComponent>())
        {
            iTankComponentList.Add(component);
        }

        iUpgradabletList = new List<IUpgradable>();
        foreach(IUpgradable component in GetComponents<IUpgradable>())
        {
            iUpgradabletList.Add(component);
        }

        ITankAnimatorsList = new List<ITankAnimator>();
        foreach(ITankAnimator component in GetComponents<ITankAnimator>())
        {
            ITankAnimatorsList.Add(component);
        }
        
    }


    public T GetTankComponent<T>() where T : ITankComponent  
    {
        T component =(T) iTankComponentList.FirstOrDefault(component => component.GetType() == typeof(T));
        if (component != null)
        {
            return component;
        }
        else
        {
            return default(T);
        }
        
    }
    
    public T GetTankAnimator<T>() where T : ITankAnimator  
    {
        T component =(T) ITankAnimatorsList.FirstOrDefault(component => component.GetType() == typeof(T));
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
            if (iTankComponentList.Count > 0)
            {
                iTankComponentList.ForEach(component =>
                {
                    string componentName = component.GetType().FullName.Replace("Controller", String.Empty);
                    ScriptableObject dataFound = tankDatas.FirstOrDefault(data => data.name.Contains(componentName));
                    
                    
                    if (dataFound)
                    {
                        component.BindData(dataFound);
                        component.BindComponent();
                        component.BindStats();
                    }
                    else
                    {
                        Debug.LogError("Tank Controller missing "+componentName+"Data in TankController");
                    }
                    
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