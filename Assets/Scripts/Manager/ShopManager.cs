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




public class ShopManager : MonoBehaviour
{
    
    [System.Serializable]
    public class ShopItem
    {
        [Header("Shop Item Components")]
        public int identifier;
        public int itemBaseCost;
  
        [Range(1, 3)] public float itemCostMultiplyer;
        public BaseScriptableObject objectScript;
        public UnityEvent calledWhenComponentUpgraded;
        public Vector2 imageSize = new Vector2(1, 1);
        public int imageRotationSpeed = 8;
       
        public string name;
        public string parentName;
        [TextArea(5, 10)]
        public string description;

        [HideInInspector] public Vector3 rotationEuler;
        [HideInInspector] public int itemActualCost;
        [HideInInspector] public int itemLvl;
        [HideInInspector] public TankScriptable tankDatas;
        [HideInInspector] public ShopItemPatern patern;
        
      
    }

    [System.Serializable]
    public class ShopItemPatern
    {
        [Header("Shop Patern")] 
        public GameObject shopItemParent;
        public GameObject shopItemTitle;
        public GameObject shopItemLevel;
        public GameObject shopItemCost;
        public GameObject shopItemUpgrade;
        public GameObject shopItemImage;
        public GameObject shopItemHelper;

        [HideInInspector] public TextMeshProUGUI shopItemCostUI;
        [HideInInspector] public TextMeshProUGUI shopItemLevelUI;
        [HideInInspector] public TextMeshProUGUI shopItemTitleUI;
        [HideInInspector] public Button shopItemUpgradeUI;
        [HideInInspector] public Image shopItemImageUI;
        [HideInInspector] public Button shopItemHelperButton;

    }
    
    private GameManager m_Game;
    public int golds;

    [Header("Shop Menu")] public GameObject shopParent;


    [Header("Shop Wrapper ")] public GameObject shopWrapperParent;
    [Range(1, 1000)] public int slideSpeed = 350;


    public ShopItemPatern patern;
    
    
    
    private RectTransform m_shopRectTransform;
    private RectTransform m_containerRectTransform;
    private RectTransform m_wrapperRectTransform;

    private int elementDisplayed = 1;
    private int maxItem;
    private bool isNexting = false;
    private float valueToReach;
    private float value;
    private bool isPreviousing = false;

    private List<BaseScriptableObjectData> baseScriptableObjectDatas;
   

    private int frames;

    public List<ShopItem> listOfShopItems = new List<ShopItem>();


    void Awake()
    {
        m_Game = GameManager.Instance;
        m_wrapperRectTransform = shopWrapperParent.GetComponent<RectTransform>();
        m_containerRectTransform = m_wrapperRectTransform.GetComponentInParent<RectTransform>();
        m_shopRectTransform = shopParent.GetComponent<RectTransform>();
        maxItem = listOfShopItems.OrderByDescending(S => S.identifier).FirstOrDefault().identifier;
       
    }

