using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ListExtensions
{
    public static T TakeAtIndexOrLast<T>(this List<T> list, int index)
    {
        if (list != null && index < list.Count   && index >= 0)
        {
            return list[index];
        }
        else
        {
            return list[list.Count - 1];
        }
    }

    public static bool IsIndexAfter<T>(this List<T> list, int index)
    {
        if (list != null && index +1 < list.Count && index >= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}

public static class TMath 
{
    public static Quaternion GetAngleFromVector2D(Vector2 direction, int offset)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        return Quaternion.AngleAxis(angle+ offset, Vector3.forward);
    }

  

    public static bool IsValidJson(string strInput)
    {
        if (string.IsNullOrWhiteSpace(strInput)) { return false; }
        strInput = strInput.Trim();
        if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
            (strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array
        {
            try
            {
                var obj = JToken.Parse(strInput);
                return true;
            }
            catch (JsonReaderException jex)
            {
                //Exception in parsing json
                Console.WriteLine(jex.Message);
                return false;
            }
            catch (Exception ex) //some other exception
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
        else
        {
            return false;
        }
    }


}
