using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ShopItemUI
{
    [Header("Shop Item Components")] [SerializeField]
    public int identifier;

    [SerializeField] public string name;
    [SerializeField] public Sprite imageSprite;
    [SerializeField] public Color imageColor;
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


public class UIManager : MonoBehaviour
{
    [Header("In Game Menu")] public TextMeshProUGUI lifeUI;
    public TextMeshProUGUI goldsUI;
    public TextMeshProUGUI waveUI;
    public GameObject inGameParent;

    [Header("Paused Menu")] public GameObject pausedParent;

    [Header("Dead Menu")]
    public GameObject deadParent;
    public Image deadBackgroundImage;
    public TextMeshProUGUI deadButton;
    public TextMeshProUGUI deadTitle;
    private float deadTimeLerp = 0;
    private float fadeSpeed = 0.25f;
    private bool isDead = false;
    
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


    public List<ShopItemUI> listOfShopItemUI = new List<ShopItemUI>();


    private int elementDisplayed = 1;
    private int maxItem;
    private bool isNexting = false;
    private float valueToReach;
    private float value;
    private bool isPreviousing = false;
    private GameManager m_Game;

    private Vector3 rotationEuler;
    // Start is called before the first frame update

    
    public float updateInterval = 0.5F;
    private double lastInterval;
    private int frames;


 
    
    private void Awake()
    {
        m_Game = GameManager.Instance;
        m_wrapperRectTransform = shopWrapperParent.GetComponent<RectTransform>();
        m_containerRectTransform = m_wrapperRectTransform.GetComponentInParent<RectTransform>();
        m_shopRectTransform = shopParent.GetComponent<RectTransform>();
        m_shopItemParentRowRectTransform = shopItemParentRow.GetComponent<RectTransform>();
        maxItem = listOfShopItemUI.OrderByDescending(S => S.identifier).FirstOrDefault().identifier;

       
    }

    void Start()
    {
       
        listOfShopItemUI.ForEach(shopItemUIGet => { GenerateShopItemUI(shopItemUIGet); });
        shopParent.SetActive(false);
        HideParentPatern();
        
        lastInterval = Time.realtimeSinceStartup;
        frames = 0;
        
        
        

    }

    void Update()
    {
        
  
        if (shopParent.activeSelf)
        {
            RotateShopImage();
            Slide();
        }


        if (isDead)
        {
            SetBackgroundOpacityToBlack();
            SetDeadItemOpacityToWhite();
        }
        
       

    }

  

    #region Pause Menu

    public void TogglePausedMenu()
    {
        if (pausedParent != null)
        {
            if (pausedParent.activeSelf)
            {
                pausedParent.SetActive(false);
            }
            else
            {
                pausedParent.SetActive(true);
            }
        }
        else
        {
            Debug.LogWarning("missing in game parent");
        }
    }
    
    
    public void HidePausedMenu()
    {
        if (pausedParent != null)
        {
            pausedParent.SetActive(false);
        }
        else
        {
            Debug.LogWarning("missing in game parent");
        }
    }
    
    public void ShowPausedMenu()
    {
        if (pausedParent != null)
        {
            pausedParent.SetActive(true);
        }
        else
        {
            Debug.LogWarning("missing in game parent");
        }
    }
    

    #endregion


    #region InGame Menu

    public void ToggleInGameMenu()
    {
        if (inGameParent != null)
        {
            if (inGameParent.activeSelf)
            {
                inGameParent.SetActive(false);
            }
            else
            {
                inGameParent.SetActive(true);
            }
        }
        else
        {
            Debug.LogWarning("missing in game parent");
        }
    }

    public void SetLifeUI(float maxLife, float life)
    {
        if (lifeUI != null)
        {
            lifeUI.text = "LIFE : " + (life / maxLife * 100f).ToString("F0") + "%";
        }
        else
        {
            Debug.LogWarning("Missing Life component");
        }
    }

    public void SetGoldUI(int golds)
    {
        if (goldsUI != null)
        {
            goldsUI.text = "GOLDS : " + golds;
        }
        else
        {
            Debug.LogWarning("Missing Gold component");
        }
    }

    public void SetWaveUI(int wave)
    {
        if (waveUI != null)
        {
            waveUI.text = "WAVE : " + wave;
        }
        else
        {
            Debug.LogWarning("Missing wave component");
        }
    }

    #endregion


    #region Shop Menu

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
            Debug.LogWarning("missing in game parent");
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
            Debug.LogWarning("missing in game parent");
        }
    }


    public void GenerateShopItemUI(ShopItemUI shopItemUIGet)
    {
        if (shopWrapperParent != null)
        {
            //Create Parent Slide
            shopItemUIGet.shopItemParentGameObject = Instantiate(shopItemParentRow.gameObject,
                shopItemParentRow.transform.position, shopItemParentRow.transform.rotation,
                shopWrapperParent.transform) as GameObject;
            
            //Create Title of slide and add name
            shopItemUIGet.shopItemTitleGameObject = Instantiate(shopItemTitle.gameObject,
                shopItemTitle.transform.position, shopItemTitle.transform.rotation,
                shopItemUIGet.shopItemParentGameObject.transform);
            shopItemUIGet.shopItemTitleUI = shopItemUIGet.shopItemTitleGameObject.GetComponent<TextMeshProUGUI>();
            shopItemUIGet.shopItemTitleUI.text = shopItemUIGet.name;
            //Create Cost of slide
            shopItemUIGet.shopItemCostGameObject = Instantiate(shopItemCost.gameObject, shopItemCost.transform.position,
                shopItemCost.transform.rotation, shopItemUIGet.shopItemParentGameObject.transform);
            shopItemUIGet.shopItemCostUI = shopItemUIGet.shopItemCostGameObject.GetComponent<TextMeshProUGUI>();

            //Create Lvl of slide
            shopItemUIGet.shopItemLevelGameObject = Instantiate(shopItemLevel.gameObject,
                shopItemLevel.transform.position, shopItemLevel.transform.rotation,
                shopItemUIGet.shopItemParentGameObject.transform);
            shopItemUIGet.shopItemLevelUI = shopItemUIGet.shopItemLevelGameObject.GetComponent<TextMeshProUGUI>();

            //Create Img of slide
            shopItemUIGet.shopItemImageGameObject = Instantiate(shopItemImage.gameObject,
                shopItemImage.transform.position, shopItemImage.transform.rotation,
                shopItemUIGet.shopItemParentGameObject.transform);
            shopItemUIGet.shopItemImageUI = shopItemUIGet.shopItemImageGameObject.GetComponent<Image>();
            SetShopItemImage(shopItemUIGet.identifier);

            //Create Upgrade
            shopItemUIGet.shopItemUpgradeGameObject = Instantiate(shopItemUpgrade.gameObject,
                shopItemUpgrade.transform.position, shopItemUpgrade.transform.rotation,
                shopItemUIGet.shopItemParentGameObject.transform);
            shopItemUIGet.shopItemUpgradeUI = shopItemUIGet.shopItemUpgradeGameObject.GetComponent<Button>();
            shopItemUIGet.shopItemUpgradeUI.onClick.AddListener(() =>
            {
                m_Game.Shop.UpgradeShopItem(shopItemUIGet.identifier);
            });

            //Positioning of Item in wrapper
            RectTransform rectTransform = shopItemUIGet.shopItemParentGameObject.GetComponent<RectTransform>();
            rectTransform.localPosition =
                new Vector3(
                    rectTransform.localPosition.x + ( m_shopRectTransform.rect.width  * (shopItemUIGet.identifier - 1)), 0, 0);
             rectTransform.sizeDelta = new Vector2(rectTransform.rect.width,m_containerRectTransform.rect.height );
           
        }
    }

    //Set Image to Item
    public void SetShopItemImage(int identifier)
    {
        ShopItemUI shopItemUI = listOfShopItemUI.FirstOrDefault(I => I.identifier == identifier);

        if (shopItemUI.shopItemImageUI != null)
        {
            shopItemUI.shopItemImageUI.sprite = shopItemUI.imageSprite;
            shopItemUI.shopItemImageUI.color = shopItemUI.imageColor;
        }
        else
        {
            Debug.LogWarning("Missing shopItemImageUI component" + identifier);
        }
    }

    //Set Image to Item with new sprite and color
    public void SetShopItemImage(int identifier, Sprite sprite, Color color)
    {
        ShopItemUI shopItemUI = listOfShopItemUI.FirstOrDefault(I => I.identifier == identifier);

        if (shopItemUI.shopItemImageUI != null)
        {
            shopItemUI.shopItemImageUI.sprite = sprite;
            shopItemUI.shopItemImageUI.color = color;
        }
        else
        {
            Debug.LogWarning("Missing shopItemImageUI component" + identifier);
        }
    }

    //Rotate image item when shop is displayed
    public void RotateShopImage()
    {
        //Rotate Image
        rotationEuler += Vector3.forward * itemRotationSpeed * 0.01f;
        listOfShopItemUI.ForEach(I =>
        {
            if (I.shopItemImageGameObject != null)
            {
                I.shopItemImageGameObject.transform.rotation = Quaternion.Euler(rotationEuler);
            }
        });
    }

    //Set the cost of next item upgrade
    public void SetShopItemCost(int identifier, int cost)
    {
        ShopItemUI shopItemUI = listOfShopItemUI.FirstOrDefault(I => I.identifier == identifier);

        if (shopItemUI.shopItemCostUI != null)
        {
            shopItemUI.shopItemCostUI.text = "COST : " + cost + " GOLDS";
        }
        else
        {
            Debug.LogWarning("Missing tracksCostUI component" + identifier);
        }
    }

    //Set the actual level of item
    public void SetShopItemLevel(int identifier, int lvl)
    {
        ShopItemUI shopItemUI = listOfShopItemUI.FirstOrDefault(I => I.identifier == identifier);


        if (shopItemUI.shopItemLevelUI != null)
        {
            shopItemUI.shopItemLevelUI.text = "Lv : " + lvl;
        }
        else
        {
            Debug.LogWarning("Missing tracksLevelUI component " + identifier);
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
        }
    }

    //Slide to previous item
    public void SlidePrevious()
    {
        if (isNexting == false && isPreviousing == false)
        {
            if (elementDisplayed > 1)
            {
                value = m_wrapperRectTransform.localPosition.x;
                valueToReach = m_wrapperRectTransform.localPosition.x +  m_shopRectTransform.rect.width;
                isPreviousing = true;
                elementDisplayed--;
            }
        }
    }

    //Slide wrapper
    public void Slide()
    {
        if (m_wrapperRectTransform.localPosition.x > valueToReach && isNexting == true)
        {
            m_wrapperRectTransform.localPosition =
                new Vector3(m_wrapperRectTransform.localPosition.x - slideSpeed  * 0.01f  *Time.deltaTime, 0, 0);

            if (m_wrapperRectTransform.localPosition.x <= valueToReach)
            {
                m_wrapperRectTransform.localPosition = new Vector3(valueToReach , 0, 0);
                isNexting = false;
                isPreviousing = false;
            }
        }
        else if (m_wrapperRectTransform.localPosition.x < valueToReach && isPreviousing == true)
        {
            m_wrapperRectTransform.localPosition =
                new Vector3(m_wrapperRectTransform.localPosition.x + slideSpeed  * 0.01f  *Time.deltaTime, 0, 0);

            if (m_wrapperRectTransform.localPosition.x >= valueToReach)
            {
                isPreviousing = false;
                isNexting = false;
                m_wrapperRectTransform.localPosition = new Vector3(valueToReach , 0, 0);
            }
        }
    }

    //Reload all Shop Items UI
    public void ResetShopUIManager()
    {
        m_Game.Shop.listOfShopItems.ForEach(shopItem =>
        {
            SetShopItemCost(shopItem.identifier, shopItem.itemActualCost);
            SetShopItemLevel(shopItem.identifier, shopItem.itemLvl);
        });
        
        
    }

    #endregion

    #region Dead Menu

    public void ToggleDeadMenu()
    {
        if (deadParent != null)
        {
            if (deadParent.activeSelf)
            {
                deadParent.SetActive(false);
            }
            else
            {
                deadParent.SetActive(true);
            }
        }
        else
        {
            Debug.LogWarning("missing dead parent");
        }
    }
    
    public void HideDeadMenu()
    {
        if (deadParent != null)
        {
            deadParent.SetActive(false);
            deadTimeLerp = 0;
            isDead = false;
        }
        else
        {
            Debug.LogWarning("missing dead parent");
        }
    }
    
    public void ShowDeadMenu()
    {
        if (deadParent != null)
        {
            deadParent.SetActive(true);
            isDead = true;
        }
        else
        {
            Debug.LogWarning("missing dead parent");
        }
    }

    public void SetBackgroundOpacityToBlack()
    {
        deadTimeLerp += fadeSpeed * Time.deltaTime  *  m_Game.TimeManager.timeScale;
        
        var color = deadBackgroundImage.color;
        var newColor = new Color(color.r, color.g, color.b,  Mathf.Lerp(0, 1, deadTimeLerp));
        deadBackgroundImage.color = newColor;
       
        if (deadTimeLerp > 1.0f)
        {
            deadTimeLerp = 1;
        }
        

    }
    
    public void SetBackgroundOpacityToWhite()
    {
        deadTimeLerp += fadeSpeed * Time.deltaTime *  m_Game.TimeManager.timeScale;
        
        var color = deadBackgroundImage.color;
        var newColor = new Color(color.r, color.g, color.b,  Mathf.Lerp(1, 0, deadTimeLerp));
        deadBackgroundImage.color = newColor;
       
        if (deadTimeLerp > 1.0f)
        {
            deadTimeLerp = 1;
        }
        

    }

    public void SetDeadItemOpacityToWhite()
    {
        deadTimeLerp += fadeSpeed * Time.deltaTime  *  m_Game.TimeManager.timeScale;

        
        
        var colorButton =deadButton.color;
        var newColorButton = new Color(colorButton.r, colorButton.g, colorButton.b,  Mathf.Lerp(0, 1, deadTimeLerp));
        deadButton.color = newColorButton;
       
        var colorTitle =deadTitle.color;
        var newColorTitle = new Color(colorTitle.r, colorTitle.g, colorTitle.b,  Mathf.Lerp(0, 1, deadTimeLerp));
        deadTitle.color = newColorTitle;

        
        if (deadTimeLerp > 1.0f)
        {
            deadTimeLerp = 1;
        }
        

    }



    #endregion
    
}