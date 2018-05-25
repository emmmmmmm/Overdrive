using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManagerSingleton : MonoBehaviour
{
    public bool isPaused = false;
    private UIManager UI;
    [FMODUnity.EventRef]
    public string audioMixerEvent;
    FMOD.Studio.EventInstance audioMixer;

    private static GameManagerSingleton singleton = null;
    public static GameManagerSingleton Singleton
    {
        get { return singleton; }
    }

    //---------------------------------------
    // Use this for initialization
    void Awake()
    {
        if (singleton == null)            singleton = this;
        // DontDestroyOnLoad(this.gameObject);
        UI = GetComponent<UIManager>();
        Cursor.visible = false;
        audioMixer = FMODUnity.RuntimeManager.CreateInstance(audioMixerEvent);
        audioMixer.start();

        audioMixer.setParameterValue("paused", 0);
        audioMixer.setParameterValue("preRace", 1);
        audioMixer.setParameterValue("raceFinished", 0);
    }
    //---------------------------------------
    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Start_1"))
        {
            if (!isPaused) pauseGame();
            else resumeGame();
        }

        if (GetComponent<RaceManagerSingleton>().raceStarted)
        {
            updatePreRaceCondition();
        }

        if (GetComponent<RaceManagerSingleton>().raceFinished)
        {
            updateRaceFinishedCondition();
        }
          }
    //---------------------------------------
    public void updatePreRaceCondition()
    {
        audioMixer.setParameterValue("preRace", 0);
    }
    //---------------------------------------
    private void updateRaceFinishedCondition()
    {
        audioMixer.setParameterValue("raceFinished", 1);
    }
    //---------------------------------------
    public void pauseGame()
    {
        if (isPaused) return;
        isPaused = true;

        UI.pauseGame(true);
        audioMixer.setParameterValue("paused", 1);
        Time.timeScale = 0;
        Debug.Log("paused");
    }
    //---------------------------------------
    public void resumeGame()
    {
        if (!isPaused) return;
        isPaused = false;
        UI.pauseGame(false);
        audioMixer.setParameterValue("paused", 0);
        Time.timeScale = 1;

    }
    //---------------------------------------

    public void quitGame()
    {
        returnToMenu();
    }
    //---------------------------------------
    public void returnToMenu()
    {
        print("returning to menu!");
        if (MusicSingleton.Instance) MusicSingleton.Instance.EndRace();
        audioMixer.setParameterValue("paused", 0);
          SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);//SceneManager.GetActiveScene().buildIndex-1);
    }
    //---------------------------------------

    public void raceFinished()
    {
        UI.raceFinished();
    }
    void OnDestroy()
    {
        audioMixer.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        audioMixer.release();
        Debug.Log("mixer released");
    }
}

