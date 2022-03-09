using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

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
    [Header("Shop Item Components")]
    [SerializeField]
    public int identifier;



    public int itemBaseCost;

    // [HideInInspector]
    public int itemActualCost;
    [Range(1, 3)] public float itemCostMultiplyer;
    [HideInInspector] public int itemLvl;

    public BaseScriptableObject objectScript;

    public UnityEvent calledWhenComponentUpgraded;

    public Vector2 imageSize = new Vector2(1, 1);


    [SerializeField] public string name;
    [HideInInspector] public TextMeshProUGUI shopItemCostUI;
    [HideInInspector] public TextMeshProUGUI shopItemLevelUI;
    [HideInInspector] public TextMeshProUGUI shopItemTitleUI;
    [HideInInspector] public Button shopItemUpgradeUI;
    [HideInInspector] public Image shopItemImageUI;
    [HideInInspector] public GameObject shopItemCostGameObject;
    [HideInInspector] public GameObject shopItemLevelGameObject;
    [HideInInspector] public GameObject shopItemImageGameObject;
    [HideInInspector] public GameObject shopItemTitleGameObject;
    [HideInInspector] public GameObject shopItemUpgradeGameObject;
    [HideInInspector] public GameObject shopItemParentGameObject;
}



public class ShopManager : MonoBehaviour
{
    private GameManager m_Game;
    public int golds;

    [Header("Shop Menu")] public GameObject shopParent;


    [Header("Shop Wrapper ")] public GameObject shopWrapperParent;
    [Range(1, 1000000)] public int slideSpeed = 350;
    private RectTransform m_shopRectTransform;
    private RectTransform m_containerRectTransform;
    private RectTransform m_wrapperRectTransform;


    [Header("Shop Patern Slide")] public GameObject shopItemParentRow;
    private RectTransform m_shopItemParentRowRectTransform;
    public GameObject shopItemParent;
    public GameObject shopItemTitle;
    public GameObject shopItemLevel;
    public GameObject shopItemCost;
    public GameObject shopItemUpgrade;
    public GameObject shopItemImage;
    public int itemRotationSpeed = 8;




    private int elementDisplayed = 1;
    private int maxItem;
    private bool isNexting = false;
    private float valueToReach;
    private float value;
    private bool isPreviousing = false;

    private Vector3 rotationEuler;

    private int frames;

    public List<ShopItem> listOfShopItems = new List<ShopItem>();


    void Awake()
    {
        m_Game = GameManager.Instance;
        m_wrapperRectTransform = shopWrapperParent.GetComponent<RectTransform>();
        m_containerRectTransform = m_wrapperRectTransform.GetComponentInParent<RectTransform>();
        m_shopRectTransform = shopParent.GetComponent<RectTransform>();
        m_shopItemParentRowRectTransform = shopItemParentRow.GetComponent<RectTransform>();
        maxItem = listOfShopItems.OrderByDescending(S => S.identifier).FirstOrDefault().identifier;

    }

    private void Start()
    {
        listOfShopItems.ForEach(shopItem =>
        {
            shopItem.itemActualCost = shopItem.itemBaseCost;
           SetShopItemCost(shopItem, shopItem.itemActualCost);
           SetShopItemLevel(shopItem, shopItem.itemLvl);
        });

        listOfShopItems.ForEach(shopItem => { GenerateshopItem(shopItem); });
        shopParent.SetActive(false);
        HideParentPatern();
        frames = 0;
    }



