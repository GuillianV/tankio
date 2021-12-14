using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class UIEnemyArrow : MonoBehaviour
{
    public GameObject arrowPrefab;
    [Range(1, 10)] public int redrawRate = 5;
    [Range(1, 10)] public int redrawSpeed = 2;
    private float m_redrawRate;

    private GameObject m_container;
    private RectTransform m_containerRectTransform;
    private Renderer m_renderer;
    private Vector2 m_containerSizeScaled;


    private GameObject m_arrowCloned;
    private Vector2 arrowPos;
    private Vector3 finalArrowPos;
    private Quaternion finalArrowRotation;


    private Vector2 m_offsetPos;
    private bool isVisible = false;

    #region UnityEvent

    
    public void Start()
    {
        m_container = GameObject.FindWithTag("UIEnemyArrowParent");
        m_redrawRate = (float) redrawRate * Time.maximumDeltaTime;
        if (m_container)
        {
            m_containerRectTransform = m_container.GetComponent<RectTransform>();
            m_renderer = GetComponent<Renderer>();
            m_containerSizeScaled =
                new Vector2((m_containerRectTransform.rect.size.x * m_containerRectTransform.lossyScale.x) / 2,
                    (m_containerRectTransform.rect.size.y * m_containerRectTransform.lossyScale.y) / 2);

            GetArrowRotation();
            GetNewArrowPos();
            GetArrowPosClamped();

            m_arrowCloned = Instantiate(arrowPrefab, finalArrowPos, finalArrowRotation, m_container.transform);
        }

        InvokeRepeating("SlowUpdate", 0.0f, m_redrawRate);
    }


    void SlowUpdate()
    {
        if (!m_renderer.isVisible && !isVisible)
        {
            GetNewArrowPos();
            GetArrowRotation();
        }

    }


    private void FixedUpdate()
    {
        if (!m_renderer.isVisible  && !isVisible)
        {
            GetArrowPosClamped();
            LerpArrowUI();
        }

        
    }

    private void OnBecameVisible()
    {
        isVisible = true;
        m_arrowCloned.SetActive(false);
        
    }

    private void OnBecameInvisible()
    {
        isVisible = false;
        m_arrowCloned.SetActive(true);
        GetArrowRotation();
        GetNewArrowPos();
        GetArrowPosClamped();
        MoveArrowUI();
        
    }

    private void OnDestroy()
    {
        if (m_arrowCloned)
        {
            Destroy(m_arrowCloned);
        }
    }
    
    #endregion

    #region CalculateArrow

    protected void GetNewArrowPos()
    {
        m_offsetPos = GetVector(m_container.transform.position);
        arrowPos = new Vector2(m_container.transform.position.x - m_offsetPos.x,
            m_container.transform.position.y - m_offsetPos.y);
    }

    protected void GetArrowPosClamped()
    {
        finalArrowPos = new Vector3(
            Mathf.Clamp(this.arrowPos.x, m_container.transform.position.x - m_containerSizeScaled.x,
                m_container.transform.position.x + m_containerSizeScaled.x),
            Mathf.Clamp(arrowPos.y, m_container.transform.position.y - m_containerSizeScaled.y,
                m_container.transform.position.y + m_containerSizeScaled.y), 0);
    }

    protected void GetArrowRotation()
    {
        Vector3 vectorToTarget = m_container.transform.position - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        finalArrowRotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }


    private Vector2 GetVector(Vector2 vecPos)
    {
        float valueX = 0;
        float valueY = 0;

        if (vecPos.x < transform.position.x)
        {
            valueX = vecPos.x - transform.position.x;
        }
        else
        {
            valueX = (transform.position.x - vecPos.x) * -1;
        }


        if (vecPos.y < transform.position.y)
        {
            valueY = vecPos.y - transform.position.y;
        }
        else
        {
            valueY = (transform.position.y - vecPos.y) * -1;
        }

        return new Vector2(valueX, valueY);
    }

    #endregion

    #region Move Arrow

    protected void LerpArrowUI()
    {
        m_arrowCloned.transform.position = Vector3.Lerp(
            new Vector3(m_arrowCloned.transform.position.x, m_arrowCloned.transform.position.y, 0), finalArrowPos,
            Time.deltaTime * redrawSpeed);
        m_arrowCloned.transform.rotation = Quaternion.Slerp(m_arrowCloned.transform.rotation, finalArrowRotation,
            Time.deltaTime * redrawSpeed);
    }

    protected void MoveArrowUI()
    {
        m_arrowCloned.transform.position = finalArrowPos;
        m_arrowCloned.transform.rotation = finalArrowRotation;
    }

    #endregion


  
}