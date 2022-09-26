using EZCameraShake;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GunManager : MonoBehaviour, IManager
{
    public BaseAsset gunAsset;
    public BaseAnimator gunAnimator;
    public GunController gunController;
    private GunData gunData;
    //private BulletData bulletData;

    private List<GunAmmo> gunAmmos = new List<GunAmmo>();
    private GunAmmo projectileEquiped;

    public event EventHandler<ProjectileEvent> BulletDestroyed;
    public event EventHandler<ProjectileEvent> BulletCreated;
    private TankController tankController;


    void IManager.Bind()
    {
        tankController = GetComponent<TankController>();
        BindData(tankController.GetData<GunData>());
        gunController.BindController(gunData);
        gunAsset.BindAssets();
        gunAnimator.BindAnimators(gunData.animators);

        AddProjectileData("basicBullet", tankController.GetData<BulletData>(), "Projectile", true);


    }


    void BindData(ScriptableObject obj)
    {

        if (obj.GetType() == typeof(GunData))
        {
            gunData = (GunData)obj;
        }


    }
    public void AddProjectileData(string name, ProjectileScriptableObject obj,GameObject gameObjectInstancied, bool isEquiped = false)
    {

        if (gameObjectInstancied.GetComponent(typeof(IBulletManager)) as IBulletManager != null)
        {
            GunAmmo gm = new GunAmmo(name, obj,gameObjectInstancied,isEquiped);
            if(!gunAmmos.Any(gun => gm.name == gun.name))
            {
                gunAmmos.Add(gm);
            }else
            {
                GunAmmo? oldGm = gunAmmos.FirstOrDefault(gm => gm.name == name);
                if (oldGm != null)
                {
                    gunAmmos.Remove(oldGm ?? new GunAmmo());
                }
                gunAmmos.Add(gm);
            }
        }
        else
        {
            Debug.LogError("Projectile add in GunManager does not contain a component IBulletManager");
        }
        BindProjectile();
    }

    public void AddProjectileData(string name,  ProjectileScriptableObject obj, string assetName, bool isEquiped = false)
    {

        GameObject goInst = gunAsset.CallAsset(assetName);
        if(goInst.GetComponent(typeof(IBulletManager)) as IBulletManager != null)
        {
            GunAmmo gm = new GunAmmo(name, obj, goInst, isEquiped);
            if (  !gunAmmos.Any(gun => gm.name == gun.name))
            {
                gunAmmos.Add(gm);
            }else
            {
                GunAmmo? oldGm = gunAmmos.FirstOrDefault(gm => gm.name == name);
                if (oldGm != null)
                {
                    gunAmmos.Remove(oldGm ?? new GunAmmo());
                }
                gunAmmos.Add(gm);
            }
        }
        else
        {
            Debug.LogError("Projectile add in GunManager does not contain a component IBulletManager");
        }
        BindProjectile();
    }

    public void BindProjectile()
    {
        if(gunAmmos.Count > 0)
        {
            gunAmmos?.ForEach(ga =>
            {
                if (ga.isEquiped)
                    projectileEquiped = ga;
            });
        }
        else
        {
            Debug.LogWarning("Missing gunAmmos in GunManager");
        }
        
    }



    public GameObject Shoot()
    {
      
        GameObject ammo = Instantiate(projectileEquiped.objectInstancied,
                   gunController.bulletSpawn.transform.position,
                   gunController.bulletSpawn.transform.rotation) as GameObject;

        BulletDestroyed bulletDestroyed = ammo.GetComponent<BulletDestroyed>();
        BulletCreated bulletCreated = ammo.GetComponent<BulletCreated>();
        bulletDestroyed.Destroyed += OnBulletDestroyed;
        bulletCreated.Created += OnBulletCreated;

        
        IBulletManager iBulletManager = ammo.GetComponent<IBulletManager>();
        if (iBulletManager != null)
        {


            if (gameObject.tag == "Player")
            {
                CameraShaker.Instance.ShakeOnce(3, 6, .1f, .1f);
            }

            iBulletManager.AdditionalBulletData(projectileEquiped.projectileData, gunController.GetBulletVelocity(), gameObject.tag, gunController.bulletSpawn.transform.up);
       
            iBulletManager.Bind();
         
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
