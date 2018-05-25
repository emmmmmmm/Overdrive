using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MenuManager : MonoBehaviour {
    [FMODUnity.EventRef]
    public string menuHoverSound;
    [FMODUnity.EventRef]
    public string menuSelectSound;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void onHover()
    {
        if (menuHoverSound != "") FMODUnity.RuntimeManager.PlayOneShot(menuHoverSound);
    }
    public void onSelect()
    {
        if (menuSelectSound != "") FMODUnity.RuntimeManager.PlayOneShot(menuSelectSound);
    }

    public void initializeButton(Button initialySelectedButton)
    {
        initialySelectedButton.Select();
        initialySelectedButton.OnSelect(null); // needed to actually select the button ... for whatever reason.
    }
}
