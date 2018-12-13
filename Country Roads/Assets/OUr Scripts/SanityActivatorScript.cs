using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanityActivatorScript : MonoBehaviour {

	private Renderer[] hollowRoad;
	private bool playerNear = false;

    // for crazy tree script
    public SanityMeter sanity;

	// Use this for initialization
	void Start () {
		hollowRoad = this.gameObject.GetComponentsInChildren<Renderer>();
		setOff ();

	}

	// Update is called once per frame
	void Update () {
		if (sanity.getSanity () <= 45 && sanity.getSelector() == 4) {
			setOn ();
		}

	}

	public void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "player") {
			playerNear = true;
			Debug.Log ("Road: Player is near");
		}
	}

	public void OnTriggerStay(Collider col)
	{
		if (col.gameObject.tag == "player") {
			playerNear = true;
			Debug.Log ("Road: Player is hear");
		}
	}

	public void OnTriggerExit(Collider col)
	{
		if (col.gameObject.tag == "player") {
			playerNear = false;
			Debug.Log ("Road: Player is far");
		}
	}

	public void setOn()
	{
		if (playerNear) {
			foreach (Renderer piece in hollowRoad) {
				piece.enabled = true;
			}
			Debug.Log ("Turn on Road");
		}
	}

	public void setOff()
	{
		foreach (Renderer piece in hollowRoad) {
			piece.enabled = false;
		}
		Debug.Log ("Turn off Road");
	}
}
