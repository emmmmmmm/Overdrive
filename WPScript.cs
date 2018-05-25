using UnityEngine;
using System.Collections;

public class WPScript : MonoBehaviour {
	public int id = 0;
	public bool drawGizmo = false;
	public float radius = 15;
    public bool active = false;

    [FMODUnity.EventRef]
    public string wayPointSoundEvent;
    FMOD.Studio.EventInstance wayPointSound;

    private void Start()
    {
       // wayPointSound = FMODUnity.RuntimeManager.CreateInstance(wayPointSoundEvent);
       // wayPointSound.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject,null));
    }
    void Awake(){
        
        Deactivate();

    }

	// Use this for initialization
	public void setID(int _id){
		id = _id+1;
	}
	public int getID(){
		return id;
	}
	void OnDrawGizmos ()
	{
		if(drawGizmo)
			Gizmos.DrawSphere (transform.position, radius);
	}
    public void Activate() {
        GetComponent<MeshRenderer>().enabled = true;
        active = true;
     //   wayPointSound.start();
    }
    public void Deactivate() {
        GetComponent<MeshRenderer>().enabled = false;
        active = false;
       // wayPointSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}
