using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerNamePlate : MonoBehaviour
{

    public Text usernameText;
   private Player player;

    private void Start()
    {
        player = GetComponentInParent<Player>();
    }
    // Update is called once per frame
    void SetText()
    {
        usernameText.text = player.currentPosition.ToString()+ " - "+player.name;
    }
    private void Update()
    {
        SetText();
        FaceCamera();
    }
    void FaceCamera()
    {
        Camera cam = Camera.main;
        transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward, cam.transform.rotation * Vector3.up);


    }
}
