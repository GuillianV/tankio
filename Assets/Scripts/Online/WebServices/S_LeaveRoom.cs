using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_LeaveRoom : MonoBehaviour
{
    public struct S_LeaveRoomData
    {
        public string RoomID;
        public string socketID;
        public string type;
    }

    public class S_LeaveRoomDataEvent : EventArgs
    {
        public S_LeaveRoomData JoinLeaveRoomData { get; private set; }


        public S_LeaveRoomDataEvent(S_LeaveRoomData _joinLeaveRoomData)
        {
            JoinLeaveRoomData = _joinLeaveRoomData;
        }
    }



    public static string ToJson(string socketID, string RoomId)
    {
        var obj = new S_LeaveRoomData()
        {
            socketID = socketID,
            type = "S_LeaveRoomData",
            RoomID = RoomId,

        };
        return JsonConvert.SerializeObject(obj);
    }

    public static S_LeaveRoomData FromJson(string json)
    {

        return JsonConvert.DeserializeObject<S_LeaveRoomData>(json);

    }



    public static bool TypeMatch(string jsonString)
    {
        if (TMath.IsValidJson(jsonString))
        {
            var jo = JObject.Parse(jsonString);
            var type = jo["type"].ToString();
            if (!string.IsNullOrEmpty(type) && type == "S_LeaveRoomData")
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
