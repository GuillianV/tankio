using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TowerController : MonoBehaviour, ITankComponent, IUpgradable
{
    public SpriteRenderer towerSprite;

    private Tower tower;
    private float towerRotationSpeed;


    private void Awake()
    {
        tower = gameObject.AddComponent<Tower>();
    }

    void ITankComponent.BindData(ScriptableObject obj)
    {

        if (obj.GetType() == typeof(TowerData))
        {
            TowerData towerData = (TowerData)obj;
            tower.LoadData(towerData);

        }
        else
        {
            Debug.LogError("TowerController cannot load TowerData");
        }


    }

    void ITankComponent.BindComponent()
    {
        if (tower.Data != null)
        {


            if (towerSprite != null && tower != null)
            {
                towerSprite.color = tower.Data.color;
                towerSprite.sprite = tower.Data.spriteTower;
            }

        }
        else
        {
            Debug.LogError("TowerController cannot load Data in Tower");
        }
    }

    void ITankComponent.BindStats()
    {
        if (tower.Data != null)
        {

            towerRotationSpeed = tower.Data.rotationSpeed;
        }
        else
        {
            Debug.LogError("TowerController cannot load Data in Tower");
        }
    }



    void IUpgradable.Upgrade()
    {
        if (tower.Data != null)
        {

            SetTowerRotationSpeed(GetTowerRotationSpeed() +   (tower.Data.coefRotationSpeed *  tower.Data.rotationSpeed));

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
        return (TowerData)tower.GetBaseData();
    }


}
