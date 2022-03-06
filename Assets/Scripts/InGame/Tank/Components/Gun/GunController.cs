using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GunController
{

    private Gun m_gun = new Gun();
    
    public SpriteRenderer gunSprite;
    public GameObject gunObject;
    public GameObject bulletSpawn;

    
    private float bulletVelocity;
    private float reloadTimeSpeed;


    public void BindController(GunData gunData)
    {
        m_gun.LoadData(gunData);
        BindComponent();
        BindStats();
    }
    


   public void BindComponent()
    {

        if (m_gun.Data != null)
        {

            if (gunSprite != null && m_gun != null)
            {
             
                gunSprite.color = m_gun.Data.color;
                gunSprite.size = new Vector2(m_gun.Data.GunScaleX, m_gun.Data.GunScaleY);

                gunObject.transform.localPosition = new Vector3(0, m_gun.Data.TowerGunOffset, 0);
                bulletSpawn.transform.localPosition =
                    new Vector3(0, m_gun.Data.GunSpawnOffset, 0);

                if (m_gun.Data.GunScaleX != 0 && m_gun.Data.GunScaleY != 0)
                {
                    gunSprite.size = new Vector2(m_gun.Data.GunScaleX, m_gun.Data.GunScaleY);

                }
                gunSprite.sprite = m_gun.Data.sprite;



            }

        }
        else
        {
            Debug.LogError("GunController cannot load Data in Gun");
        }

    }

   public void BindStats()
    {


        if (m_gun.Data != null)
        {
            reloadTimeSpeed = m_gun.Data.reloadTimeSecond;
            bulletVelocity = m_gun.Data.bulletVelocity;

        }
        else
        {
            Debug.LogError("GunController cannot load Data in Gun");
        }
    }


    public void SetBulletVelocity(float newValue)
    {
        bulletVelocity = newValue;
    }

    public float GetBulletVelocity()
    {
        return bulletVelocity;
    }

    public void SetReloadTimeSpeed(float newValue)
    {
        reloadTimeSpeed = newValue;
    }

    public float GetReloadTimeSpeed()
    {
        return reloadTimeSpeed;
    }

    public GunData GetBaseData()
    {
        return (GunData) m_gun.GetBaseData();
    }




}
