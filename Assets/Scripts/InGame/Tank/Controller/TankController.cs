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
            if (component.dataList.scriptableDatas.FirstOrDefault().GetType() == typeof(T))
                return true;
            else
                return false;
        } );


        T scriptable = (T)component.dataList.scriptableDatas.TakeAtIndexOrLast(component.upgradeLevel);

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
    public void BindTank()
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




    public BaseScriptableObjectData Upgrade(string managerName,string parentManagerName = "")
    {
        BaseScriptableObjectData scriptableFound = new BaseScriptableObjectData();
        tankScriptable.baseScriptableObjects.ForEach(baseScriptableObject =>
        {
            if (baseScriptableObject.dataList.scriptableDatas.First().name.Contains(managerName))
                scriptableFound = baseScriptableObject;
        });
        if (scriptableFound != null)
        {
            if (scriptableFound.dataList.scriptableDatas.IsIndexAfter(scriptableFound.upgradeLevel))
            {
                scriptableFound.upgradeLevel = scriptableFound.upgradeLevel + 1;



                string managerNameToBind = managerName;

                if (!String.IsNullOrEmpty(parentManagerName))
                {
                    managerNameToBind = parentManagerName;
                }

                //Bind le manager corespondant � l'upgrade
                iTankManager.ForEach(component =>
                {
                    if(component.GetType().Name.Contains(managerNameToBind))
                        component.Bind();

                });


                //BindTank(); 
            } 

            return scriptableFound;
        }
        else
            return null;
          

      
    }

}