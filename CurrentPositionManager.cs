using UnityEngine;
using System.Collections;




[RequireComponent(typeof(Vehicle))]
public class CurrentPositionManager : MonoBehaviour
{
    // track management
    public GameObject WPManager;
    private Vehicle VS;
    private PathManager AIPath;
    private RaceManagerSingleton RM;
    private Player player;
    public int currentWaypoint = 0;
    public int currentLap = -1;
    private int cpt_waypoint = 0;
    public float currentLapTime = 0;
    public float totalTime = 0;
    public bool raceFinished = false;
    public float fastestLapTime = 0;
    public float finishTime = 0;
    private Transform lastWaypoint;
    private Transform nextWayPoint;

    private static int WAYPOINT_VALUE = 10000;
    private static int LAP_VALUE = 1000000;
    public float distance = 0;
    public int currentPosition = 1;
    public int finishPosition = 0;
    public bool wrongDirection = false;
    private bool timerActive = false;
    public bool isPlayer = false;
    public Animator cameraAnim;
    [FMODUnity.EventRef]
    public string WPCaptureSound;

    public int id = 0;
    //---------------------------------------
    void Start()
    {
        currentWaypoint = 0;
        currentLap = -1; // make sure currentLap is initialized with -1 !
                         // trackmanager:
        RM = RaceManagerSingleton.Singleton;
        if (!RM) Debug.LogError("no RaceManager found!");
        AIPath = (PathManager)WPManager.GetComponent<PathManager>();
        if (!AIPath) Debug.LogError("no path found... :(");
        VS = (Vehicle)GetComponent<Vehicle>();
        if (!VS) Debug.LogError("no vs-script found?");

        player = GetComponent<Player>();


        if (AIPath)
        {
            lastWaypoint = AIPath.GetLastPoint();
            nextWayPoint = AIPath.GetPoint(0);
        }
    }
    //---------------------------------------	
    public void StartTimer() { timerActive = true; }
    //---------------------------------------
    void Update()
    {
        if (currentLap >= -1 && !raceFinished && timerActive)
        {
            currentLapTime += Time.deltaTime;
            totalTime += Time.deltaTime;
        }
 
        CheckDirection();
    }
    //---------------------------------------
    // move to VS
    void CheckDirection()
    {
        float directionalOffset = Vector3.Dot(transform.forward, nextWayPoint.position - transform.position);
        if (directionalOffset < 0)
            wrongDirection = true;
        else
            wrongDirection = false;
    }
    //---------------------------------------
    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "WP")
        {
            // test if "other" is the next waypoint:
            if (currentWaypoint <= AIPath.getNumberOfWPs())
            {
                if (other.GetComponent<WPScript>().getID() != currentWaypoint + 1)
                    return;
            }
            else
            {
                if (other.GetComponent<WPScript>().getID() != 1)
                    return;
            }
            // play capture audio!
            if (WPCaptureSound != "") FMODUnity.RuntimeManager.PlayOneShot(WPCaptureSound, transform.position);

            currentWaypoint = other.GetComponent<WPScript>().getID();
            // get next WP

            //nextWayPointID = currentWaypoint + 1;
            if (currentWaypoint <= AIPath.getNumberOfWPs())
                nextWayPoint = AIPath.GetPoint(currentWaypoint);
            else
                nextWayPoint = AIPath.GetPoint(0);

            //WUT? this should probably be isPlayer, no? what's this ID about?  and where is it from?^^
            if (isPlayer) // if(id==0)
            {
                AIPath.getWP(currentWaypoint - 1).Deactivate();
                AIPath.getWP(currentWaypoint).Activate();
            }

            // if completed a lap, increase currentLap, and reset Timer, and update fastest Laptime
            if (currentWaypoint == 1 && cpt_waypoint == AIPath.getNumberOfWPs() + 1 || currentLap == -1)
            {
                currentLap++;
                cpt_waypoint = 0;
                if (currentLap > 0)
                { // include time before first pass of finish line  to first round!
                  // add currentLaptime to an array of all lap times? -> send to raceManager! ?
                    if (currentLapTime < fastestLapTime || fastestLapTime == 0)
                    {
                        fastestLapTime = currentLapTime;
                    }
                    currentLapTime = 0;
                }

            }

            // make sure every wp is passed!
            if (cpt_waypoint == currentWaypoint - 1)
            {
                lastWaypoint = other.transform;
                cpt_waypoint++;
            }

            if (currentLap >= RM.laps)
            {
                if (!raceFinished)
                {
                    RaceFinished();

                }
                //... maybe invoke some function on RaceManager to shedule loading of race-finished screen?

            }
        }
    }
    //---------------------------------------
    // move this to playersetup??
    private void RaceFinished()
    {
        GetComponent<PlayerSetup>().LoadAutoPilot();
        finishPosition = player.currentPosition;
        finishTime = totalTime;
        raceFinished = true;
        VS.player.isRacing = false;

        if (isPlayer)
        {
            RM.EndRace();
        }
        // play camera animation

    }
    //---------------------------------------
    public float GetDistance()
    {
        // can i use sqrmagnitude? that'd be a lot cheaper, and this func runs quite often...
        distance = (transform.position - lastWaypoint.position).magnitude + currentWaypoint * WAYPOINT_VALUE + currentLap * LAP_VALUE;
       return distance;
    }
    //---------------------------------------
    public Vector3 GetNextWayPoint() { return nextWayPoint.position; }
    //---------------------------------------

    /*
     * public int GetCurrentPosition(CurrentPositionScript[] allCars)
    {
        if (!raceFinished)
        {
            distance = GetDistance();
            currentPosition = 1;
            foreach (CurrentPositionScript car in allCars)
            {
                if (car.GetDistance() > distance && car.id != id)
                    currentPosition++;
            }
        }
        return currentPosition;
    }
    */
    //---------------------------------------
    public Transform GetRespawnPoint() { return AIPath.GetPoint(currentWaypoint - 1); }
    //---------------------------------------
    
}
