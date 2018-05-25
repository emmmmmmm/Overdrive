using UnityEngine;
using System.Collections;
using System;
public class AIInput : MonoBehaviour
{
    // steering values
    private float shipSteer = 0;
    private float shipPitch = 0;
    private float shipAirBrakeLeft = 0;
    private float shipAirBrakeRight = 0;
    private bool isAccelerating = false;
    private bool isBoosting = false;
    private Vehicle VS;
    //public GameObject WPManager;
    public float wpSteerForce = 3;
    public float vehicleAvoidanceForce = 0.5f;
    public float sideAvoidanceForce = 0.5f;
    
    public float maxViewDistance = 50;
    public float viewAngle = 20;
    public float viewSideOffset = 1.1f;
    public float scanHeight = 0.3f;
    public float sphereCastSize = 0.5f;
    public float forceLeft = 0;
    public float forceRight = 0;
    public float steerForce = 0;
    public float currentSteer = 0;
    public float steerSmoothTime = 5;


    public float l;
    public float r;
    public float wpForce;
    private CurrentPositionManager pointManager;
    //---------------------------------------
    void Start()
    {
        // connect to the vehicle.
        VS = (Vehicle)GetComponent(typeof(Vehicle));
        if (!VS) print("oh no!?");
        // manages current position on track:
        pointManager = (CurrentPositionManager)GetComponent(typeof(CurrentPositionManager));
        if (!pointManager) print("no positioningScript found!");
    }
    //---------------------------------------
    void FixedUpdate()
    {
        
        //RestSteeringValues();

        Vector3 target = pointManager.GetNextWayPoint();
        Vector3 steerVector = transform.InverseTransformPoint(target);

        float directionalOffset = Vector3.Dot(transform.forward, target - transform.position);
         wpForce =  steerVector.x / steerVector.magnitude;  // steerVector.normalized.x ?? should work aswell
        float viewDistance = Mathf.Clamp(2*VS.GetSpeed(), 10, maxViewDistance);
       // viewDistance = maxViewDistance;

        if (directionalOffset < -0.5f)
        {// if driving in wrong direction!
         
                wpForce /= Mathf.Abs(wpForce); // steer as hard as possible
          
        }
        wpForce *= wpSteerForce;

        forceLeft = 0;
        forceRight = 0;
        float[] left = new[] { 0f, 0f, 0f };
        float[] right = new[] { 0f, 0f, 0f };

        for (int i = 0; i < 3; i++)
        {
            left[i] = CastRay(-viewSideOffset, -viewAngle, viewDistance, sideAvoidanceForce, i - 1);
            right[i] = CastRay(viewSideOffset, viewAngle, viewDistance, sideAvoidanceForce, i - 1);
        }

        // ???
        Array.Sort(left);
        Array.Sort(right);

        float sumLeft = sum(left);
        float sumRight = sum(right);
                // normalize
        sumLeft /= left.Length;
        sumRight /= right.Length;


        steerForce = left[0] < right[0] ? 1-left[0] : -1+right[0];

  
        steerForce *= sideAvoidanceForce;
        steerForce += wpForce;

       
      //  steerForce = Mathf.Clamp(steerForce, -1, 1);
        shipSteer = Mathf.Lerp(shipSteer, steerForce, Time.deltaTime * steerSmoothTime); // Mathf.Lerp(currentSteer,steerForce,Time.deltaTime*steerSmoothTime);
      
        shipSteer = Mathf.Clamp(shipSteer, -1, 1);
        currentSteer = shipSteer;

        Debug.DrawLine(transform.position, target, Color.green);
        Debug.DrawRay(transform.position, transform.right * steerForce*10);

        // needs adjustment
        if (shipSteer <= -0.7f)
        {
            shipAirBrakeLeft = 1;
        }
        // needs adjustment
        if (steerForce >= 0.7f)
        {
            shipAirBrakeRight = 1;
        }
        //pitch

        if (Mathf.Abs(shipSteer) < 0.3f) isBoosting = true;
        else isBoosting = false;
        // needs adjustment

        if (Mathf.Abs(shipSteer) > 0.5f)
            shipPitch = 0;
        
        isAccelerating = true;
        if (Mathf.Abs(shipSteer) > 0.97)
            isAccelerating = false;

        SetVS();

    }

    //---------------------------------------
    float CastRay(float sideOffset, float sideDirection, float distance, float avoidanceForce, int scanLevel)
    {
        Vector3 RayCastPoint = VS.transform.position + transform.forward * 1;
        RaycastHit hit;
        float dist = distance;
        Vector3 RayCastDirection = transform.forward + transform.right * sideDirection + transform.up * scanLevel * scanHeight;
        Debug.DrawRay(RayCastPoint + transform.right * sideOffset, RayCastDirection * distance, Color.red);

        if (Physics.Raycast(RayCastPoint + transform.right * sideOffset, RayCastDirection, out hit, distance))
        {
            if (hit.collider.tag != "Groundd" )
            {
                //force = (distance - hit.distance) / (distance) * avoidanceForce;
                Debug.DrawLine(RayCastPoint + transform.right * sideOffset, hit.point, Color.cyan);
                if (hit.collider.tag == "Vehicle")
                   dist = hit.distance * vehicleAvoidanceForce;
                else
                    dist = hit.distance ;
            }
        }
        return dist / distance;
    }
    //---------------------------------------
    private void SetVS()
    {
        VS.Turn(shipSteer);
        VS.Pitch(shipPitch);
        VS.Strafe((shipAirBrakeLeft - shipAirBrakeRight)*-1f);
        if (isAccelerating)
        {
            VS.Accelerate(1);
            VS.Deccelerate(0);
        }
        else
        {
            VS.Accelerate(0);
            VS.Deccelerate(1);
        }
        if (isBoosting)
            VS.Boost();
    }
    //---------------------------------------
    private float sum(float[] ar) {
        float ret = 0;
        for (int i = 0; i < ar.Length; i++)
            ret += ar[i];
        return ret;
    }
}
