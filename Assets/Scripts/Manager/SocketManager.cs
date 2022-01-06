using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using WebSocketSharp;

public class SocketManager : MonoBehaviour
{
    public static SocketManager Instance;
    public System.Diagnostics.Stopwatch st = new System.Diagnostics.Stopwatch();
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


    void Start()
    {
        checkConnectionAlive = CheckConnectionAlive();
        Connect();
    }


    IEnumerator CheckConnectionAlive()
    {
        while (true)
        {
          
            if (ws.IsAlive == true)
            {
               
            }
            else
            {
                Debug.Log("not connected");
                Disconnect();
                Connect();

            }

            yield return new WaitForSeconds(5);
        }
    }

    void OnApplicationQuit()
    {
        Disconnect();
        StopCoroutine(checkConnectionAlive);
    }

    void Connect()
    {

        ws = new WebSocket("ws://192.168.1.14:8888");

        ws.OnOpen += (sender, e) => {
            Debug.Log("WebSocket Open");
        };
        ws.EmitOnPing = true;
        ws.OnMessage += (sender, e) => {

            Debug.Log(e.Data);

        };

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


}
