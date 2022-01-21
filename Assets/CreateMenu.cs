using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;


#if UNITY_EDITOR
[CustomEditor(typeof(CreateMenu))]
class CreateMenuEditor : Editor
{


    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();


        CreateMenu createMenu = (CreateMenu)target;
      

        if (GUILayout.Button("Add Player"))
        {
            createMenu.AddPlayer(createMenu.namePlayerTest);
          
        }


        if (GUILayout.Button("Remove Player"))
        {
            createMenu.RemovePlayer(createMenu.namePlayerTest);
            
        }

    }
}
#endif


public class CreateMenu : MonoBehaviour
{
    public GameObject menuMultiplayer;
    public GameObject menuCreate;

    public GameObject goPlayerNamePattern;
    public GameObject goPlayerContainer;

    public TextMeshProUGUI tmpRoom;

    public string namePlayerTest;


    private SocketManager m_Socket;
    private float namePatternHeight = 0;
    private float actualPlayerContainerHeight = 100;
    private List<GameObject> playerNameList = new List<GameObject>(); 

    private void Awake()
    {
        m_Socket = SocketManager.Instance;
        m_Socket.OnCreation += RoomCreatedHandler;
        m_Socket.OnRoom += RoomJoinHandler;
        namePatternHeight = goPlayerNamePattern.GetComponent<RectTransform>().sizeDelta.y;
        goPlayerNamePattern.SetActive(false);
    }

    public void RoomCreatedHandler(object sender, S_Creation.S_CreationDataEvent e)
    {
        m_Socket.socketID = e.CreationData.socketID;
        m_Socket.roomID = e.CreationData.roomID;

        AddPlayer(e.CreationData.socketID);
        SetRoomID(e.CreationData.roomID);
    }


    public void RoomJoinHandler(object sender, S_Room.S_RoomDataEvent roomDataEvent)
    {
        if (roomDataEvent.JoinRoomData.isValid)
        {
            m_Socket.roomID = roomDataEvent.JoinRoomData.roomID;
            PrepareRoom(roomDataEvent.JoinRoomData.playersName.ToList());
        }

       
    }


    public void PrepareRoom(List<string> playerNameList)
    {
        SetRoomID(m_Socket.roomID);
        RemoveAllPlayer();
        playerNameList.ForEach(name =>
        {
           
            AddPlayer(name);
        });
    }


    public void AddPlayer(string name)
    {
        GameObject player = Instantiate(goPlayerNamePattern, goPlayerNamePattern.transform.position, goPlayerNamePattern.transform.rotation, goPlayerContainer.transform);
        playerNameList.Add(player);
        actualPlayerContainerHeight -= namePatternHeight;

        RectTransform rectContainer = goPlayerContainer.GetComponent<RectTransform>();
        if(rectContainer)
        {
            rectContainer.sizeDelta = new Vector2(0,(-1* actualPlayerContainerHeight) + namePatternHeight);
        }


        RectTransform rect = player.GetComponent<RectTransform>();
        if (rect)
        {
            rect.localPosition = new Vector3(0, actualPlayerContainerHeight, 0);
        }

        TextMeshProUGUI tmp = player.GetComponent<TextMeshProUGUI>();
        if (tmp)
        {
            tmp.text = name;
        }

        player.SetActive(true);
    }


    public void RemoveAllPlayer()
    {

    
            playerNameList.ForEach(player =>
            {
                Destroy(player);
            });

        

            actualPlayerContainerHeight = namePatternHeight;

            RectTransform rectContainer = goPlayerContainer.GetComponent<RectTransform>();
            if (rectContainer)
            {
                rectContainer.sizeDelta = new Vector2(0, (-1 * actualPlayerContainerHeight) + namePatternHeight);
            }
        
    }


    public void RemovePlayer(string name)
    {

        GameObject player = playerNameList.Find(player => player.GetComponent<TextMeshProUGUI>().text == name);


        if (player)
        {
            int index = playerNameList.FindIndex(player => player.GetComponent<TextMeshProUGUI>().text == name);
            int baseIndex = 0;
            playerNameList.ForEach(player =>
            {
                if(baseIndex >= index)
                {
                    RectTransform rect = player.GetComponent<RectTransform>();
                    if (rect)
                    {
                        float newPos = rect.localPosition.y + namePatternHeight;

                        rect.localPosition = new Vector3(0, newPos, 0);
                    }
                }

                baseIndex++;
            });

            playerNameList.Remove(player);

            Destroy(player);

            actualPlayerContainerHeight += namePatternHeight;

            RectTransform rectContainer = goPlayerContainer.GetComponent<RectTransform>();
            if (rectContainer)
            {
                rectContainer.sizeDelta = new Vector2(0, (-1 * actualPlayerContainerHeight) + namePatternHeight);
            }
        }
      


    }


    public void SetRoomID(string id)
    {
        if (!tmpRoom)
        {
            Debug.LogError("Lack TMP Component Room in CreateMenu / SetRoomID");
            return;
        }
        tmpRoom.text = "Room : " +id;
    }


    public void Back()
    {
        menuMultiplayer.SetActive(true);
        menuCreate.SetActive(false);
        RemoveAllPlayer();
        playerNameList.Clear();
        m_Socket.Send(S_LeaveRoom.ToJson(m_Socket.socketID, m_Socket.roomID));
        actualPlayerContainerHeight = 100;
        m_Socket.roomID = "";
    }




}
