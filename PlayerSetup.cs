using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(AIInput))]

public class PlayerSetup : MonoBehaviour
{





    void Start()
    {
        RegisterToRaceManager();
    }

    //---------------------------------------
    private void RegisterToRaceManager()
    {
        if (RaceManagerSingleton.Singleton)
        {
            RaceManagerSingleton.Singleton.RegisterPlayer(GetComponent<Player>());
        }
        else Debug.LogError(gameObject.name + ": racemanager not found");
    }
    //---------------------------------------
    public void LoadAutoPilot()
    {
        GetComponent<AIInput>().enabled = true;
        GetComponent<PlayerInput>().enabled = false;
    }
}
