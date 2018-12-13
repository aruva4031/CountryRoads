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

    public void OnCollisionStay(Collision collision)
    {
        Debug.Log(collision.transform.tag);
        if (collision.transform.tag == "straightRoad" || collision.transform.tag == "curvedroadright" || collision.transform.tag == "curvedroadleft")
        {
            Destroy(this.gameObject);

        }
    }



}
