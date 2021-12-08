using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubCameraMap : MonoBehaviour
{

    public Camera subCamera;
    private GameObject m_player;
    public RectTransform canvaTransform;
    public Image subCameraBackground;


    private Vector2 canvaSize;
    private Vector2 cameraSize;
    
    // Start is called before the first frame update
    void Start()
    {
        m_player = GameObject.FindGameObjectWithTag("Player");
        if (!subCamera)
        {
            subCamera = GetComponent<Camera>();
        }
        if (subCamera)
        {

            if (m_player)
            {
                     
                subCamera.transform.position = new Vector3(subCamera.transform.position.x, subCamera.transform.position.y,
                    m_player.transform.position.z - 10);
            }
            else
            {
                Debug.LogWarning("Lack player");
            }



            if (canvaTransform && subCameraBackground)
            {
                  
                subCameraBackground.rectTransform.sizeDelta =
                    new Vector2(canvaTransform.rect.width * subCamera.rect.width +5, canvaTransform.rect.height * subCamera.rect.height +5);
            }
            else
            {
                Debug.LogWarning("Lack ui components");
            }
          
        }
    }

    void Update()
    {
       
    }
}
