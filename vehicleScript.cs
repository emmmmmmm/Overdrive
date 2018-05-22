using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[RequireComponent(typeof(Player))]
[RequireComponent(typeof(Rigidbody))]

public class vehicleScript : MonoBehaviour
{

    /* Manages AG-Vehicle Physics*/



    // 2Do:
    // - clean up
    // - change all those "input" publics to Serialize-field-privates.
    // - use private + get{} for variables that are "read only"
    // - move all remaining audio-events to audio-script!





    [Space(10)]
    [Header("Movement Settings")] // this will become a ScriptableObject for vehicle-stats (+ physics settings)
    public float acceleration = 50f;
    public float decceleration = 0.1f;
    public float maxSpeed = 100f;
    private float currentMaxSpeed = 100f;
    public float turnSpeed = 5;
    public float straveSpeed = 40;
    public bool groundContact = false;
    private bool roadContact = false;
    public float roadSpeedIncrease = 0.06f; // should be set from GameManager
    public AnimationCurve accelerationCurve; // not yet implemented
    // add curve for drag aswell?

    [Space(10)]
    [Header("Physics Settings")] // -> to ScriptableObject 
    public float hoverHeight = 1;
    public float liftForce = 50;
    public float holdForce = 100;
    public Vector3 drag = new Vector3(0.02f, 0.24f, 0.02f);
    public float airDrag = 0.02f;
    public float angularDrag = 1f;
    public float forwardAlignmentSpeed = 0.1f; // "magnetic stabalizers"
    public float currentGravity = 5;
    public float standardGravity = 5; // <- from GM
    public Vector3 gravityDirection = Vector3.down;
    public float currentBanking = 0; // no need for this to be public!

    // private Vector3 velocity;
    [Space(10)]
    [Header("Race Logic Settings")]
    public float currentAcceleration = 0; // for engine-audio
    public float currentSpeed = 0;
    public float overDriveBoost = 0;        // to boostscript...
    public float maxOverDriveBoost = 10;

    [Space(10)]
    [Header("Transforms")]
    public GameObject vehicleModel;
    public Transform[] hoverPoints;
    public GameObject collisionParticle;
    // public ParticleSystem dustParticles; // to modify particles if inflight etc.

    [Space(10)]
    [Header("FMOD Audio Events")] // to audio script
    [FMODUnity.EventRef]
    public string boostFieldSoundEvent;
    [FMODUnity.EventRef]
    public string collisionSoundEvent;
    [FMODUnity.EventRef]
    public string turboSoundEvent;
    [FMODUnity.EventRef]
    public string respawnSoundEvent;

    [HideInInspector]
    public Player player;

    private Rigidbody rb;

    //---------------------------------------
    void Start()
    {
        rb = GetComponent<Rigidbody>(); // required
        player = GetComponent<Player>(); // required
        // set initial gravity:
        gravityDirection = Vector3.down;
        currentGravity = standardGravity;
    }
    //----------------------------------------------------------------
    // Physics 
    //----------------------------------------------------------------
    #region physics
    void FixedUpdate()
    {
        RaycastHit hit;
        Vector3 upForce;
        float strength;
        groundContact = false;
        roadContact = false;
        Vector3 normal = Vector3.zero;


        for (int i = 0; i < hoverPoints.Length; i++)
        {
            if (Physics.Raycast(hoverPoints[i].position, transform.up * -1, out hit, hoverHeight * 4f))
            {
                strength = (hoverHeight - hit.distance) / hoverHeight * 100f;

                if (strength < 0) strength *= holdForce;
                else strength *= liftForce * Mathf.Abs(strength / 2);

                upForce = transform.up * strength;

                rb.AddForceAtPosition(upForce, hoverPoints[i].position);
                normal += hit.normal;
                groundContact = true;
                if (hit.transform.tag == "raceTrack") roadContact = true;
                Debug.DrawRay(hoverPoints[i].position, upForce / 10f, Color.red);
            }
        }
        normal.Normalize(); // averaged up-vector

        ApplyDrag(groundContact);
        if (groundContact)        // align to ground!
        {
            AlignRotation(normal);
        }
        else // flying
        {
            ApplyGravity(gravityDirection);
            AlignRotation(-gravityDirection);
        }

        // increase max speed if you are on the racetrack!
        if (roadContact) currentMaxSpeed = maxSpeed + maxSpeed * roadSpeedIncrease;
        else currentMaxSpeed = maxSpeed;

        // charge booster
        UpdateBoost(); // just move this to the boostscript already...
        UpdateBanking();

        // update variables
        currentSpeed = rb.velocity.magnitude;
    }
    //---------------------------------------
    // align to current up-axis
    private void AlignRotation(Vector3 up)
    {
        Quaternion rotationCorrection = Quaternion.FromToRotation(transform.up, up) * transform.rotation;
        rb.MoveRotation(Quaternion.Lerp(rb.rotation, rotationCorrection, Time.fixedDeltaTime * 10));
    }

