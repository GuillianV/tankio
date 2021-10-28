using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateZero : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.transform.rotation =  Quaternion.Euler(0f, 0f, 0f);
    }

}
