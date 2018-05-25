using UnityEngine;
using System.Collections;

using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.

public class MenuButton : MonoBehaviour {

	// Use this for initialization
	public AudioClip clip;
	private AudioSource sound;
	public  Button button;
	public void Start(){
		sound = GetComponent<AudioSource> ();
		sound.playOnAwake = false;
		sound.loop = false;
		button.Select ();
	}
	public void OnHover(){
        if(sound) sound.PlayOneShot (clip);
	}
}
