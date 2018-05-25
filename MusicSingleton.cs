using UnityEngine;
using System.Collections;


public class MusicSingleton : MonoBehaviour {

	private static MusicSingleton instance = null;

    [FMODUnity.EventRef]
    public string musicSoundEvent;
    FMOD.Studio.EventInstance musicSound;


    public static MusicSingleton Instance {
		get { return instance; }
	}
	void Awake() {
		if (instance != null&& instance != this) {
			Destroy(this.gameObject);
			return;
		} else {
			instance = this;
		}
		DontDestroyOnLoad(this.gameObject);
	}

    private void Start()
    {
        musicSound = FMODUnity.RuntimeManager.CreateInstance(musicSoundEvent);
        musicSound.start();
        musicSound.setParameterValue("Intensity",0f);
    }
    public void StartRace()
    {
        musicSound.setParameterValue("Intensity", 1f);
    }
    public void EndRace()
    {
        musicSound.setParameterValue("Intensity", 0f);
    }

}
