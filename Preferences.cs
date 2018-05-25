using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Preferences : MonoBehaviour
{
    Resolution[] resolutions;
    public Dropdown resolutionDropdown;
    /* [FMODUnity.EventRef]
     public string FxVCA;
     [FMODUnity.EventRef]
     public string MusicVCA;
     */
    // Use this for initialization
    void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }
    //---------------------------------------
    // good to know: linear-> db conversion
    // float db = linear > 0 ? 20.0f * Mathf.Log10(linear * Mathf.Sqrt(2.0f)) : -80.0f;
    //
    public void SetVolumeFx(float vol)
    {
        //  Debug.Log("vol: " + vol);
        string vcaPath = "vca:/FXVCA";
        FMOD.Studio.VCA vca = FMODUnity.RuntimeManager.GetVCA(vcaPath);
        vca.setVolume(vol);
    }
    public void SetVolumeMusic(float vol)
    {
        string vcaPath = "vca:/MusicVCA";
        FMOD.Studio.VCA vca = FMODUnity.RuntimeManager.GetVCA(vcaPath);
        vca.setVolume(vol);
    }
    //---------------------------------------
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
    //---------------------------------------
    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
    //---------------------------------------
    public void SetResolution(int index)
    {
        if (resolutions == null) return;
        Screen.SetResolution(resolutions[index].width, resolutions[index].height, Screen.fullScreen);
    }
}
