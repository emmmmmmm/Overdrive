using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuInput : MonoBehaviour {

    private Vehicle VS;
    private CameraManager CS;

    void Start()
    {
        VS = GetComponent<Vehicle>();
      
    }

    void FixedUpdate()
    {
        VS.Turn(Input.GetAxis("L_XAxis_1"));
        VS.Pitch(Input.GetAxis("L_YAxis_1"));
    }
}
