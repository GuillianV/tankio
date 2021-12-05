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
    
    [Header("Shop Item Components")]
    [SerializeField]
    public int identifier;
    [SerializeField]
    public string name;
    [SerializeField]
    public Sprite imageSprite;
    [SerializeField]
    public Color imageColor;
    [HideInInspector]
    public TextMeshProUGUI shopItemCostUI;
    [HideInInspector]
    public TextMeshProUGUI shopItemLevelUI;
    [HideInInspector]
    public TextMeshProUGUI shopItemTitleUI;
    [HideInInspector]
    public Button shopItemUpgradeUI;
    [HideInInspector]
    public Image shopItemImageUI;
    [HideInInspector]
    public GameObject shopItemCostGameObject;
    [HideInInspector]
    public GameObject shopItemLevelGameObject;
    [HideInInspector]
    public GameObject shopItemImageGameObject;
    [HideInInspector]
    public GameObject shopItemTitleGameObject;
    [HideInInspector]
    public GameObject shopItemUpgradeGameObject;
    [HideInInspector]
    public GameObject shopItemParentGameObject;
   
}



public class UIManager : MonoBehaviour
{
    [Header("In Game Menu")]
    public TextMeshProUGUI lifeUI;
    public TextMeshProUGUI goldsUI;
    public TextMeshProUGUI waveUI;
    public GameObject inGameParent;
    
    [Header("Paused Menu")]
    public GameObject pausedParent;
    
    
    [Header("Shop Menu")] 
    public GameObject shopParent;
   

    [Header("Shop Wrapper ")] 
    public GameObject shopWrapperParent;
    [Range(100,1000)]
    public int slideSpeed = 350;
    private RectTransform m_wrapperRectTransform;
    
    [Header("Shop Patern Slide")]
    public GameObject shopItemParentRow;
    public RectTransform m_shopItemParentRowRectTransform;
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

    private void Awake()
    {
        
        m_Game = GameManager.Instance;
        m_wrapperRectTransform = shopWrapperParent.GetComponent<RectTransform>();
        m_shopItemParentRowRectTransform = shopItemParentRow.GetComponent<RectTransform>();
        maxItem = listOfShopItemUI.OrderByDescending(S => S.identifier).FirstOrDefault().identifier;

        listOfShopItemUI.ForEach(shopItemUIGet =>
        {
            GenerateShopItemUI(shopItemUIGet);
        });
    }

    void Start()
    {

        HideParentPatern();
        
        
        
    }

   
   
    void Update(){

        if (shopParent.activeSelf)
        {

            RotateShopImage();
            Slide();
         
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
            lifeUI.text = "LIFE : "+ (life / maxLife * 100f)+"%";
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
            goldsUI.text = "GOLDS : "+ golds;
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
            waveUI.text = "WAVE : "+ wave;
        }
        else
        {
            Debug.LogWarning("Missing wave component");
        }
    }


    #endregion

   

    #region Shop Menu

    
    
    public void GenerateShopItemUI(ShopItemUI shopItemUIGet)
    {
       // ShopItemUI shopItemUIGet = listOfShopItemUI.FirstOrDefault(I => I.identifier == identifier);
        
        
        if (shopWrapperParent != null)
        {
            //Create Parent Slide
            shopItemUIGet.shopItemParentGameObject = Instantiate(shopItemParentRow.gameObject, shopItemParentRow.transform.position, shopItemParentRow.transform.rotation, shopWrapperParent.transform) as GameObject ;
            
            //Create Title of slide and add name
            shopItemUIGet.shopItemTitleGameObject = Instantiate(shopItemTitle.gameObject, shopItemTitle.transform.position, shopItemTitle.transform.rotation, shopItemUIGet.shopItemParentGameObject.transform);
            shopItemUIGet.shopItemTitleUI = shopItemUIGet.shopItemTitleGameObject.GetComponent<TextMeshProUGUI>();
            shopItemUIGet.shopItemTitleUI.text = shopItemUIGet.name;
            //Create Cost of slide
            shopItemUIGet.shopItemCostGameObject = Instantiate(shopItemCost.gameObject, shopItemCost.transform.position, shopItemCost.transform.rotation, shopItemUIGet.shopItemParentGameObject.transform);
            shopItemUIGet.shopItemCostUI = shopItemUIGet.shopItemCostGameObject.GetComponent<TextMeshProUGUI>();

            //Create Lvl of slide
            shopItemUIGet.shopItemLevelGameObject = Instantiate(shopItemLevel.gameObject, shopItemLevel.transform.position, shopItemLevel.transform.rotation, shopItemUIGet.shopItemParentGameObject.transform);
            shopItemUIGet.shopItemLevelUI = shopItemUIGet.shopItemLevelGameObject.GetComponent<TextMeshProUGUI>();
         
            //Create Img of slide
            shopItemUIGet.shopItemImageGameObject = Instantiate(shopItemImage.gameObject, shopItemImage.transform.position, shopItemImage.transform.rotation, shopItemUIGet.shopItemParentGameObject.transform);
            shopItemUIGet.shopItemImageUI = shopItemUIGet.shopItemImageGameObject.GetComponent<Image>();
            SetShopItemImage(shopItemUIGet.identifier);
            
            //Create Upgrade
            shopItemUIGet.shopItemUpgradeGameObject = Instantiate(shopItemUpgrade.gameObject, shopItemUpgrade.transform.position, shopItemUpgrade.transform.rotation, shopItemUIGet.shopItemParentGameObject.transform);
            shopItemUIGet.shopItemUpgradeUI = shopItemUIGet.shopItemUpgradeGameObject.GetComponent<Button>();
            shopItemUIGet.shopItemUpgradeUI.onClick.AddListener(() => {
                m_Game.Shop.UpgradeShopItem(shopItemUIGet.identifier);
            });

            RectTransform rectTransform = shopItemUIGet.shopItemParentGameObject.GetComponent<RectTransform>();
            rectTransform.localPosition = new Vector3(rectTransform.localPosition.x + (rectTransform.sizeDelta.x * (shopItemUIGet.identifier-1)), 0, 0);

        }
    }

    
    public void SetShopItemImage(int identifier )
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
    
