using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostCar : MonoBehaviour {
	public float CarShutdownTime;
	public float Duration;
	private float NextEvent = 0.00f;
	public float speed;
	public int SanityLowerOnceAmount;
	Rigidbody thisCar;
	private bool SpookeEm = false;
	private GameObject camera;
	private GameObject SanityHandler;
	// Use this for initialization
	void Start () {
		thisCar = GetComponent<Rigidbody> ();
		SanityHandler = GameObject.FindGameObjectWithTag("SanityHandler");
		camera = GameObject.FindGameObjectWithTag("GameCamera");
	}
	
	// Update is called once per frame
	void Update ()
	{
		//keeps accelerating the ghost car for the duration of its life
		if (Time.time < Duration) {
			thisCar.velocity = transform.forward * speed;
			speed = speed + 0.05f;
		}
		//if its duration is over it deactivates itself
		else if(Time.time > Duration) {
			this.gameObject.SetActive(false);
		}
		//lowers the players sanity by looking at the car
		if(camera.GetComponent<CameraController>().isGhostSeen(this.tag) && SpookeEm == false){
			SanityHandler.GetComponent<SanityMeter>().lowerOnce(SanityLowerOnceAmount);
			SpookeEm = true;
		}
        else
        {
            SanityHandler.GetComponent<SanityMeter>().lowerSanity=false;
        }

	}

	//checks to see if the ghost car has hit the players car in order to start the event
	private void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player" && Time.time > NextEvent) {
			NextEvent = Time.time + Duration;
			other.gameObject.GetComponent<cr_controller> ().CarShutdown (CarShutdownTime);
		}
	}
}
