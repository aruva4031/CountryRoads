using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrazyTreeScript : MonoBehaviour {

	private Renderer[] crazyTrees;
	private bool playerNear = false;

    public SanityMeter sanity;

	// Use this for initialization
	void Start () {
		crazyTrees = this.gameObject.GetComponentsInChildren<Renderer>();
		setOn ();

	}

    // Update is called once per frame
    void Update() {
        if (sanity.getSanity() <= 45 && sanity.getSelector() == 4)
        {
            setOff();
        } 
	}

	public void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "player") {
			playerNear = true;
			Debug.Log ("Trees: Player is near");
		} 
	}

	public void OnTriggerStay(Collider col)
	{
		if (col.gameObject.tag == "player") {
			playerNear = true;
			Debug.Log ("Trees: Player is hear");
		} 
	}

	public void OnTriggerExit(Collider col)
	{
		if (col.gameObject.tag == "player") {
			playerNear = false;
			Debug.Log ("Trees: Player is far");
		}
	}


	public void setOn()
	{
		foreach (Renderer tree in crazyTrees) {
			tree.enabled = true;
		}
		Debug.Log ("Turn on Trees");
	}

	public void setOff()
	{
		if (playerNear) {
			foreach (Renderer tree in crazyTrees) {
				tree.enabled = false;

			}
			Debug.Log ("Turn off Trees");
		}
	}


}