    private void Start()
    {
        //Alias of scriptable objects Players (List of tracks, towers, body..)
        baseScriptableObjectDatas = m_Game.Player.tankDatas.baseScriptableObjects;

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



    //Show Helper
    public void ShowHelper(ShopItem shopItem)
    {

        baseScriptableObjectDatas?.ForEach(od =>
        {
            BaseScriptableObject? baseScriptableObject = od.dataList.scriptableDatas.FirstOrDefault();
            if (!baseScriptableObject)
                Debug.LogError("Scriptable data of " + od.GetType().Name + " is empty list"); return;

            if (baseScriptableObject.GetType().Name.Contains(shopItem.name))
            {

            }
            

            //switch (baseScriptableObject.GetType().Name)
            //{
            //    case "TowerData":

            //        break;
            //    case "BodyData":

            //        BodyData bodyDataActual = (BodyData)od.dataList.scriptableDatas[od.upgradeLevel];
            //        BodyData bodyDataOne = (BodyData)od.dataList.scriptableDatas.First();
            //        float bodyMaxLifePercent = bodyDataActual.life * 100f / bodyDataOne.life;
            //        m_Game.Helper.ShowPopup(String.Format(shopItem.description, od.upgradeLevel, shopItem.itemCostMultiplyer, bodyMaxLifePercent, bodyDataActual.life), shopItem.name);


            //        break;
            //    case "TracksData":

            //        TracksData tracksDataActual = (TracksData)od.dataList.scriptableDatas[od.upgradeLevel];
            //        TracksData tracksDataOne = (TracksData)od.dataList.scriptableDatas.First();
            //        float tracksRotationSpeedPercent = tracksDataActual.rotationSpeed * 100f / tracksDataOne.rotationSpeed;
            //        float tracksSpeedPercent = tracksDataActual.speed * 100f / tracksDataOne.speed;
            //        m_Game.Helper.ShowPopup(String.Format(shopItem.description, od.upgradeLevel, shopItem.itemCostMultiplyer, tracksSpeedPercent, tracksRotationSpeedPercent), shopItem.name);


            //        break;
            //    case "GunData":

            //        GunData gunDataActual = (GunData)od.dataList.scriptableDatas[od.upgradeLevel];
            //        GunData gunDataOne = (GunData)od.dataList.scriptableDatas.First();
            //        float gunBulletSpeed= gunDataActual.bulletVelocity * 100f / gunDataOne.bulletVelocity;
            //        float gunReloadTime = gunDataActual.reloadTimeSecond * 100f / gunDataOne.reloadTimeSecond;
            //        m_Game.Helper.ShowPopup(String.Format(shopItem.description, od.upgradeLevel, shopItem.itemCostMultiplyer, gunBulletSpeed, gunReloadTime), shopItem.name);

            //        break;
            //    case "BulletData":

            //        BulletData bulletDataActual = (BulletData)od.dataList.scriptableDatas[od.upgradeLevel];
            //        BulletData bulletDataOne = (BulletData)od.dataList.scriptableDatas.First();
            //        float bulletDamage = bulletDataActual.damage;
            //        float bulletBounce = bulletDataActual.maxBounce;
            //        m_Game.Helper.ShowPopup(String.Format(shopItem.description, od.upgradeLevel, shopItem.itemCostMultiplyer, bulletDamage, bulletBounce), shopItem.name);

            //        break;
               
            //    default:

            //        break;
            //}


           

        });

       
    }

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


            BaseScriptableObjectData objectData = tankController.Upgrade(name, shopItem.parentName);


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
            SetShopItemImage(shopItem);
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
            shopItem.patern.shopItemParent = Instantiate(patern.shopItemParent.gameObject,
                patern.shopItemParent.transform.position, patern.shopItemParent.transform.rotation,
                shopWrapperParent.transform) as GameObject;

            foreach (Transform child in shopItem.patern.shopItemParent.transform) {
                Destroy(child.gameObject);
            }
            
            
            //Create Title of slide and add name
            shopItem.patern.shopItemTitle = Instantiate(patern.shopItemTitle.gameObject,
                patern.shopItemTitle.transform.position, patern.shopItemTitle.transform.rotation,
                shopItem.patern.shopItemParent.transform);
            shopItem.patern.shopItemTitleUI = shopItem.patern.shopItemTitle.GetComponent<TextMeshProUGUI>();
            shopItem.patern.shopItemTitleUI.text = shopItem.name;
            //Create Cost of slide
            shopItem.patern.shopItemCost = Instantiate(patern.shopItemCost.gameObject, patern.shopItemCost.transform.position,
                patern.shopItemCost.transform.rotation, shopItem.patern.shopItemParent.transform);
            shopItem.patern.shopItemCostUI = shopItem.patern.shopItemCost.GetComponent<TextMeshProUGUI>();

            //Create Lvl of slide
            shopItem.patern.shopItemLevel = Instantiate(patern.shopItemLevel.gameObject,
                patern.shopItemLevel.transform.position, patern.shopItemLevel.transform.rotation,
                shopItem.patern.shopItemParent.transform);
            shopItem.patern.shopItemLevelUI = shopItem.patern.shopItemLevel.GetComponent<TextMeshProUGUI>();

            //Create Img of slide
            shopItem.patern.shopItemImage = Instantiate(patern.shopItemImage.gameObject,
                patern.shopItemImage.transform.position, patern.shopItemImage.transform.rotation,
                shopItem.patern.shopItemParent.transform);
            shopItem.patern.shopItemImageUI = shopItem.patern.shopItemImage.GetComponent<Image>();
            
            //Create Upgrade
            shopItem.patern.shopItemUpgrade = Instantiate(patern.shopItemUpgrade.gameObject,
                patern.shopItemUpgrade.transform.position, patern.shopItemUpgrade.transform.rotation,
                shopItem.patern.shopItemParent.transform);
            shopItem.patern.shopItemUpgradeUI = shopItem.patern.shopItemUpgrade.GetComponent<Button>();
            shopItem.patern.shopItemUpgradeUI.onClick.AddListener(() =>
            {
                UpgradeShopItem(shopItem);
            });

            //Create Helper
            shopItem.patern.shopItemHelper = Instantiate(patern.shopItemHelper.gameObject,
                patern.shopItemHelper.transform.position, patern.shopItemHelper.transform.rotation,
                shopItem.patern.shopItemParent.transform);
            shopItem.patern.shopItemHelperButton = shopItem.patern.shopItemHelper.GetComponent<Button>();
            shopItem.patern.shopItemHelperButton.onClick.AddListener(() =>
            {
                ShowHelper(shopItem);
            });
            



            //Positioning of Item in wrapper
            RectTransform rectTransform = shopItem.patern.shopItemParent.GetComponent<RectTransform>();
            rectTransform.localPosition =
                new Vector3(
                    rectTransform.localPosition.x + (m_shopRectTransform.rect.width * (shopItem.identifier - 1)), 0, 0);
            rectTransform.sizeDelta = new Vector2(rectTransform.rect.width, m_containerRectTransform.rect.height);

            SetShopItemImage(shopItem);
            shopItem.patern.shopItemImage.transform.localScale = new Vector3( shopItem.imageSize.x, shopItem.imageSize.y, 1);
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


        if (shopItem.patern.shopItemImageUI != null)
        {
            shopItem.patern.shopItemImageUI.sprite = scriptableFound.dataList.scriptableDatas[scriptableFound.upgradeLevel].sprite ;
            shopItem.patern.shopItemImageUI.color = scriptableFound.dataList.scriptableDatas[scriptableFound.upgradeLevel].color;
        }
    }

