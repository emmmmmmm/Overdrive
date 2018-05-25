using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleBoost : MonoBehaviour
{

    public GameObject boostPrefab;
    public float energyPool = 0;
    public float energyPoolMax = 100;
    public float energyPoolChargeRate = 1;
    public bool isCharged = false;
    public int energyPoolTimeOut = 1;
    public float energyPoolTimer = 0;
    public bool isCharging = true;

    /*
     * maybe add a time-out before charging again!
     */

    private void FixedUpdate()
    {
        if (!isCharging)
        {
            energyPoolTimer += Time.fixedDeltaTime;
            if (energyPoolTimer >= energyPoolTimeOut) {
                isCharging = true;
                energyPoolTimer = 0;
            }
        }
    }


    //---------------------------------------
    private void ResetEnergyPool()
    {
        energyPool = 0;
        isCharged = false;
    }
    //---------------------------------------
    public void UpdateBoost()
    {
        ChargeBoost();

    }
    //---------------------------------------
    public void ChargeBoost()
    {
        if (!isCharging) return;
        energyPool += energyPoolChargeRate * Time.fixedDeltaTime;
        energyPool = Mathf.Clamp(energyPool, 0, energyPoolMax);
        if (energyPool >= energyPoolMax)
            isCharged = true;
        else
            isCharged = false;
    }
    //---------------------------------------
    public int ApplyBoost()
    {
        if (!isCharged) return -1;
        isCharging = false;
        ResetEnergyPool();
        GameObject booster = Instantiate(boostPrefab);
        booster.transform.parent = this.transform;
        return 1;
    }
}



