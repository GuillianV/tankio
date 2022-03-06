using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TowerController 
{
    public SpriteRenderer towerSprite;

    private Tower m_tower = new Tower();
    private float towerRotationSpeed;


    public void BindController(TowerData towerData)
    {
        m_tower.LoadData(towerData);
        BindComponent();
        BindStats();
    }



    void BindComponent()
    {
        if (m_tower.Data != null)
        {


            if (towerSprite != null && m_tower != null)
            {
                towerSprite.color = m_tower.Data.color;
                towerSprite.sprite = m_tower.Data.sprite;
            }

        }
        else
        {
            Debug.LogError("TowerController cannot load Data in Tower");
        }
    }

    void BindStats()
    {
        if (m_tower.Data != null)
        {

            towerRotationSpeed = m_tower.Data.rotationSpeed;
        }
        else
        {
            Debug.LogError("TowerController cannot load Data in Tower");
        }
    }







    public void SetTowerRotationSpeed(float newValue)
    {
        towerRotationSpeed = newValue;
    }

    public float GetTowerRotationSpeed()
    {
        return towerRotationSpeed;
    }

    public TowerData GetBaseData()
    {
        return (TowerData)m_tower.GetBaseData();
    }


}
