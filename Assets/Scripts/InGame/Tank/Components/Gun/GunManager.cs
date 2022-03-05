using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour, IManager, IUpgradable
{
    public BaseAsset gunAsset ;
    public BaseAnimator gunAnimator;
    public GunController gunController;
    private TankController tankController;
    private GunData gunData;
  

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

        BulletManager ammoProjectile = ammo.GetComponent<BulletManager>();
        IManager imanager = ammoProjectile.GetComponent<IManager>();
        if (imanager != null)
        {
            tankController.tankScriptable.listProjectileScriptableObject.ForEach(projectileData =>
            {
                imanager.Bind(projectileData);
            });
        }
         



        ammoProjectile.addParentData(gunController.bulletSpawn.transform.up, gameObject.tag, gunController.GetBulletVelocity());
        gunAnimator.CallAnimator("BulletSpawn").SetTrigger("Fire");
        gunAnimator.CallAnimator("Gun").SetTrigger("Fire");
        return ammo;
    }

}
