using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraController : MonoBehaviour {
    public GameObject player;

    // Use this for initialization
    void Start () {
        this.transform.rotation = player.transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log(Input.GetAxis("HorizontalRS"));

        //TODO: fixupdate method

        if (Input.GetAxis("HorizontalRS").Equals(1))
        {
            this.transform.Rotate(Vector3.down*Time.deltaTime*3);
        }
        else if (Input.GetAxis("HorizontalRS")<1)
        {
            this.transform.Rotate(Vector3.zero);
        }
	}
}
