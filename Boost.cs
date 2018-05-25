using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boost : MonoBehaviour {
    public float boostAmount = 1;
    public float boostDuration = 1;
    public float boostTimer = 0;
    private Rigidbody rb;
    //---------------------------------------
    void Start () {
    rb = GetComponentInParent<Rigidbody>();
        if (!rb) RemoveBoost();
        boostTimer = boostDuration;
	}

    //---------------------------------------
    void FixedUpdate () {
        ApplyBoost();
        if (boostTimer < 0)
            RemoveBoost();
	}
    //---------------------------------------
    public void ApplyBoost()
    {

        if (boostTimer > 0)
        {
            rb.velocity += rb.transform.forward * boostAmount * Time.fixedDeltaTime;
            boostTimer -= Time.fixedDeltaTime;
        }
    }
    //---------------------------------------
    public void RemoveBoost() {
        Destroy(gameObject);
    }
}
