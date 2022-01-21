using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using WebSocketSharp;

public class SocketManager : MonoBehaviour
{
    public string socketID;
    public string token;
    public string roomID;
    public bool isConnectedToRoom;

    [Header("Join")]
    public string roomConnect;

    public static SocketManager Instance;
    public System.Diagnostics.Stopwatch st = new System.Diagnostics.Stopwatch();
    public readonly Queue<Action> ExecuteOnMainThread = new Queue<Action>();

    public event EventHandler<S_Connexion.S_ConnexionDataEvent> OnConnexion;
    public event EventHandler<S_Creation.S_CreationDataEvent> OnCreation;
    public event EventHandler<S_Room.S_RoomDataEvent> OnRoom;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

    }

    private WebSocket ws;
    private IEnumerator checkConnectionAlive;


    public void ConnectServer()
    {
        checkConnectionAlive = CheckConnectionAlive();
        Connect();
    }

    public void DisconectServer()
    {
        Disconnect();
        StopAllCoroutines();
    }

    IEnumerator CheckConnectionAlive()
    {
        while (true)
        {
            yield return new WaitForSeconds(5);

            if (ws.IsAlive == true)
            {
               
            }
            else
            {
                Debug.Log("not connected");
                Disconnect();
                Connect();

            }

           
        }
    }

    void OnApplicationQuit()
    {
        DisconectServer();
    }

    void Connect()
    {

        ws = new WebSocket("ws://localhost:8888");

        ws.OnOpen += (sender, e) => {
            Debug.Log("WebSocket Open");
        };
        ws.EmitOnPing = true;
        ws.OnMessage += MessageHandler;

        ws.OnError += (sender, e) => {
            Debug.Log("WebSocket Error Message:" + e.Message);
        };

        ws.OnClose += (sender, e) => {
            Debug.Log("WebSocket Close" + e.Reason + " --- " + e.Code);

        };

        ConnectF();


    }
    private void ConnectF()
    {
        ws.ConnectAsync();
        StartCoroutine(checkConnectionAlive);
    }

    void Disconnect()
    {
        ws.Close();
        ws = null;
    }

    public void Send(string message)
    {
        ws.Send(message);
    }


    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
           Send(S_Creation.ToJson(socketID));
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            Send(S_Room.ToJson(socketID,roomConnect));
        }

        while (ExecuteOnMainThread.Count > 0)
        {
            ExecuteOnMainThread.Dequeue().Invoke();
        }

    }

    public void MessageHandler(object sender, MessageEventArgs e)
    {


        

        if (S_Connexion.TypeMatch(e.Data))
        {
            S_Connexion.S_ConnexionData connexionData = S_Connexion.FromJson(e.Data);
            ExecuteOnMainThread.Enqueue(() =>
            {
                socketID = connexionData.socketID;
                token = connexionData.token;

                OnConnexion?.Invoke(this, new S_Connexion.S_ConnexionDataEvent(connexionData));
            });

        }

        if (S_Creation.TypeMatch(e.Data))
        {
            S_Creation.S_CreationData creationData = S_Creation.FromJson(e.Data);

            roomID = creationData.roomID;

            ExecuteOnMainThread.Enqueue(() => { OnCreation?.Invoke(this, new S_Creation.S_CreationDataEvent(creationData)); });

        }

        if (S_Room.TypeMatch(e.Data))
        {
            S_Room.S_RoomData roomData = S_Room.FromJson(e.Data);

            if (roomData.isValid)
            {
                isConnectedToRoom = true;
                roomID = roomData.roomID;
            }

            ExecuteOnMainThread.Enqueue(() => { OnRoom?.Invoke(this, new S_Room.S_RoomDataEvent(roomData)); });

        }
    }


}
