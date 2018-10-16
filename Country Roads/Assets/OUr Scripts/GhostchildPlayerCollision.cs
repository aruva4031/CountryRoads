using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 /*This script's purpose is to create a connection between player and ghost child. Since the player has the collider and
  *the ghost child has the trigger collider, this script needs to be part of the player, while the GhostChild script should
  *be appended to the GhostChild object. Therefore, this scripts creates the connection.*/
public class GhostchildPlayerCollision : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    }

    //if the player collides with a trigger collider
    private void OnTriggerEnter(Collider collider)
    {
        //if the player collides with the ghost child
        if (collider.gameObject.tag == "GhostChild")
        {
            //if the ghost child has the GhostChild script
            if (collider.gameObject.GetComponent<GhostChild>())
            {
                //make the ghost child start haunting the player in the car by calling the startHaunting() function
                collider.gameObject.GetComponent<GhostChild>().startHaunting();
            }
        }
    }
}
