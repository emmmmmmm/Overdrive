using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class RaceManagerSingleton : MonoBehaviour
{
    private Vehicle[] vs;
    public EventType type = EventType.Race;
    public enum EventType
    {
        Race,
        Points,
        Free
    }

    public float laps = 3;
    public float startDelay = 2;
    public bool raceFinished = false;
    public bool raceStarted = false;
    public GameObject Lamp;
    public CameraManager playerCamScript;


    [FMODUnity.EventRef]
    public string lampSetupSound;

    [FMODUnity.EventRef]
    public string lampStartSound;

    public List<Player> allPlayers;

    private static RaceManagerSingleton singleton = null;
    public static RaceManagerSingleton Singleton
    {
        get { return singleton; }
    }
    //---------------------------------------
    public void Awake()
    {
        if (singleton == null)
            singleton = this;
        
    // DontDestroyOnLoad(this.gameObject);
    // set up the car objects

    vs = GetComponentsInChildren<Vehicle>(); // use to initialize limits for the game! (maxSpeed usw...)
        int id = 0;
        foreach (Player p in allPlayers)
        {
            p.isRacing = false;
            p.isInputEnabled = false;
            p.id = id++;
        }
    }
    //---------------------------------------
    private void Update()
    {
        if (Input.anyKeyDown && !raceStarted)
        {
            if (!raceStarted)
            {
                startRace();
                raceStarted = true;
            }
            //  GetComponent<GameManager>().updatePreRaceCondition(true);
        }
    }
    //---------------------------------------
    private void startRace()
    {

        InvokeRepeating("ManualUpdate", 0.0f, 0.2f);
        StartCoroutine(StartRaceRoutine());

        // start camera-start-anim here!
        playerCamScript.StartRace();
    }
    #region Start Race Routine
    //---------------------------------------
    private IEnumerator StartRaceRoutine()
    {


        foreach (Vehicle v in vs)
        {

            if (MusicSingleton.Instance) MusicSingleton.Instance.StartRace();

        }
        yield return new WaitForSeconds(3);




        // turn on the lamps
        for (int i = 0; i < Lamp.transform.childCount; i++)
        {
            if (Lamp.transform.GetChild(i).tag != "Lamp")
                continue;
            if (i < Lamp.transform.childCount - 2)
            {
                Lamp.transform.GetChild(i).GetComponent<Renderer>().GetComponent<Renderer>().material.EnableKeyword("_EMISSION"); // material.SetColor ("_EmissionColor", Color.red);
                //engineSound.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject,null));

                FMODUnity.RuntimeManager.PlayOneShot(lampSetupSound, transform.position);
                yield return new WaitForSeconds(startDelay / (Lamp.transform.childCount - 1));

            }
            else
            {
                Lamp.transform.GetChild(i).GetComponent<Renderer>().GetComponent<Renderer>().material.EnableKeyword("_EMISSION"); //.SetColor ("_EmissionColor", Color.green);
                FMODUnity.RuntimeManager.PlayOneShot(lampStartSound, transform.position);

            }

        }
        //enable racers
        foreach (Vehicle v in vs)
        {
            v.player.isRacing = true;
            v.player.isInputEnabled = true;
        }
        // start timers
        foreach (Player p in allPlayers)
        {
            p.GetComponent<CurrentPositionManager>().StartTimer(); // -> run timers on Racemanager!?? nah, i think this makes sense...
        }

        // turn lamps off again.
        yield return new WaitForSeconds(2);
        for (int i = 0; i < Lamp.transform.childCount; i++)
        {
            if (Lamp.transform.GetChild(i).tag != "Lamp")
                continue;
            Lamp.transform.GetChild(i).GetComponent<Renderer>().GetComponent<Renderer>().material.DisableKeyword("_EMISSION");// SetColor ("_EmissionColor",  new Color(0.2051795f,0.5010926f,0.8455882f));

        }
    }
    #endregion
    //---------------------------------------
    public void ManualUpdate()
    {

        UpdateCurrentPosition();
    }
    //---------------------------------------
    public void EndRace()
    {
        GetComponent<GameManagerSingleton>().raceFinished();
        playerCamScript.RaceFinished();
        raceFinished = true;
        CancelInvoke("ManualUpdate"); // stop positions from updating - that might not be what I want!? I just want the finished ones to stop updating... -,-

        if (MusicSingleton.Instance)
            MusicSingleton.Instance.EndRace();

    }
    //---------------------------------------
    public Player[] getHighScore() { return allPlayers.ToArray(); }
    //---------------------------------------

    public void RegisterPlayer(Player player)
    {
        allPlayers.Add(player);
        // Debug.Log(player.name + " registered to the race");
    }
    //---------------------------------------
    public void DeRegisterPlayer(Player player)
    {
        allPlayers.Remove(player);
    }
    //---------------------------------------
    private void UpdateCurrentPosition()
    {
        // use lambda expression to compare distances of each player to sort the allPlayers Array by position!
        allPlayers.Sort((p2, p1) =>
            p1.GetComponent<CurrentPositionManager>().GetDistance().CompareTo(
             p2.GetComponent<CurrentPositionManager>().GetDistance()));

        for (int i = 0; i < allPlayers.Count; i++)
        {
            allPlayers[i].currentPosition = i + 1;
        }



    }
    //---------------------------------------
}

