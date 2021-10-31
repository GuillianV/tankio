using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TracksController
{
    [HideInInspector]
    public Tracks tracks;
    public SpriteRenderer tracksSpriteLeft;
    public SpriteRenderer tracksSpriteRight;
}
[System.Serializable]
public class BodyController
{
    [HideInInspector]
    public Body body;
    public SpriteRenderer bodySprite;
}

[System.Serializable]
public class TowerController
{
    [HideInInspector]
    public Tower tower;
    public SpriteRenderer towerSprite;
}

[System.Serializable]
public class GunController
{
    [HideInInspector]
    public Gun gun;
    public SpriteRenderer gunSprite;
    public GameObject gunObject;
    public GameObject bulletSpawn;

}

[System.Serializable]
public class StatsController 
{
    public float tracksSpeed;
    public float tracksRotationSpeed;
    public float health;
    public float towerRotationSpeed;
    public float bulletVelocity;
    public float reloadTimeSpeed;
    public int gold;


}




public class TankController : MonoBehaviour
{
    [Header("Parameters")]
    public TracksController TracksController;
    public BodyController BodyController;
    public TowerController TowerController;
    public GunController GunController;

    [HideInInspector]
    public StatsController StatsController;

    [HideInInspector]
    public TankAnimationController TankAnimationController;
    
    private void Awake()
    {
        TracksController.tracks = GetComponent<Tracks>();
        BodyController.body = GetComponent<Body>();
        GunController.gun = GetComponent<Gun>();
        TowerController.tower = GetComponent<Tower>();
        TankAnimationController = GetComponent<TankAnimationController>();

    }


    public void BindSprite()
    {
        
        if (TracksController.tracksSpriteLeft != null && TracksController.tracksSpriteRight != null && TracksController.tracks != null)
        {
            TracksController.tracksSpriteLeft.color = TracksController.tracks.Data.color;
            TracksController.tracksSpriteRight.color = TracksController.tracks.Data.color;
            TracksController.tracksSpriteLeft.sprite = TracksController.tracks.Data.spriteTrack;
            TracksController.tracksSpriteRight.sprite = TracksController.tracks.Data.spriteTrack;
        }

        if (BodyController.bodySprite != null && BodyController.body != null)
        {
            BodyController.bodySprite.color = BodyController.body.Data.color;
            BodyController.bodySprite.sprite = BodyController.body.Data.sprite;
        }
        
        if (TowerController.towerSprite != null && TowerController.tower != null)
        {
            TowerController.towerSprite.color = TowerController.tower.Data.color;
            TowerController.towerSprite.sprite = TowerController.tower.Data.spriteTower;
        }
        
        if (GunController.gunSprite != null && GunController.gun != null)
        {
            
            GunController.gunSprite.sprite = GunController.gun.Data.spriteGun;


            GunController.gunObject.transform.localPosition = new Vector3(0, GunController.gun.Data.TowerGunOffset, 0);
            GunController.bulletSpawn.transform.localPosition =
                new Vector3(0, GunController.gun.Data.GunSpawnOffset, 0);
            
        }
    }

    public void BindStats()
    {
       StatsController.tracksSpeed =  TracksController.tracks.Data.speed;
       StatsController.tracksRotationSpeed = TracksController.tracks.Data.rotationSpeed;
       StatsController.health = BodyController.body.Data.life;
       StatsController.gold = BodyController.body.Data.golds;
       StatsController.towerRotationSpeed = TowerController.tower.Data.rotationSpeed;
       StatsController.reloadTimeSpeed = GunController.gun.Data.reloadTimeSecond;
       StatsController.bulletVelocity = GunController.gun.Data.bulletVelocity;
       
    }

    private void FixedUpdate()
    {
        if (StatsController.health <= 0)
        {
            Destroy(gameObject);
        }
    }

    
    
    public void OnDestroy()
    {
        Debug.Log("Tank " + gameObject.tag +" Destroyed");
    }
}
