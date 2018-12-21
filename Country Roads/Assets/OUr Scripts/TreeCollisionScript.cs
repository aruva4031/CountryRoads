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
	//if the tree collides
    public void OnCollisionStay(Collision collision)
    {
		//with any piece of road 
        Debug.Log(collision.transform.tag);
        if (collision.transform.tag == "straightRoad" || collision.transform.tag == "curvedroadright" || collision.transform.tag == "curvedroadleft")
        {
			//destory this tree
            Destroy(this.gameObject);

        }
    }



}
