using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeManager : MonoBehaviour 
{
    public BaseAsset lifeAsset ;
    public BaseAnimator lifeAnimator;
    public LifeController lifeController;
    private LifeData lifeData;
    private GameManager m_Game;

    public void Bind(BaseScriptableObject data)
    {
        BindData(data);
        lifeController.BindController(lifeData);
        lifeAsset.BindAssets();
        lifeAnimator.BindAnimators(lifeData.animators);
        m_Game = GameManager.Instance;
        BonusCollected bonusCollected = GetComponent<BonusCollected>();
        bonusCollected.Collided += BonusCollidedHandler;
    }

    void BonusCollidedHandler(object sender, TagEvent tagEvent)
    {
        if (tagEvent.Tag == "Player")
        {
            GameObject player = GameObject.FindWithTag("Player");
            BodyManager playerBodyManager = player.GetComponent<BodyManager>();
            playerBodyManager.bodyController.SetHealt(playerBodyManager.bodyController.GetHealt() + lifeController.GetLifeEarned());
            m_Game.Ui.SetLifeUI(playerBodyManager.bodyController.GetMaxHealt(),playerBodyManager.bodyController.GetHealt());
            Destroy(gameObject);
        }
    }
    

    void BindData(ScriptableObject obj)
    {

        if (obj.GetType() == typeof(LifeData))
        {
            lifeData = (LifeData)obj;
        }
        else
        {
            Debug.LogError("lifeManager cannot load lifeData");
        }


    }


}
