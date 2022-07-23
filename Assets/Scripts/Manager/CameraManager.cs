using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class CameraManager : MonoBehaviour
{

    public Camera mainCamera;
    public Camera subCamera;

    [HideInInspector]
    public bool followObject;
    
    [HideInInspector]
    public GameObject objectToFollow;
    
    [HideInInspector]
    public float speed;

    private GameManager m_Game; 
    
    // Start is called before the first frame update
    private void Awake()
    {
        m_Game = GameManager.Instance;
    }

    public void SetGameObjectToFollow(GameObject _objectToFollow)
    {
        objectToFollow = _objectToFollow;
    }


    
    // Update is called once per frame
    void FixedUpdate()
    {
     
        // 220 t
        
        if (followObject && objectToFollow != null)
        {

            LerpCameraTo(objectToFollow.transform.position, speed, mainCamera);
        }      
        
        if (followObject && objectToFollow != null && subCamera != null)
        {

            LerpCameraTo(objectToFollow.transform.position, speed,subCamera);
        } 
        

    }


    public void LerpCameraTo(Vector3 _objectToFollowPos, float _speed, Camera camera)
    {
        camera.transform.position = Vector3.Lerp(camera.transform.position,new Vector3(_objectToFollowPos.x,_objectToFollowPos.y,camera.transform.position.z),_speed*Time.deltaTime * m_Game.TimeManager.timeScale);
    }
    
    
}


#if UNITY_EDITOR
[CustomEditor(typeof(CameraManager))]
public class CameraManager_Follow_GameObject : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // for other non-HideInInspector fields
 
        CameraManager cameraManager = (CameraManager)target;
 
   
        // draw checkbox for the bool
        cameraManager.followObject = EditorGUILayout.Toggle("Follow gameObject", cameraManager.followObject);
        if (cameraManager.followObject) // if bool is true, show other fields
        {
           
            
            cameraManager.objectToFollow = EditorGUILayout.ObjectField("gameObject to follow", cameraManager.objectToFollow, typeof(GameObject),true ) as GameObject;
            cameraManager.speed = EditorGUILayout.FloatField("speed", cameraManager.speed) ;
        }
    }
} 
#endif