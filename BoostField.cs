using UnityEngine;
using System.Collections;

public class BoostField : MonoBehaviour
{
    public float boostAmount = 150;
    public GameObject boostPrefab;
    //---------------------------------------
    void OnTriggerEnter(Collider trigger)
    {
        if (trigger.tag == "Vehicle")
        { // dont actually need that.
            Vehicle VS = trigger.GetComponentInParent<Vehicle>();
            if (VS)
            {
                GameObject boost = Instantiate(boostPrefab);
                boost.transform.parent = VS.transform;
                boost.GetComponent<Boost>().boostAmount = boostAmount;
                VS.GetComponent<VehicleAudio>().BoostField();
            }
        }
    }
    //---------------------------------------
}
