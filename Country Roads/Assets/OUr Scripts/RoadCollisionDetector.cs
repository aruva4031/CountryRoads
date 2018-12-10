using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadCollisionDetector : MonoBehaviour {


	public bool hitARoad = false;
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	private void OnTriggerEnter(Collider other) {
		if(other.tag == "straightRoad" || other.tag == "curvedroad" || other.tag == "curvedroadleft")
		{
			hitARoad = true;
		}
	}
}
