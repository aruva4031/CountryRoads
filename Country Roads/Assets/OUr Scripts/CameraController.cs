using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraController : MonoBehaviour {
    public GameObject player;
    public float lerpValue;

    // Use this for initialization
    void Start () {
        this.transform.rotation = player.transform.rotation;
        this.lerpValue = 0.01F;
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log(Input.GetAxis("HorizontalRS"));

        //TODO: fixupdate method

        if (Input.GetAxis("HorizontalRS").Equals(1))
        {
            // look left
            this.transform.Rotate(Vector3.down*Time.deltaTime * -20);
        }
        else if (Input.GetAxis("HorizontalRS").Equals(-1))
        {
            // look right
            this.transform.Rotate(Vector3.down * Time.deltaTime * 20);
        }
        else if (Input.GetAxis("HorizontalRS") < 1 || Input.GetAxis("HorizontalRS") > 1)
        {
            // snap the camera back to looking forward in not moving camera
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, player.transform.rotation, Time.time * lerpValue);
        }
	}
}
