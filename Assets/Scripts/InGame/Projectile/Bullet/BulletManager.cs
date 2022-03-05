using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour, IUpgradable, IManager
{



    public BaseAsset bodyAsset;
    public BaseAnimator bodyAnimator;
    public BulletController bulletController;
    private BulletData bulletData;

  

    public Bullet bullet { get; private set; }



    private Rigidbody2D m_projectileRigidbody2D;
    private Animator m_projectileAnimator;
    private CircleCollider2D m_projectileCircleCollider2D;
    private SpriteRenderer m_spriteRenderer;
    private GameManager m_Game;

    public void addParentData(Vector3 direction, string senderTag, float bulletVelocity)
    {
        bulletController.SetDirection(direction);
        bulletController.SetSenderTag(senderTag);
        bulletController.SetVelocity(bulletVelocity);
    }


    void IManager.Bind(ScriptableObject data)
    {
        BindData(data);
        if (bulletData)
        {
            bulletController.BindController(bulletData);
            bodyAsset.BindAssets();
            bodyAnimator.BindAnimators(bulletData.animators);
        }
       

    }


    void BindData(ScriptableObject obj)
    {

        if (obj.GetType() == typeof(BulletData))
        {
            bulletData = (BulletData)obj;
        }


    }





    void IUpgradable.Upgrade()
    {

    }


    private void Awake()
    {
        m_Game = GameManager.Instance;
        this.m_projectileRigidbody2D = GetComponent<Rigidbody2D>();
        this.m_projectileAnimator = GetComponent<Animator>();
        this.m_projectileCircleCollider2D = GetComponent<CircleCollider2D>();
        this.m_spriteRenderer = GetComponent<SpriteRenderer>();




    }



    private void FixedUpdate()
    {
        if (this.m_projectileRigidbody2D != null)
        {
            m_projectileRigidbody2D.velocity = new Vector2(bulletController.GetDirection().x * Time.deltaTime * m_Game.TimeManager.timeScale * bulletController.GetVelocity() * 100,
                bulletController.GetDirection().y * Time.deltaTime * m_Game.TimeManager.timeScale * bulletController.GetVelocity()* 100);

        }

    }

    private void OnCollisionEnter2D(Collision2D elementCollided)
    {
        if (bulletController.GetBounce() <= 0 || elementCollided.collider.gameObject.layer == LayerMask.NameToLayer("Destructible"))
        {
            SendDamage(elementCollided);
            StartDestroyAnim();
        }
        else
        {
            Bounce(elementCollided.contacts[0].normal);
            bulletController.SetBounce(bulletController.GetBounce()-1);
        }


    }

    private void SendDamage(Collision2D elementCollided)
    {
        IDamagable damagable = elementCollided.gameObject.GetComponent<IDamagable>();
        if (damagable != null)
            damagable.TakeDamage(gameObject.tag, bulletController.GetDamage());

    }

    private void Bounce(Vector3 collisionNormal)
    {

        bulletController.SetDirection(Vector2.Reflect(bulletController.GetDirection().normalized, collisionNormal));
        gameObject.transform.rotation = TMath.GetAngleFromVector2D(collisionNormal, -90);
    }


    public void StartDestroyAnim()
    {
        m_projectileAnimator.SetTrigger("IsDead");
        Destroy(this.m_projectileRigidbody2D);
        Destroy(this.m_projectileCircleCollider2D);
    }

    public void EndDestroyAnim()
    {
        Destroy(gameObject);
    }


}
