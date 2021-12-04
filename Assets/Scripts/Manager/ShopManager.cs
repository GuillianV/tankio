using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(ShopManager))]
class ShopButtons : Editor {
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        
        GameManager m_game = GameManager.Instance;
        ShopManager shopManager = (ShopManager) target;
        
        if (GUILayout.Button("Reset All"))
        {
            shopManager.ResetShopManager();
        }
  
            
    }
}
[System.Serializable]
public class ShopItem
{
    public int identifier;
    public int itemBaseCost ;
    // [HideInInspector]
    public int itemActualCost;
    [Range(1,3)]
    public float itemCostMultiplyer;
    [HideInInspector]
    public int itemLvl;
        
}


public class ShopManager : MonoBehaviour
{
    private GameManager m_Game;
    public int golds;

    public List<ShopItem> listOfShopItems = new List<ShopItem>();


    
    
    void Awake()
    {
        m_Game = GameManager.Instance;
    }

    private void Start()
    {

        listOfShopItems.ForEach(I =>
        {
            I.itemActualCost = I.itemBaseCost;
            m_Game.Ui.SetShopItemCost(I.identifier, I.itemActualCost);
            m_Game.Ui.SetShopItemLevel(I.identifier, I.itemLvl);
        });
        
        

            
    }

    public void AddGolds(int golds)
    {
        this.golds += golds;
        m_Game.Ui.SetGoldUI(this.golds);
    }

    public void ResetGolds()
    {
        golds = 0;
    }
    
    public void ResetShopManager()
    {
        ResetGolds();
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    #region TracksShop

    public void UpgradeShopItem(int uniqueIdentifier)
    {

        ShopItem shopItem = listOfShopItems.FirstOrDefault(I => I.identifier == uniqueIdentifier);

        
            if (golds > shopItem.itemActualCost)
            {
                shopItem.itemLvl++;
                golds -= shopItem.itemActualCost;
                shopItem.itemActualCost = Mathf.RoundToInt(shopItem.itemActualCost * shopItem.itemCostMultiplyer); 
                m_Game.Ui.SetShopItemCost(shopItem.identifier, shopItem.itemActualCost);
                m_Game.Ui.SetShopItemLevel(shopItem.identifier, shopItem.itemLvl);
            
            }
            else
            {
                Debug.LogWarning("lack gold");
            }
    
        
       
    }

    #endregion
    
}