    public void SetShopItemImage(int identifier ,Sprite sprite, Color color)
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

    public void RotateShopImage()
    {
        //Rotate Image
        rotationEuler+= Vector3.forward*itemRotationSpeed*0.01f;
        listOfShopItemUI.ForEach(I =>
        {
            if (I.shopItemImageGameObject != null)
            {
                I.shopItemImageGameObject.transform.rotation = Quaternion.Euler(rotationEuler);
            }
                
             
        });
    }
    
    
    public void SetShopItemCost(int identifier,int cost)
    {

        ShopItemUI shopItemUI = listOfShopItemUI.FirstOrDefault(I => I.identifier == identifier);
        
        if (shopItemUI.shopItemCostUI != null)
        {
            shopItemUI.shopItemCostUI.text = "COST : " +cost+ " GOLDS" ;
        }
        else
        {
            Debug.LogWarning("Missing tracksCostUI component" + identifier);
        }
    }
    
    public void SetShopItemLevel(int identifier,int lvl)
    {
        ShopItemUI shopItemUI = listOfShopItemUI.FirstOrDefault(I => I.identifier == identifier);

        
        if (shopItemUI.shopItemLevelUI != null)
        {
            shopItemUI.shopItemLevelUI.text = "Lv : "+ lvl;
        }
        else
        {
            Debug.LogWarning("Missing tracksLevelUI component "+ identifier);
        }
    }


    public void HideParentPatern()
    {
        shopItemParent.SetActive(false);
        shopItemParentRow.SetActive(false);

    }

    public void SlideNext()
    {

        if (isPreviousing == false && isNexting == false)
        {

           
            if (elementDisplayed < maxItem)
            {
                value = m_wrapperRectTransform.localPosition.x;
                valueToReach = m_wrapperRectTransform.localPosition.x - m_shopItemParentRowRectTransform.sizeDelta.x;
                isNexting = true;
                elementDisplayed++;
            }
            
     

        }
        
       
    }
    
    public void SlidePrevious()
    {
        if (isNexting == false && isPreviousing == false)
        {

            if (elementDisplayed > 1)
            {
                value = m_wrapperRectTransform.localPosition.x;
                valueToReach = m_wrapperRectTransform.localPosition.x + m_shopItemParentRowRectTransform.sizeDelta.x;
                isPreviousing = true;
                elementDisplayed--;
            }
            
        }
    }


    public void Slide()
    {
        if (m_wrapperRectTransform.localPosition.x  > valueToReach  && isNexting == true)
        {
            m_wrapperRectTransform.localPosition = new Vector3(m_wrapperRectTransform.localPosition.x - slideSpeed * 0.01f, 0, 0);
      
            if ( m_wrapperRectTransform.localPosition.x  <= valueToReach )
            {
                m_wrapperRectTransform.localPosition = new Vector3(valueToReach, 0, 0);
                isNexting = false;
                isPreviousing = false;
            }
                
        }
        else if (m_wrapperRectTransform.localPosition.x  < valueToReach && isPreviousing == true )
        {
            m_wrapperRectTransform.localPosition = new Vector3(m_wrapperRectTransform.localPosition.x + slideSpeed * 0.01f, 0, 0);

            if (m_wrapperRectTransform.localPosition.x >= valueToReach)
            {
                isPreviousing = false;
                isNexting = false;
                m_wrapperRectTransform.localPosition = new Vector3(valueToReach, 0, 0);
            }
        }
    }
    

    #endregion
}
