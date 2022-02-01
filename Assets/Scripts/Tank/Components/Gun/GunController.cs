using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour, ITankComponent, IUpgradable
{


    public SpriteRenderer gunSprite;
    public GameObject gunObject;
    public GameObject bulletSpawn;

    private Gun gun;
    private float bulletVelocity;
    private float reloadTimeSpeed;


    private void Awake()
    {
        gun = gameObject.AddComponent<Gun>();
    }

    void ITankComponent.BindData(ScriptableObject obj)
    {

        if (obj.GetType() == typeof(GunData))
        {
            GunData gunData = (GunData)obj;
            gun.LoadData(gunData);

        }
        else
        {
            Debug.LogError("GunController cannot load GunData");
        }

    }

    void ITankComponent.BindComponent()
    {

        if (gun.Data != null)
        {

            if (gunSprite != null && gun != null)
            {
                gunSprite.sprite = gun.Data.spriteGun;


                gunObject.transform.localPosition = new Vector3(0, gun.Data.TowerGunOffset, 0);
                bulletSpawn.transform.localPosition =
                    new Vector3(0, gun.Data.GunSpawnOffset, 0);
            }

        }
        else
        {
            Debug.LogError("GunController cannot load Data in Gun");
        }

    }

    void ITankComponent.BindStats()
    {


        if (gun.Data != null)
        {
            reloadTimeSpeed = gun.Data.reloadTimeSecond;
            bulletVelocity = gun.Data.bulletVelocity;

        }
        else
        {
            Debug.LogError("GunController cannot load Data in Gun");
        }
    }

    void IUpgradable.Upgrade()
    {

        if (gun.Data != null)
        {

            SetBulletVelocity(GetBulletVelocity() +   (gun.Data.coefBulletVelocity * gun.Data.bulletVelocity));

            SetReloadTimeSpeed(GetReloadTimeSpeed() - (gun.Data.coefReloadTimeSecond * gun.Data.reloadTimeSecond));


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
        return (GunData) gun.GetBaseData();
    }




}
