using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    public GameObject PauseMenu;

    public GameObject GameUI;
    public GameObject RaceFinishedUI;
    public GameObject SettingsUI;
    public GameObject PreStartUI;

    public GameObject Compass;
    private GameManagerSingleton GM;
    private RaceManagerSingleton RM;
    public Vehicle Player;
    public Button pauseButton;
    public Button returnButtenMenu;
    public Button returnButton;

    Resolution[] resolutions;
    public Dropdown resolutionDropdown;
    //---------------------------------------
    // Use this for initialization
    void Start()
    {
        GM = GameManagerSingleton.Singleton;
        RM = RaceManagerSingleton.Singleton;


        PauseMenu.SetActive(false);
        RaceFinishedUI.SetActive(false);
        GameUI.SetActive(false);
        SettingsUI.SetActive(false);
        Compass.SetActive(true);
        PreStartUI.SetActive(true);

        // create list of available resolutions:
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;


        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                currentResolutionIndex = i;

        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();


    }
    //---------------------------------------
    // Update is called once per frame
    void Update()
    {
        if (!GM.isPaused)
        {
            updateRacingUI();
        }

        // that's not efficient, but ...
        if (RM.raceStarted)
        {
            GameUI.SetActive(true);
            PreStartUI.SetActive(false);
        }
        if (RM.raceFinished)
        {
            setRaceFinishedUI();
        }
    }
    //---------------------------------------
    public void pauseGame(bool paused)
    {

        if (paused)
        {
            PauseMenu.SetActive(true);
            GameUI.SetActive(false);
            Compass.SetActive(false);
            GetComponent<MenuManager>().initializeButton(pauseButton);
        }
        else
        {
            PauseMenu.SetActive(false);
            if (!RM.raceFinished)
            {
                GameUI.SetActive(true);
                Compass.SetActive(true);
                SettingsUI.SetActive(false);
            }
        }
    }
    //---------------------------------------
    public void openMenu()
    {
        Debug.Log("opening menu");
        // disable pause, enable menu, 
    }
    //---------------------------------------
    public void closeMenu()
    {
        // go back to pause menu!
    }
    public void setVolume(float vol)
    {
        Debug.Log("vol: " + vol);
        // implement fmod master fader thingy!

    }
    //---------------------------------------
    public void setQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
    //---------------------------------------
    public void setFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
    //---------------------------------------
    public void setResolution(int index)
    {
        Screen.SetResolution(resolutions[index].width, resolutions[index].height, Screen.fullScreen);
    }
    //---------------------------------------
    public void raceFinished()
    {
        GameUI.SetActive(false);
        RaceFinishedUI.SetActive(true);
        // setRaceFinishedUI();
        Debug.Log("RaceFinishedUI loaded");
        Compass.SetActive(false);
    }
    private void updateRacingUI()
    {
        GameUI.GetComponent<GameUI>().UpdateUI();
    }

    private void setRaceFinishedUI()
    {
        Player[] HighScore = RM.getHighScore();
        string text = "";
        for (int i = 0; i < HighScore.Length; i++)
        {
            if (HighScore[i].GetComponent<CurrentPositionManager>().finishTime > 0)
                text += HighScore[i].name + ":  " + HighScore[i].GetComponent<CurrentPositionManager>().finishTime + " secs\n";
            else
                text += " ...\n";
        }
        RaceFinishedUI.GetComponentInChildren<Text>().text = text;
        returnButton.Select();
    }

}
