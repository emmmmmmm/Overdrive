using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

using UnityEngine;

public class pauseMenu : MonoBehaviour {
    [FMODUnity.EventRef]
    public string menuHoverSound;
    [FMODUnity.EventRef]
    public string menuSelectSound;
    public Button initialySelectedButton;
    
    private void Awake()
    {
        initialySelectedButton.Select();
    }
    
    public void onHover() {
        if(menuHoverSound!="") FMODUnity.RuntimeManager.PlayOneShot(menuHoverSound);
    }
    public void onSelect() {
        if (menuSelectSound != "")  FMODUnity.RuntimeManager.PlayOneShot(menuSelectSound);
    }
    public void initializeButton() {
        initialySelectedButton.Select();
        initialySelectedButton.OnSelect(null); // needed to actually select the button ... for whatever reason.
    }
}
