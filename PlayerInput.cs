using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerInput : MonoBehaviour
{
    private Vehicle VS;
    public CameraManager CS;

    public float turn = 0;
    public float pitch = 0;
    public float strave = 0;
    public float accelerate = 0;
    public float decelerate = 0;

    void Start()
    {
        VS = GetComponent<Vehicle>();
       // CS = GetComponent<CameraScript>();

    }
    void FixedUpdate()
    {
        turn = Input.GetAxis("L_XAxis_1");
       // if (!VS.isRacing) return;
        pitch = Input.GetAxis("R_YAxis_1");
        strave = Input.GetAxis("R_XAxis_1");
       
        accelerate = Input.GetAxis("TriggersR_1");
        decelerate = Input.GetAxis("TriggersL_1");
        VS.Turn(turn);
        VS.Pitch(pitch);
        VS.Strafe(strave);
        VS.Accelerate(accelerate);
        VS.Deccelerate(decelerate);

        // buttons
        if (Input.GetButton("X_1")) VS.Boost();
        if (Input.GetButton("A_1")) VS.EnableGravity();
        if(CS)CS.LookBack(Input.GetButton("B_1")); 
    }
}