    //---------------------------------------
    // move to boost-script

    private void UpdateBoost()
    {
        if (currentSpeed > currentMaxSpeed && currentMaxSpeed > 0)
            GetComponent<boostScript>().chargeBoost();
        overDriveBoost = GetComponent<boostScript>().energyPool;
        maxOverDriveBoost = GetComponent<boostScript>().energyPoolMax; // no need to set every time...! -> move to start
    }

    //---------------------------------------
    private void ApplyGravity(Vector3 dir)
    {
        rb.velocity += dir * currentGravity;
        //        rb.AddForce(dir * gravity, ForceMode.Acceleration);
    }
    //---------------------------------------
    private void ApplyDrag(bool isGrounded)
    {
        Vector3 vel = transform.InverseTransformDirection(rb.velocity);
        if (isGrounded)
        {
            vel = Vector3.Scale(vel, (Vector3.one - drag));
        }
        else // flying
        {
            vel *= (1f - airDrag);
        }

        vel = transform.TransformDirection(vel);
        rb.velocity = vel;
        rb.angularVelocity *= 1f - angularDrag;

        // forward alignment!
        // rotate velocity vector towards the ships forward! 
        float side = Vector3.Dot(transform.right, rb.velocity.normalized);
        rb.velocity += transform.forward * Mathf.Abs(side) * forwardAlignmentSpeed;
        rb.velocity -= transform.right * side * forwardAlignmentSpeed;

    }
    #endregion
    //----------------------------------------------------------------
    // steering
    //----------------------------------------------------------------
    #region steering
    public void Accelerate(float amount)
    {

        if (!player.isInputEnabled) return; // shouldn't even be called from inputScript, but just to be sure
        if (rb.velocity.magnitude < currentMaxSpeed - acceleration)
            //            rb.AddForce(transform.forward * acceleration * amount);
            rb.velocity += transform.forward * acceleration * amount;

        currentAcceleration = amount;
    }
    //---------------------------------------
    public void Deccelerate(float amount)
    { //0->1
        Vector3 vel = transform.InverseTransformDirection(rb.velocity);
        if (vel.z > 0)
            // rb.AddForce(transform.forward * -decceleration * amount);
            rb.velocity -= transform.forward * decceleration * amount;
    }

    //---------------------------------------
    public void Turn(float amount)
    {
        if (!player.isInputEnabled) return;
        // some lerping might be nice?
        Quaternion rot = rb.rotation;
        rot *= Quaternion.Euler(0, turnSpeed * amount, 0);
        rb.MoveRotation(rot);
        bank(amount);
    }
    //---------------------------------------
    public void Pitch(float amount)
    {
        // if (!isInputEnabled) return; // nope, always allow this, so the player can confirm that inputs are working!
        // only pitch when flying! // if i do that, then i maybe shoudln't align to gravity direction? ... hmmmm....
        // if (groundContact) return;
        Quaternion rot = rb.rotation;
        rot *= Quaternion.Euler(-turnSpeed * amount, 0, 0);
        rb.MoveRotation(rot);
    }
    //---------------------------------------
    public void Strafe(float amount) // strave? -> check your spelling!
    {
        if (!player.isInputEnabled) return;
        // rb.AddForce(transform.right * amount * straveSpeed);
        // rb.MovePosition(transform.position + transform.right * amount * straveSpeed); // !?? warum kippt des!?
        rb.velocity += transform.right * straveSpeed * amount;
        springBank(amount);
    }
    //---------------------------------------
    public void Boost()
    {
        if (!player.isInputEnabled) return;
        if (GetComponent<boostScript>().ApplyBoost() < 0) return;

        if (GetComponent<CameraScript>())
            GetComponent<CameraScript>().Boost();
        // play boost audio
        if (turboSoundEvent != "")
            FMODUnity.RuntimeManager.PlayOneShot(turboSoundEvent, transform.position);
    }

