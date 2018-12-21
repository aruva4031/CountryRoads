using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadCollisionDetector : MonoBehaviour {


	public bool hitARoad = false;
	public bool connectionMade = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter(Collider other) {
		//Debug.Log ("I hit: " + other.tag);
		//checks to see if a road collision would occur with the test peice
		if(other.tag == "straightRoad" || other.tag == "curvedroadright" || other.tag == "curvedroadleft")
		{
			
			hitARoad = true;
		}
		if(other.tag == "crossroad"){
			connectionMade = true;
		}
	}
	private void OnTriggerExit(Collider other) {
		if(other.tag == "straightRoad" || other.tag == "curvedroadright" || other.tag == "curvedroadleft")
		{
			hitARoad = false;
		}
	}
}
