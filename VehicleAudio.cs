using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleAudio : MonoBehaviour
{

    Vehicle VS;
    Player player;
    [Header("FMOD Audio Events")] // to audio script
    [FMODUnity.EventRef]
    public string boostFieldSoundEvent;
    [FMODUnity.EventRef]
    public string collisionSoundEvent;
    [FMODUnity.EventRef]
    public string turboSoundEvent;
    [FMODUnity.EventRef]
    public string respawnSoundEvent;
    [FMODUnity.EventRef]
    public string engineSoundEvent;
    FMOD.Studio.EventInstance engineSound;



    // Use this for initialization
    void Start()
    {
        VS = GetComponent<Vehicle>();
        player = GetComponent<Player>();
        engineSound = FMODUnity.RuntimeManager.CreateInstance(engineSoundEvent);
        engineSound.start();
    }
    // Update is called once per frame
    void Update()
    {
        UpdateAudio();

        /*
        FMOD.Studio.PLAYBACK_STATE isPlaying;
        engineSound.getPlaybackState(out isPlaying);
        if (isPlaying == FMOD.Studio.PLAYBACK_STATE.STOPPED) engineSound.start();
        */
    }
    private void UpdateAudio()
    {
        if (engineSoundEvent == "") return;
        engineSound.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject, GetComponent<Rigidbody>()));
        engineSound.setParameterValue("speed", (float)VS.currentSpeed / (player.vehicle.maxSpeed * 1.12f));
        engineSound.setParameterValue("load", VS.currentAcceleration);
        engineSound.setParameterValue("grounded", VS.groundContact ? 1 : 0);

    }
    public void Turbo()
    {
        if (turboSoundEvent == "") return;
        FMODUnity.RuntimeManager.PlayOneShot(turboSoundEvent, transform.position);
    }
    public void BoostField()
    {
        if (boostFieldSoundEvent == "") return;
            FMODUnity.RuntimeManager.PlayOneShot(boostFieldSoundEvent, transform.position);
    }
    public void Respawn()
    {
        if (respawnSoundEvent == "") return;
        FMODUnity.RuntimeManager.PlayOneShot(respawnSoundEvent, transform.position);
    }
    public void Collision(float strength)
    {
        if (collisionSoundEvent == "") return;

        // FMODUnity.RuntimeManager.PlayOneShot(collisionSoundEvent, transform.position);
        FMOD.Studio.EventInstance collision = FMODUnity.RuntimeManager.CreateInstance(collisionSoundEvent);
        collision.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject, VS.GetComponent<Rigidbody>()));
        collision.setParameterValue("strength", strength / 350f); // that's a bit "hacky"
        collision.start();
        collision.release();
        // Debug.Log((float)col.relativeVelocity.magnitude / 350f);

    }
    //---------------------------------------
    void OnDestroy()
    {
        engineSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        engineSound.release();
    }
}
