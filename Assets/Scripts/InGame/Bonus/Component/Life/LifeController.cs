using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LifeController 
{

    public SpriteRenderer lifeSpriteRenderer;
  
    private Life m_life = new Life();
    private int lifeEarned;

    public void BindController(LifeData lifeData)
    {
        m_life.LoadData(lifeData);
        BindComponent();
        BindStats();
    }


    void BindComponent()
    {

        if (m_life.Data != null)
        {

            if (lifeSpriteRenderer != null  && m_life != null)
            {

            
                lifeSpriteRenderer.color = m_life.Data.color;
                lifeSpriteRenderer.sprite = m_life.Data.sprite;

            }
        }
        else
        {
            Debug.LogError("lifeController cannot load Data in life");
        }
    }

    void BindStats()
    {

        if (m_life.Data != null)
        {
            lifeEarned = m_life.Data.lifeEarned;
        }
        else
        {
            Debug.LogError("lifeController cannot load Data in life");
        }
    }


    public void SetLifeEarned(int newValue)
    {
        lifeEarned = newValue;
    }

    public int GetLifeEarned()
    {
        return lifeEarned;
    }


    public LifeData GetBaseData()
    {
        return (LifeData)m_life.GetBaseData();
    }
}
