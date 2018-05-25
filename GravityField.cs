using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityField : MonoBehaviour
{
    public float strength = 500;

    // Use this for initialization
    private void OnTriggerStay(Collider other) // to also mnage overlapping fields. if that's not happening then onEnter would suffice!
    {
        // should i directly apply acceleration to the passing rigidbody? 
        // then i'll get some weird effects because of standard-gravity i guess?
        // also then I'll get alignment-problems.. better do it this way...!

        Vehicle VS = other.GetComponentInParent<Vehicle>();

        if (!VS) return;

        VS.gravityDirection = transform.up;
        VS.currentGravity = strength;

    }
    private void OnTriggerExit(Collider other)
    {

        Vehicle VS = other.GetComponentInParent<Vehicle>();
        if (!VS) return;

        VS.gravityDirection = Vector3.down;
        VS.currentGravity = VS.standardGravity;


    }
}
