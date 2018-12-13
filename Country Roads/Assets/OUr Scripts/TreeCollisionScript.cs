using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeCollisionScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.tag == "straightRoad" || col.gameObject.tag == "curvedroadleft" || col.gameObject.tag == "curvedroadright" ) {
			Destroy (this);
		}

	}

}
