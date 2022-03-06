using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour, IManager, IUpgradable
{
    public BaseAsset gunAsset;
    public BaseAnimator gunAnimator;
    public GunController gunController;
    private TankController tankController;
    private GunData gunData;

    public event EventHandler<ProjectileEvent> BulletDestroyed;
    public event EventHandler<ProjectileEvent> BulletCreated;

    void IManager.Bind(ScriptableObject data)
    {
        BindData(data);
        tankController = GetComponent<TankController>();
        gunController.BindController(gunData);
        gunAsset.BindAssets();
        gunAnimator.BindAnimators(gunData.animators);
    }

    void IUpgradable.Upgrade()
    {
        gunController.Upgrade();
    }


    void BindData(ScriptableObject obj)
    {

        if (obj.GetType() == typeof(GunData))
        {
            gunData = (GunData)obj;
        }
        else
        {
            Debug.LogError("gunManager cannot load gunData");
        }


    }


    public GameObject Shoot()
    {
        GameObject ammo = Instantiate(gunAsset.CallAsset("Projectile"),
                   gunController.bulletSpawn.transform.position,
                   gunController.bulletSpawn.transform.rotation) as GameObject;

        BulletDestroyed bulletDestroyed = ammo.GetComponent<BulletDestroyed>();
        BulletCreated bulletCreated = ammo.GetComponent<BulletCreated>();
        bulletDestroyed.Destroyed += OnBulletDestroyed;
        bulletCreated.Created += OnBulletCreated;

        IBulletManager iBulletManager = ammo.GetComponent<IBulletManager>();
        if (iBulletManager != null)
        {
            iBulletManager.AdditionalBulletData(gunController.GetBulletVelocity(), gameObject.tag, gunController.bulletSpawn.transform.up,3);
            tankController.tankScriptable.listProjectileScriptableObject.ForEach(projectileData =>
            {
                iBulletManager.Bind(projectileData);
            });
        }


        gunAnimator.CallAnimator("BulletSpawn").SetTrigger("Fire");
        gunAnimator.CallAnimator("Gun").SetTrigger("Fire");
        return ammo;
    }


    public void OnBulletDestroyed(object sender, EventArgs args)
    {

        BulletDestroyed bulletDestroyed = sender as BulletDestroyed;
        BulletDestroyedHandler(bulletDestroyed.gameObject, bulletDestroyed.tag);
    }

    public void OnBulletCreated(object sender, EventArgs args)
    {
        BulletCreated bulletCreated = sender as BulletCreated;
        BulletCreatedHandler(bulletCreated.gameObject, bulletCreated.tag);
    }

    public void BulletDestroyedHandler(GameObject bullet, string tag)
    {
        BulletDestroyed?.Invoke(this, new ProjectileEvent(bullet, tag));
    }

    public void BulletCreatedHandler(GameObject bullet, string tag)
    {
        BulletCreated?.Invoke(this, new ProjectileEvent(bullet, tag));
    }

}
