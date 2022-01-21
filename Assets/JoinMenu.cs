using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class JoinMenu : MonoBehaviour
{
    public GameObject menuMultiplayer;
    public GameObject menuJoin;
    public GameObject menuCreate;

    public TMP_InputField inpRoom;
    public TextMeshProUGUI tmpError;

    private SocketManager m_Socket;
    private CreateMenu createMenu;

    private void Awake()
    {
        m_Socket = SocketManager.Instance;
        m_Socket.OnRoom += JoinedRoomHandler;
        createMenu = GetComponent<CreateMenu>();
    }

    public void JoinedRoomHandler(object sender, S_Room.S_RoomDataEvent roomDataEvent)
    {

        if (!roomDataEvent.JoinRoomData.isValid)
        {
            SetError(roomDataEvent.JoinRoomData.errorMessage);
            return;
        }
        else
        {
            menuCreate.SetActive(true);
            menuJoin.SetActive(false);
            m_Socket.roomID = roomDataEvent.JoinRoomData.roomID;
            createMenu.PrepareRoom(roomDataEvent.JoinRoomData.playersName.ToList());
        }

      
        
    }


    public void Back()
    {
        menuMultiplayer.SetActive(true);
        menuJoin.SetActive(false);
    }

    private void SetError(string message)
    {
        tmpError.gameObject.SetActive(true);
        tmpError.text = message;
    }

    public void Join()
    {
        m_Socket.Send(S_Room.ToJson(m_Socket.socketID, inpRoom.text));
    }

}