    //---------------------------------------
    // player controlled gravity
    public void EnableGravity()
    {
        if (!player.isInputEnabled) return;
        if (!groundContact) return; // gravity already applied
        applyGravity(gravityDirection);// actually there's really no need to pass the direction...!
        //        rb.AddForce(gravityDirection * gravity * 1f, ForceMode.Acceleration);
    }
    //---------------------------------------
    // remove the spring, use animation blend tree for this...! much clean, very simple, WOW!
    Spring bankingSpring = new Spring(); // move to Top
    private void SpringBank(float amount)
    {

        // vehicleModel.transform.eulerAngles = transform.eulerAngles + Vector3.forward *
        currentBanking += bankingSpring.update(-amount);
    }
    private void Bank(float amount)
    {
        // float bankAmount = 20;
        //vehicleModel.transform.eulerAngles = transform.eulerAngles +
        currentBanking += -amount;
    }
    private void UpdateBanking()
    {
        float bankAmount = currentBanking * 20f;
        bankAmount = Mathf.Clamp(bankAmount, -20, 20);
        vehicleModel.transform.eulerAngles = transform.eulerAngles + Vector3.forward * bankAmount;
        currentBanking = 0;
    }

    #endregion

    //---------------------------------------
    public float GetSpeed() { return currentSpeed; }
    //---------------------------------------
    public void Respawn()
    {
        // todo:
        // disable input, trigger animation, on animation-end: re-enable inputs

        Transform spawnPoint = GetComponent<CurrentPositionScript>().GetRespawnPoint();
        rb.velocity *= 0.0f;
        rb.angularVelocity *= 0.0f;
        rb.rotation = spawnPoint.rotation;
        rb.position = spawnPoint.position + Vector3.up * 5; // remove z-offset! (and place waypoints properly!)

        // move to new player.reset() function?
        player.health = player.maxHealth;

        // reset gravity?

        // reset Boost
        foreach (Boost b in GetComponentsInChildren<Boost>())
        {
            b.removeBoost();
        }

        //play respawnaudio - will move to new respawn Routine / maybe even called from a respawn animation!
        FMODUnity.RuntimeManager.PlayOneShot(respawnSoundEvent, transform.position);

        /*
                FMOD.Studio.EventInstance rspwn = FMODUnity.RuntimeManager.CreateInstance(respawnSoundEvent);
                rspwn.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject, rb));
                rspwn.start();
                rspwn.release();
                */

        Debug.Log(player.name + " respawned");

    }
    //---------------------------------------

    void OnCollisionEnter(Collision col)
    {
        // audio
        if (collisionSoundEvent != "")
        {
            // FMODUnity.RuntimeManager.PlayOneShot(collisionSoundEvent, transform.position);
            FMOD.Studio.EventInstance collision = FMODUnity.RuntimeManager.CreateInstance(collisionSoundEvent);
            collision.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject, rb));
            collision.setParameterValue("strength", (float)col.relativeVelocity.magnitude / 350f); // that's a bit "hacky"
            collision.start();
            collision.release();
            // Debug.Log((float)col.relativeVelocity.magnitude / 350f);
        }

        //particles
        for (int i = 0; i < col.contacts.Length; i++)
        {
            GameObject p = Instantiate(collisionParticle, col.contacts[i].point, Quaternion.identity) as GameObject;
            p.transform.parent = this.transform;

        }

        // player health
        if (player.isRacing)
        {
            if (col.impulse.magnitude < 1000)
                player.health -= col.impulse.magnitude * 0.1f;
            else player.health -= 100; // maximum damage
        }
        if (player.health <= 0)
        {
            Respawn();
        }
        if (col.transform.tag == "deathZone") // map/sector boundaries
        {
            Respawn();
        }
    }
}
