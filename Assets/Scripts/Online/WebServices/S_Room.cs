using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Room : MonoBehaviour
{
    public struct S_RoomData
    {
        public string roomID;
        public string socketID;
        public string type;
        public bool isFull;
        public bool isValid;
        public string errorMessage;
        public string[] playersName;
    }

    public class S_RoomDataEvent : EventArgs
    {
        public S_RoomData JoinRoomData { get; private set; }


        public S_RoomDataEvent(S_RoomData _joinRoomData)
        {
            JoinRoomData = _joinRoomData;
        }
    }



    public static string ToJson(string socketID, string roomId)
    {
        var obj = new S_RoomData()
        {
            socketID = socketID,
            type = "S_RoomData",
            roomID = roomId,

        };
        return JsonConvert.SerializeObject(obj);
    }

    public static S_RoomData FromJson(string json)
    {

        return JsonConvert.DeserializeObject<S_RoomData>(json);

    }



    public static bool TypeMatch(string jsonString)
    {
        if (TMath.IsValidJson(jsonString))
        {
            var jo = JObject.Parse(jsonString);
            var type = jo["type"].ToString();
            if (!string.IsNullOrEmpty(type) && type == "S_RoomData")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }


    }
}
