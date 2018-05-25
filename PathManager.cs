using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathManager : MonoBehaviour
{
	public Transform[] WayPoints;
	private WPScript[] WPs;
	public int PointSize = 5;
	public bool ClosedPath = true;
	public bool ReIndexPath = false;
	//---------------------------------------
	void Start()
	{
		// get all WayPoints
		if(ReIndexPath)WayPoints = new Transform[transform.childCount];
		WPs = new WPScript[transform.childCount];

		int i = 0;
		foreach (Transform c in transform) {
			if (ReIndexPath) {
				WayPoints [i] = c;
				WPs [i] = c.GetComponent<WPScript> ();
			} else
				WPs [i] = WayPoints [i].GetComponent<WPScript>();
			if (!WPs [i])
				print ("meh?");
			WPs [i].setID (i);
			i++;
		}
		Debug.Log ("waypoints initialised!");
        WPs[0].Activate();
    }
	
	//---------------------------------------
	public Transform GetPoint(int index){
        if (index < 0) index += WayPoints.Length;
        else if (index >= WayPoints.Length) index -= WayPoints.Length;
		return WayPoints [index];
    }
    //---------------------------------------
    public int getNumberOfWPs(){
		return WayPoints.Length-1;
    }
    //---------------------------------------
    public int getPointSize(){
		return PointSize;
    }
    //---------------------------------------
    public Transform GetLastPoint(){
		return WayPoints [WayPoints.Length - 1];
    }
    //---------------------------------------
    public WPScript getWP(int id) {
        if (id >= WPs.Length) return WPs[0];
        else return WPs[id];
    }
	//---------------------------------------
	void OnDrawGizmos ()
	{
		if (WayPoints.Length == 0)
			return;
		for (int i = 0; i < WayPoints.Length - 1; i++) {
			Gizmos.DrawLine (WayPoints [i].position, WayPoints [i + 1].position);
			//Gizmos.DrawSphere (WayPoints [i].position, PointSize);
		}
		//Gizmos.DrawSphere (WayPoints [WayPoints.Length - 1].position, PointSize);
		if (ClosedPath) {
			Gizmos.DrawLine (WayPoints [WayPoints.Length - 1].position, WayPoints [0].position);

		}
    }
    //---------------------------------------
}
