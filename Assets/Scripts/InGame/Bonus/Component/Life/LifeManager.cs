using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeManager : MonoBehaviour ,IBonusManager
{
    public BaseAsset lifeAsset ;
    public BaseAnimator lifeAnimator;
    public LifeController lifeController;
    public Vector2 globalMapPosition = Vector2.zero;
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

    public void BindMapPos(Vector2 pos)
    {

        globalMapPosition = pos;

    }
    
    void BonusCollidedHandler(object sender, MapEvent mapEvent)
    {
        if (mapEvent.Tag == "Player")
        {
            GameObject player = GameObject.FindWithTag("Player");
            BodyManager playerBodyManager = player.GetComponent<BodyManager>();
            playerBodyManager.bodyController.SetHealt(playerBodyManager.bodyController.GetHealt() + lifeController.GetLifeEarned());
            m_Game.Ui.SetLifeUI(playerBodyManager.bodyController.GetMaxHealt(),playerBodyManager.bodyController.GetHealt());
            m_Game.Map.SetMapUnused(globalMapPosition);
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
