using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Creation : MonoBehaviour
{
    public struct S_CreationData
    {
        public string roomID;
        public string socketID;
        public string type;
    }

    public class S_CreationDataEvent : EventArgs
    {
        public S_CreationData CreationData { get; private set; }


        public S_CreationDataEvent(S_CreationData _creation)
        {
            CreationData = _creation;
        }
    }



    public static string ToJson(string socketID)
    {
        var obj = new S_CreationData()
        {
            socketID = socketID,
            type = "S_CreationData",
        };
        return JsonConvert.SerializeObject(obj);
    }

    public static S_CreationData FromJson(string json)
    {

        return JsonConvert.DeserializeObject<S_CreationData>(json);

    }



    public static bool TypeMatch(string jsonString)
    {
        if (TMath.IsValidJson(jsonString))
        {
            var jo = JObject.Parse(jsonString);
            var type = jo["type"].ToString();
            if (!string.IsNullOrEmpty(type) && type == "S_CreationData")
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
