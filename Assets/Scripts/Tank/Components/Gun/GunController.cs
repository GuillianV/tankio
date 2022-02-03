using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GunController :  IUpgradable
{

    private Gun m_gun = new Gun();
    
    public SpriteRenderer gunSprite;
    public GameObject gunObject;
    public GameObject bulletSpawn;

    
    private float bulletVelocity;
    private float reloadTimeSpeed;


    public void BindController(ScriptableObject data)
    {
        BindData(data);
        BindComponent();
        BindStats();
    }
    

   public void BindData(ScriptableObject obj)
    {

        if (obj.GetType() == typeof(GunData))
        {
            GunData gunData = (GunData)obj;
            m_gun.LoadData(gunData);

        }
        else
        {
            Debug.LogError("GunController cannot load GunData");
        }

    }

   public void BindComponent()
    {

        if (m_gun.Data != null)
        {

            if (gunSprite != null && m_gun != null)
            {
                gunSprite.sprite = m_gun.Data.spriteGun;


                gunObject.transform.localPosition = new Vector3(0, m_gun.Data.TowerGunOffset, 0);
                bulletSpawn.transform.localPosition =
                    new Vector3(0, m_gun.Data.GunSpawnOffset, 0);
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

    void IUpgradable.Upgrade()
    {

        if (m_gun.Data != null)
        {

            SetBulletVelocity(GetBulletVelocity() +   (m_gun.Data.coefBulletVelocity * m_gun.Data.bulletVelocity));

            SetReloadTimeSpeed(GetReloadTimeSpeed() - (m_gun.Data.coefReloadTimeSecond * m_gun.Data.reloadTimeSecond));


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
