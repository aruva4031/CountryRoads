using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Script that turns off and on the crazy trees 

public class CrazyTreeScript : MonoBehaviour {

	//store all the renders of the crazy trees in a array 
    private Renderer[] crazyTrees;
	//check to see if the player is near 
    private bool playerNear = false;

	//get the functions from the sanity meter script
    public SanityMeter sanity;

    // Use this for initialization
    void Start()
    {
		//get the crazy tree renderer and store it in an array 
        crazyTrees = this.gameObject.GetComponentsInChildren<Renderer>();
		//call the set on function
        setOn();
		//get the sanity meter from the sanity handler
        sanity = GameObject.FindWithTag("SanityHandler").GetComponent<SanityMeter>();
    }

    // Update is called once per frame
    void Update()
    {
		//if the players sanity is less than 45 and the event generatoor selected the crazy tree edvent
        if (sanity.getSanity() <= 45 && sanity.getSelector() == 4)
        {
			//turn off the render of the trees 
            setOff();
        }
    }

	//if the player is near the event 
    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "player")
        {
			//set the boolean to true
            playerNear = true;
            Debug.Log("Trees: Player is near");
        }
    }

	//if the player stays in the collider of the edvent
    public void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "player")
        {
			//set the boolean to true
            playerNear = true;
            Debug.Log("Trees: Player is hear");
        }
    }

	//if the player leaves the area 
    public void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "player")
        {
			//set the boolean to false
            playerNear = false;
            Debug.Log("Trees: Player is far");
        }
    }


	//turns on all the tree renders
    public void setOn()
    {
		//for each crazy tree
        foreach (Renderer tree in crazyTrees)
        {
			//turn on the renderer
            tree.enabled = true;
        }
        Debug.Log("Turn on Trees");
    }

	//turns off the tree renders
    public void setOff()
    {
		//if the player is near 
        if (playerNear)
        {
			//for each tree turn off the renderer 
            foreach (Renderer tree in crazyTrees)
            {
                tree.enabled = false;

            }
            Debug.Log("Turn off Trees");
        }
    }


}
