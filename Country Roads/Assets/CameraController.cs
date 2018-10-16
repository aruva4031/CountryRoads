using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraController : MonoBehaviour {
    public GameObject player;

    // Use this for initialization
    void Start () {
        transform.rotation = player.transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log(Input.GetAxis("HorizontalRS"));
        if (Input.GetAxis("HorizontalRS").Equals())
        {

        }
	}
}
