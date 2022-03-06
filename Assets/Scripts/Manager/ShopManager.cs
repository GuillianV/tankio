using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.PostProcessing;

#if UNITY_EDITOR
[CustomEditor(typeof(ShopManager))]
class ShopButtons : Editor
{
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
#endif
[System.Serializable]
public class ShopItem
{
    public int identifier;

    public int itemBaseCost;

    // [HideInInspector]
    public int itemActualCost;
    [Range(1, 3)] public float itemCostMultiplyer;
    [HideInInspector] public int itemLvl;

    public BaseScriptableObject objectScript;

    public UnityEvent calledWhenComponentUpgraded;

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

    
    #region Golds

    //Add golds
    public void AddGolds(int golds)
    {
        this.golds += golds;
        m_Game.Ui.SetGoldUI(this.golds);
    }

    //Reset golds
    public void ResetGolds()
    {
        golds = 0;
        m_Game.Ui.SetGoldUI(golds);
    }

    #endregion


    #region Shop

    //Upgrade an item
    public void UpgradeShopItem(int uniqueIdentifier)
    {
        //Search for item clicked with button generated in UIManager
        ShopItem shopItem = listOfShopItems.FirstOrDefault(I => I.identifier == uniqueIdentifier);

        //Check if we have more golds than the next upgrade cost of item found
        if (golds >= shopItem.itemActualCost)
        {
            
            //Look for player gameobject
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player == null || shopItem.objectScript == null)
            {
                return;
            }

            TankController tankController = player.GetComponent<TankController>();
            string name = shopItem.objectScript.GetType().ToString().Replace("Data",String.Empty);


            BaseScriptableObjectData objectData = tankController.Upgrade(name);


            if (shopItem.itemLvl < objectData.upgradeLevel)
            {
                shopItem.itemLvl++;
                golds -= shopItem.itemActualCost;
                m_Game.Ui.SetGoldUI(golds);
                shopItem.itemActualCost =
                    Mathf.RoundToInt(shopItem.itemActualCost * shopItem.itemCostMultiplyer);
                m_Game.Ui.SetShopItemCost(shopItem.identifier, shopItem.itemActualCost);
                m_Game.Ui.SetShopItemLevel(shopItem.identifier, shopItem.itemLvl);
                m_Game.Ui.SetShopItemImage(shopItem.identifier, objectData.listScriptableObjectUpgrade[objectData.upgradeLevel].sprite, objectData.listScriptableObjectUpgrade[objectData.upgradeLevel].color);
                shopItem.calledWhenComponentUpgraded.Invoke();
            }

        }
        else
        {
            Debug.LogWarning("lack gold");
        }
    }


    //Reset 
    public void ResetShopItems()
    {
        listOfShopItems.ForEach(shopItem =>
        {
            shopItem.itemLvl = 0;
            shopItem.itemActualCost = shopItem.itemBaseCost;
            m_Game.Ui.SetShopItemCost(shopItem.identifier, shopItem.itemActualCost);
            m_Game.Ui.SetShopItemLevel(shopItem.identifier, shopItem.itemLvl);
        });
    }

    #endregion
    
    
    //Reset all shop item upgrades and gold
    public void ResetShopManager()
    {
        ResetGolds();
        ResetShopItems();
    }

}