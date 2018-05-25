using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Compass : MonoBehaviour
{
    public TextMesh textField;
    public bool invertCompassDirection = true;
    public float dist = 0;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    public void updateCompass(Transform currentTransform, Vector3 target)
    {
        transform.position = currentTransform.position;
        transform.rotation = currentTransform.rotation;

        Vector3 dir = invertCompassDirection ? target - transform.position : transform.position - target;
        dist = dir.magnitude;
        dir.Normalize();

        Quaternion rot = Quaternion.LookRotation(dir, transform.up);
    
        transform.rotation = rot;
        textField.text = dist.ToString("F0") + "m";

    }
}
