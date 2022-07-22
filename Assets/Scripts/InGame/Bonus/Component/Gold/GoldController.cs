using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GoldController 
{

    public SpriteRenderer goldSpriteRenderer;
  
    private Gold m_gold = new Gold();
    private int goldEarned;

    public void BindController(GoldData goldData)
    {
        m_gold.LoadData(goldData);
        BindComponent();
        BindStats();
    }


    void BindComponent()
    {

        if (m_gold.Data != null)
        {

            if (goldSpriteRenderer != null  && m_gold != null)
            {

            
                goldSpriteRenderer.color = m_gold.Data.color;
                goldSpriteRenderer.sprite = m_gold.Data.sprite;

            }
        }
        else
        {
            Debug.LogError("goldController cannot load Data in gold");
        }
    }

    void BindStats()
    {

        if (m_gold.Data != null)
        {
            goldEarned = m_gold.Data.goldEarned;
        }
        else
        {
            Debug.LogError("goldController cannot load Data in gold");
        }
    }


    public void SetGoldEarned(int newValue)
    {
        goldEarned = newValue;
    }

    public int GetGoldEarned()
    {
        return goldEarned;
    }


    public GoldData GetBaseData()
    {
        return (GoldData)m_gold.GetBaseData();
    }
}