    public void Update()
    {

        if (shopParent.activeSelf)
        {
            RotateShopImage();
            Slide();
        }
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
    public void UpgradeShopItem(ShopItem shopItem)
    {
      
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
                SetShopItemCost(shopItem, shopItem.itemActualCost);
                SetShopItemLevel(shopItem, shopItem.itemLvl);
                SetShopItemImage(shopItem, objectData.dataList.scriptableDatas[objectData.upgradeLevel].sprite, objectData.dataList.scriptableDatas[objectData.upgradeLevel].color);
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
            SetShopItemCost(shopItem, shopItem.itemActualCost);
            SetShopItemLevel(shopItem, shopItem.itemLvl);
        });
    }


    //hide or show shop menu
    public void ToggleShopMenu()
    {
        if (shopParent != null)
        {
            if (shopParent.activeSelf)
            {
                shopParent.SetActive(false);
            }
            else
            {
                shopParent.SetActive(true);
            }
        }
        else
        {
            Debug.LogWarning("missing shop parent");
        }
    }

    public void HideShopMenu()
    {
        if (shopParent != null)
        {
            shopParent.SetActive(false);
        }
        else
        {
            Debug.LogWarning("missing shop parent");
        }
    }

    public void ShowShopMenu()
    {
        if (shopParent != null)
        {
            shopParent.SetActive(true);
        }
        else
        {
            Debug.LogWarning("missing shop parent");
        }
    }


    public void GenerateshopItem(ShopItem shopItem)
    {
        if (shopWrapperParent != null)
        {
            //Create Parent Slide
            shopItem.shopItemParentGameObject = Instantiate(shopItemParentRow.gameObject,
                shopItemParentRow.transform.position, shopItemParentRow.transform.rotation,
                shopWrapperParent.transform) as GameObject;

            //Create Title of slide and add name
            shopItem.shopItemTitleGameObject = Instantiate(shopItemTitle.gameObject,
                shopItemTitle.transform.position, shopItemTitle.transform.rotation,
                shopItem.shopItemParentGameObject.transform);
            shopItem.shopItemTitleUI = shopItem.shopItemTitleGameObject.GetComponent<TextMeshProUGUI>();
            shopItem.shopItemTitleUI.text = shopItem.name;
            //Create Cost of slide
            shopItem.shopItemCostGameObject = Instantiate(shopItemCost.gameObject, shopItemCost.transform.position,
                shopItemCost.transform.rotation, shopItem.shopItemParentGameObject.transform);
            shopItem.shopItemCostUI = shopItem.shopItemCostGameObject.GetComponent<TextMeshProUGUI>();

            //Create Lvl of slide
            shopItem.shopItemLevelGameObject = Instantiate(shopItemLevel.gameObject,
                shopItemLevel.transform.position, shopItemLevel.transform.rotation,
                shopItem.shopItemParentGameObject.transform);
            shopItem.shopItemLevelUI = shopItem.shopItemLevelGameObject.GetComponent<TextMeshProUGUI>();

            //Create Img of slide
            shopItem.shopItemImageGameObject = Instantiate(shopItemImage.gameObject,
                shopItemImage.transform.position, shopItemImage.transform.rotation,
                shopItem.shopItemParentGameObject.transform);
            shopItem.shopItemImageUI = shopItem.shopItemImageGameObject.GetComponent<Image>();
            
            //Create Upgrade
            shopItem.shopItemUpgradeGameObject = Instantiate(shopItemUpgrade.gameObject,
                shopItemUpgrade.transform.position, shopItemUpgrade.transform.rotation,
                shopItem.shopItemParentGameObject.transform);
            shopItem.shopItemUpgradeUI = shopItem.shopItemUpgradeGameObject.GetComponent<Button>();
            shopItem.shopItemUpgradeUI.onClick.AddListener(() =>
            {
                m_Game.Shop.UpgradeShopItem(shopItem);
            });

            //Positioning of Item in wrapper
            RectTransform rectTransform = shopItem.shopItemParentGameObject.GetComponent<RectTransform>();
            rectTransform.localPosition =
                new Vector3(
                    rectTransform.localPosition.x + (m_shopRectTransform.rect.width * (shopItem.identifier - 1)), 0, 0);
            rectTransform.sizeDelta = new Vector2(rectTransform.rect.width, m_containerRectTransform.rect.height);

            SetShopItemImage(shopItem);
            shopItem.shopItemImageGameObject.transform.localScale = new Vector3( shopItem.imageSize.x, shopItem.imageSize.y, 1);
            SetShopItemCost(shopItem, shopItem.itemActualCost);
            SetShopItemLevel(shopItem, shopItem.itemLvl);

        }
    }

    //Set Image to Item
    public void SetShopItemImage(ShopItem shopItem)
    {


        //Look for player gameobject
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null || shopItem.objectScript == null)
        {
            return;
        }

        TankController tankController = player.GetComponent<TankController>();
        string name = shopItem.objectScript.GetType().ToString().Replace("Data", String.Empty);

        BaseScriptableObjectData scriptableFound = new BaseScriptableObjectData();
        tankController.tankScriptable.baseScriptableObjects.ForEach(baseScriptableObject =>
        {
            if (baseScriptableObject.dataList.scriptableDatas.First().name.Contains(name))
                scriptableFound = baseScriptableObject;
        });


        if (shopItem.shopItemImageUI != null)
        {
            shopItem.shopItemImageUI.sprite = scriptableFound.dataList.scriptableDatas[scriptableFound.upgradeLevel].sprite ;
            shopItem.shopItemImageUI.color = scriptableFound.dataList.scriptableDatas[scriptableFound.upgradeLevel].color;
        }
    }

    //Set the sprite of next item upgrade

    public void SetShopItemImage(ShopItem shopItem, Sprite sprite, Color color)
    {
        if (shopItem.shopItemImageUI != null)
        {
            shopItem.shopItemImageUI.sprite = sprite;
            shopItem.shopItemImageUI.color = color;
        }
        else
        {
            Debug.LogWarning("Missing shopItemImageUI component" + shopItem.name);
        }


    }


    //Rotate image item when shop is displayed
    public void RotateShopImage()
    {
        //Rotate Image
        rotationEuler += Vector3.forward * itemRotationSpeed * 0.01f;
        listOfShopItems.ForEach(shopItem =>
        {
            if (shopItem.shopItemImageGameObject != null)
            {
                shopItem.shopItemImageGameObject.transform.rotation = Quaternion.Euler(rotationEuler);
            }
        });
    }

    //Set the cost of next item upgrade
    public void SetShopItemCost(ShopItem shopItem, int cost)
    {

        if (shopItem.shopItemCostUI != null)
        {
            shopItem.shopItemCostUI.text = "COST   " + cost + " GOLDS";
        }
        else
        {
            Debug.LogWarning("Missing tracksCostUI component" + shopItem.name);
        }
    }

    //Set the actual level of item
    public void SetShopItemLevel(ShopItem shopItem, int lvl)
    {

        if (shopItem.shopItemLevelUI != null)
        {
            shopItem.shopItemLevelUI.text = "Lv   " + (lvl+1);
        }
        else
        {
            Debug.LogWarning("Missing tracksLevelUI component " + shopItem.name);
        }
    }

    //Hide patern game object used to create items
    public void HideParentPatern()
    {
        shopItemParent.SetActive(false);
        shopItemParentRow.SetActive(false);
    }

    //Slide to next item
    public void SlideNext()
    {
        if (isPreviousing == false && isNexting == false)
        {
            if (elementDisplayed < maxItem)
            {
                value = m_wrapperRectTransform.localPosition.x;
                valueToReach = m_wrapperRectTransform.localPosition.x - m_shopRectTransform.rect.width;
                isNexting = true;
                elementDisplayed++;
            }
            else
            {
                value = m_wrapperRectTransform.localPosition.x;
                valueToReach = m_wrapperRectTransform.localPosition.x + (m_shopRectTransform.rect.width * (elementDisplayed - 1));
                isPreviousing = true;
                elementDisplayed = 1;
            }
        }
    }

    //Slide to previous item
    public void SlidePrevious()
    {
        if (isNexting == false && isPreviousing == false)
        {
            value = m_wrapperRectTransform.localPosition.x;
            if (elementDisplayed > 1)
            {

                valueToReach = m_wrapperRectTransform.localPosition.x + m_shopRectTransform.rect.width;
                isPreviousing = true;
                elementDisplayed--;
            }
            else
            {
                valueToReach = m_wrapperRectTransform.localPosition.x - (m_shopRectTransform.rect.width * (listOfShopItems.Count - 1));
                isNexting = true;
                elementDisplayed = listOfShopItems.Count;
            }
        }
    }

    //Slide wrapper
    public void Slide()
    {
        if (isNexting == true)
        {
            m_wrapperRectTransform.localPosition =
                new Vector3(m_wrapperRectTransform.localPosition.x - slideSpeed * 0.01f * Time.deltaTime, 0, 0);

            if (m_wrapperRectTransform.localPosition.x <= valueToReach)
            {
                m_wrapperRectTransform.localPosition = new Vector3(valueToReach, 0, 0);
                isNexting = false;
                isPreviousing = false;
            }
        }
        else if (isPreviousing == true)
        {
            m_wrapperRectTransform.localPosition =
                new Vector3(m_wrapperRectTransform.localPosition.x + slideSpeed * 0.01f * Time.deltaTime, 0, 0);

            if (m_wrapperRectTransform.localPosition.x >= valueToReach)
            {
                isPreviousing = false;
                isNexting = false;
                m_wrapperRectTransform.localPosition = new Vector3(valueToReach, 0, 0);
            }
        }
    }

    //Reload all Shop Items UI
    public void ResetShopUIManager()
    {
        m_Game.Shop.listOfShopItems.ForEach(shopItem =>
        {
            SetShopItemCost(shopItem, shopItem.itemActualCost);
            SetShopItemLevel(shopItem, shopItem.itemLvl);
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