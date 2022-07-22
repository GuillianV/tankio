using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldManager : MonoBehaviour ,IBonusManager
{
    public BaseAsset goldAsset ;
    public BaseAnimator goldAnimator;
    public GoldController goldController;
    private GoldData goldData;
    private GameManager m_Game;

    public void Bind(BaseScriptableObject data)
    {
        BindData(data);
        goldController.BindController(goldData);
        goldAsset.BindAssets();
        goldAnimator.BindAnimators(goldData.animators);
        m_Game = GameManager.Instance;
        BonusCollected bonusCollected = GetComponent<BonusCollected>();
        bonusCollected.Collided += BonusCollidedHandler;
    }

    void BonusCollidedHandler(object sender, TagEvent tagEvent)
    {
        if (tagEvent.Tag == "Player")
        {
            GameObject player = GameObject.FindWithTag("Player");
            m_Game.Shop.golds += goldController.GetGoldEarned();
            m_Game.Ui.SetGoldUI(m_Game.Shop.golds);
            Destroy(gameObject);
        }
    }
    

    void BindData(ScriptableObject obj)
    {

        if (obj.GetType() == typeof(GoldData))
        {
            goldData = (GoldData)obj;
        }
        else
        {
            Debug.LogError("goldManager cannot load goldData");
        }


    }


}
