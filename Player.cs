using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public new string name = "Frank";
    public bool isAlive = true;
    public bool isRacing = true;
    public bool isInputEnabled = false;
    public float health = 100;
    public float maxHealth;
    public int currentPosition = 0;

    public int id = 0;



    public VehicleStats vehicle;
    private void Start()
    {
        maxHealth = vehicle.maxHealth;
        health = maxHealth;
        this.gameObject.name = name+" ("+this.vehicle.name+")";
    }
}
