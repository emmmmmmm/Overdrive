using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public RaceManagerSingleton RM;
    public Transform camCanvas;
    public Transform camHolder;
    public Transform UIcam;
    public Transform terrainCam;
    public Transform vehicleCam;
    public Transform camDefault;
    public Transform AudioListener;
    public Transform camRotator;
    public Transform target;
    public float turnSpeed = 5;
    //public float lookAhead = 100;
    public Vector3 moveDampening = new Vector3(0.3f, 0.3f, 0.4f);

    public float defaultFOV = 0;
    public float boostedFOV = 1;
    public float currentFOV = 0;
    public float FOVBoostTime = 1f;
    private Animator anim;
    public bool boost = false;

    private void Start()
    {
        anim = camHolder.GetComponent<Animator>();
        if (!anim) Debug.Log("no camera animation found!?");

        // QualitySettings.vSyncCount = 0;
        //  Application.targetFrameRate = 60;
    }


    //---------------------------------------
    void FixedUpdate()
    {
         CamUpdate();

        if (boost) { boost = false; Boost(); }

    }
    void CamUpdate()
    {
        // i need a local transform for this somehow!
        Vector3 toVehicle = new Vector3();
        //toVehicle.x = Mathf.Lerp(camCanvas.position.x, camDefault.position.x, moveDampening.x * Time.deltaTime);
        //toVehicle.y = Mathf.Lerp(camCanvas.position.y, camDefault.position.y, moveDampening.y * Time.deltaTime);
        //toVehicle.z = Mathf.Lerp(camCanvas.position.z, camDefault.position.z, moveDampening.z * Time.deltaTime);

        
        toVehicle = camDefault.position - camCanvas.position;
        toVehicle = target.transform.InverseTransformDirection(toVehicle);

        toVehicle.x *= moveDampening.x;
        toVehicle.y *= moveDampening.y;
        toVehicle.z *= moveDampening.z;

        toVehicle = target.transform.TransformDirection(toVehicle);
        camCanvas.position += toVehicle;
        
        //camCanvas.position = toVehicle;

        //camCanvas.eulerAngles += (camDefault.eulerAngles - camCanvas.eulerAngles)*turnSpeed;


        camCanvas.rotation = Quaternion.Lerp(camCanvas.rotation, camDefault.rotation, Time.deltaTime * turnSpeed);



        // do this only at update, or always?
        // position audio listener at the vehicle:
        UpdateAudioListenerPosition();


        if (currentFOV > defaultFOV) currentFOV -= Time.fixedDeltaTime * FOVBoostTime;
        else { currentFOV = defaultFOV; }


        // FOV
        camHolder.GetComponent<Animator>().SetFloat("Boost", Mathf.Lerp(camHolder.GetComponent<Animator>().GetFloat("Boost"), currentFOV,5f*Time.fixedDeltaTime));

     ///   terrainCam.GetComponent<Camera>().fieldOfView = Mathf.Lerp(terrainCam.GetComponent<Camera>().fieldOfView, currentFOV, Time.fixedDeltaTime * 10f);
        UIcam.GetComponent<Camera>().fieldOfView = vehicleCam.GetComponent<Camera>().fieldOfView;

    }


    private void UpdateAudioListenerPosition() {
        /*  if (RM.raceStarted)
          {
              AudioListener.position = target.transform.position;
              AudioListener.rotation = camCanvas.rotation;
          }
          else {
              AudioListener.position = cam.position;
              AudioListener.rotation = cam.rotation;
          }
          */
        AudioListener.position = target.transform.position;
        AudioListener.rotation = camCanvas.rotation;
    }
    private void Update()
    {
        //camUpdate();

        UpdateAudioListenerPosition();


      //  if (Application.targetFrameRate != 60)
      //      Application.targetFrameRate = 60;
    }
    //---------------------------------------
    public void Boost() { currentFOV = boostedFOV; Debug.Log("boosting camera"); }
    //---------------------------------------
    public void RaceFinished()
    {
        anim.SetTrigger("raceFinished");
        
    }
    public void RaceStarted()
    {
       anim.SetTrigger("raceStarted");
    }

    public void LookBack(bool lookBack)
    {
        if (lookBack)
        {
            camRotator.localEulerAngles = new Vector3(0, 180, 0);
            camRotator.localPosition = new Vector3(0, 0, 10);
        }
        else
        {
            camRotator.localEulerAngles = new Vector3();
            camRotator.localPosition = Vector3.zero;
        }
    }
public void StartRace()
    {
        camHolder.GetComponent<Animator>().SetTrigger("startRace"); // or something like this
    }
}
