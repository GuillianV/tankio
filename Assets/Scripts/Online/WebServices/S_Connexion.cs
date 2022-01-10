using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Connexion : MonoBehaviour
{
    public struct S_ConnexionData
    {
        public string socketID;
        public string token;
        public string type;
    }

    public class S_ConnexionDataEvent : EventArgs
    {
        public S_ConnexionData connexionData { get; private set; }


        public S_ConnexionDataEvent(S_ConnexionData _connexionData)
        {
            connexionData = _connexionData;
        }
    }



    public static string ToJson(string socketID)
    {
        var obj = new S_ConnexionData()
        {
            type = "S_ConnexionData"
        };
        return JsonConvert.SerializeObject(obj);
    }

    public static S_ConnexionData FromJson(string json)
    {

        return JsonConvert.DeserializeObject<S_ConnexionData>(json);

    }



    public static bool TypeMatch(string jsonString)
    {
        if (TMath.IsValidJson(jsonString))
        {
            var jo = JObject.Parse(jsonString);
            var type = jo["type"].ToString();
            if (!string.IsNullOrEmpty(type) && type == "S_ConnexionData")
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
