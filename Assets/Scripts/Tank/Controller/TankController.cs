using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;









public class TankController : MonoBehaviour
{
    [Header("Parameters")]
    public TracksController TracksController;
    public BodyController BodyController;
    public TowerController TowerController;
    public GunController GunController;

    private List<ITankComponent> iTankComponentList = new List<ITankComponent>();
    [HideInInspector] public TankAnimationController TankAnimationController;

    private GameManager m_Game;

    private void Awake()
    {
        m_Game = GameManager.Instance;

        TracksController = GetComponent<TracksController>();

        BodyController = GetComponent<BodyController>();

        GunController = GetComponent<GunController>();

        TowerController = GetComponent<TowerController>();

        foreach(ITankComponent component in GetComponents<ITankComponent>())
        {
            iTankComponentList.Add(component);
        }

        TankAnimationController = GetComponent<TankAnimationController>();
    }


    public void BindSprite()
    {

        iTankComponentList.ForEach(component => component.BindComponent());

    }

    public void BindStats()
    {
        iTankComponentList.ForEach(component => component.BindStats());

    }


    private void FixedUpdate()
    {
        if (BodyController.GetHealt() <= 0)
        {
            Destroy(gameObject);
        }
    }


    public void OnDestroy()
    {
        Debug.Log("Tank " + gameObject.tag + " Destroyed");
    }
}