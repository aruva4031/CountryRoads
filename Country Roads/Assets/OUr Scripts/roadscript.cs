using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roadscript : MonoBehaviour {
	
	public bool roadConnected = false;
	public GameObject generator;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame

	private void OnTriggerEnter(Collider other) {
		if(other.tag == "CrossPiece")
		{
			roadConnected = true;
			generator.GetComponent<GenerateRoads> ().test = true;

		}
	
	}
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "tree")
        {

            Destroy(collision.gameObject);
        }
    }

    public bool isRoadconnected() {
		return roadConnected;
	}
}
