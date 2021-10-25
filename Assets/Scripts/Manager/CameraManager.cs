using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class CameraManager : MonoBehaviour
{

    public Camera mainCamera;
    
    [HideInInspector]
    public bool followObject;
    
    [HideInInspector]
    public GameObject objectToFollow;
    
    [HideInInspector]
    public float speed;
    
    // Start is called before the first frame update
    void Start()
    {
       
    }

    public void SetGameObjectToFollow(GameObject _objectToFollow)
    {
        objectToFollow = _objectToFollow;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (followObject && objectToFollow != null)
        {

            LerpCameraTo(objectToFollow.transform.position, speed);
        }            

    }


    public void LerpCameraTo(Vector3 _objectToFollowPos, float _speed)
    {
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position,new Vector3(_objectToFollowPos.x,_objectToFollowPos.y,mainCamera.transform.position.z),_speed*Time.deltaTime);
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