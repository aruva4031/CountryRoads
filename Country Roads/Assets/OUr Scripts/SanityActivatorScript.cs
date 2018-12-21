using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanityActivatorScript : MonoBehaviour {

	//used to store the road renderer
    private Renderer[] hollowRoad;
	//used to see if the player is near 
    private bool playerNear = false;

    // for crazy tree script
    public SanityMeter sanity;

    // Use this for initialization
    void Start()
    {
		//get all the haulcinating roads renderers 
        hollowRoad = this.gameObject.GetComponentsInChildren<Renderer>();
		//turn off at start
        setOff();
		//get the sanitymeter from sanity handler
        sanity = GameObject.FindWithTag("SanityHandler").GetComponent<SanityMeter>();
    }

    // Update is called once per frame
    void Update()
    {
		//if the players sanity is less that 45 and event selects the halucinating roads
        if (sanity.getSanity() <= 45 && sanity.getSelector() == 4)
        {
			//turn on the renderers of the roads
            setOn();
        }

    }

	//if the player enter the colldier of the event 
    public void OnTriggerEnter(Collider col)
    {
		//player is near 
        if (col.gameObject.tag == "player")
        {
            playerNear = true;
            Debug.Log("Road: Player is near");
        }
    }

	//if the player stays in the collider of the event 
    public void OnTriggerStay(Collider col)
    {
		//player is near 
        if (col.gameObject.tag == "player")
        {
            playerNear = true;
            Debug.Log("Road: Player is hear");
        }
    }

	//is the player leaves the coillder of the event 
    public void OnTriggerExit(Collider col)
    {
		//player is far 
        if (col.gameObject.tag == "player")
        {
            playerNear = false;
            Debug.Log("Road: Player is far");
        }
    }

	//turn on the renders of the fake roads 
    public void setOn()
    {
		//if the palyer is near 
        if (playerNear)
        {
			//turn on the fake roads
            foreach (Renderer piece in hollowRoad)
            {
                piece.enabled = true;
            }
            Debug.Log("Turn on Road");
        }
    }

	//turns off the fake road 
    public void setOff()
    {
		//turn off the fake roads 
        foreach (Renderer piece in hollowRoad)
        {
            piece.enabled = false;
        }
        Debug.Log("Turn off Road");
    }
}
