using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BodyController 
{


    public SpriteRenderer bodySprite;

    private Body m_body = new Body();
    private float maxHealth;
    private float health;
    private int gold;


    public void BindController(BodyData bodyData)
    {
        m_body.LoadData(bodyData);
        BindComponent();
        BindStats();
    }

   

    void BindComponent()
    {
        if (m_body.Data != null)
        {


            if (bodySprite != null && m_body != null)
            {
                bodySprite.color = m_body.Data.color;
                bodySprite.sprite = m_body.Data.sprite;
            }
        }
        else
        {
            Debug.LogError("BodyController cannot load Data in Body");
        }
    }

    void BindStats()
    {
        if (m_body.Data != null)
        {


            maxHealth = m_body.Data.life;
            health = m_body.Data.life;
            gold = m_body.Data.golds;
        }
        else
        {
            Debug.LogError("BodyController cannot load Data in Body");
        }
    }

    public void Upgrade()
    {
        if (m_body.Data != null)
        {


            SetMaxHealt(GetMaxHealt() +(m_body.Data.coefLife * m_body.Data.life));

            SetHealt(GetHealt() +(m_body.Data.coefLife * m_body.Data.life));

        }
        else
        {
            Debug.LogError("BodyController cannot load Data in Body");
        }
    }

    public void SetMaxHealt(float newValue)
    {
        maxHealth = newValue;

    }

    public float GetMaxHealt()
    {
        return maxHealth;
    }

    public void SetHealt(float newValue)
    {
        health = newValue;
        
    }

    public float GetHealt()
    {
        return health;
    }


    public void SetGold(int newValue)
    {
        gold = newValue;
    }

    public int GetGold()
    {
        return gold;
    }

    public BodyData GetBaseData()
    {
        return (BodyData)m_body.GetBaseData();
    }

}
