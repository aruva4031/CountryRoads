using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostchildPlayerCollision : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    }

    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log("In.");
        if (collider.gameObject.tag == "GhostChild")
        {
            Debug.Log("In2.");
            if (collider.gameObject.GetComponent<GhostChild>())
            {
                Debug.Log("In3.");
                collider.gameObject.GetComponent<GhostChild>().startHaunting();
                Debug.Log("In4.");
            }
        }
    }
}