    //Set the sprite of next item upgrade

    public void SetShopItemImage(ShopItem shopItem, Sprite sprite, Color color)
    {
        if (shopItem.patern.shopItemImageUI != null)
        {
            shopItem.patern.shopItemImageUI.sprite = sprite;
            shopItem.patern.shopItemImageUI.color = color;
        }
        else
        {
            Debug.LogWarning("Missing shopItemImageUI component" + shopItem.name);
        }


    }


    //Rotate image item when shop is displayed
    public void RotateShopImage()
    {
    
        listOfShopItems.ForEach(shopItem =>
        {
            //Rotate Image
            shopItem.rotationEuler += Vector3.forward * shopItem.imageRotationSpeed * 0.01f;
            if (shopItem.patern.shopItemImage != null)
            {
                shopItem.patern.shopItemImage.transform.rotation = Quaternion.Euler(shopItem.rotationEuler);
            }
        });
    }

    //Set the cost of next item upgrade
    public void SetShopItemCost(ShopItem shopItem, int cost, string prePhrase = null , string postPhrase = null)
    {

        if (shopItem.patern.shopItemCostUI != null)
        {
            
            string text = "";
            text += prePhrase == null ? "COST    " : prePhrase;
            text += cost.ToString();
            text += postPhrase  == null ? " GOLDS" : postPhrase;
            
            shopItem.patern.shopItemCostUI.text = text;
        }
        else
        {
            Debug.LogWarning("Missing tracksCostUI component" + shopItem.name);
        }
    }

    //Set the actual level of item
    public void SetShopItemLevel(ShopItem shopItem, int lvl, string prePhrase = null , string postPhrase = null)
    {

        if (shopItem.patern.shopItemLevelUI != null)
        {
            string text = "";
            text += prePhrase == null ? "Lv " : prePhrase;
            text += (lvl + 1).ToString();
            text += postPhrase  == null ? "" : postPhrase;
            
            shopItem.patern.shopItemLevelUI.text = text;
        }
        else
        {
            Debug.LogWarning("Missing shopItemLevelUI component " + shopItem.name);
        }
    }

    //Hide patern game object used to create items
    public void HideParentPatern()
    {
         patern.shopItemParent.SetActive(false);
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
                new Vector3(m_wrapperRectTransform.localPosition.x - slideSpeed * 10 * Time.deltaTime, 0, 0);

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
                new Vector3(m_wrapperRectTransform.localPosition.x + slideSpeed * 10 * Time.deltaTime, 0, 0);

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
        listOfShopItems.ForEach(shopItem =>
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