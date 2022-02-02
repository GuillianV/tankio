using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyController : MonoBehaviour, ITankComponent, IUpgradable
{


    public SpriteRenderer bodySprite;

    private Body body;
    private float maxHealth;
    private float health;
    private int gold;



    private void Awake()
    {
        body = gameObject.AddComponent<Body>();
    }

    void ITankComponent.BindData(ScriptableObject obj)
    {

        if (obj.GetType() == typeof(BodyData))
        {
            BodyData bodyData = (BodyData)obj;
            body.LoadData(bodyData);

        }
        else
        {
            Debug.LogError("BodyController cannot load bodyData");
        }


    }


    void ITankComponent.BindComponent()
    {
        if (body.Data != null)
        {


            if (bodySprite != null && body != null)
            {
                bodySprite.color = body.Data.color;
                bodySprite.sprite = body.Data.sprite;
            }
        }
        else
        {
            Debug.LogError("BodyController cannot load Data in Body");
        }
    }

    void ITankComponent.BindStats()
    {
        if (body.Data != null)
        {


            maxHealth = body.Data.life;
            health = body.Data.life;
            gold = body.Data.golds;
        }
        else
        {
            Debug.LogError("BodyController cannot load Data in Body");
        }
    }

    void IUpgradable.Upgrade()
    {
        if (body.Data != null)
        {


            SetMaxHealt(GetMaxHealt() +(body.Data.coefLife * body.Data.life));

            SetHealt(GetHealt() +(body.Data.coefLife * body.Data.life));

        }
        else
        {
            Debug.LogError("BodyController cannot load Data in Body");
        }
    }

    public void SetMaxHealt(float newValue)
    {
        maxHealth = newValue;

    }

    public float GetMaxHealt()
    {
        return maxHealth;
    }

    public void SetHealt(float newValue)
    {
        health = newValue;
        
        if (GetHealt() <= 0)
        {
            Destroy(gameObject);
        }
        
    }

    public float GetHealt()
    {
        return health;
    }


    public void SetGold(int newValue)
    {
        gold = newValue;
    }

    public int GetGold()
    {
        return gold;
    }

    public BodyData GetBaseData()
    {
        return (BodyData)body.GetBaseData();
    }

}
