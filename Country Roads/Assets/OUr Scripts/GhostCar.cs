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
		if (Time.time < Duration) {
			thisCar.velocity = transform.forward * speed;
			speed = speed + 0.05f;
		}
		else if(Time.time > Duration) {
			this.gameObject.SetActive(false);
		}
		if(camera.GetComponent<CameraController>().isGhostSeen(this.tag) && SpookeEm == false){
			SanityHandler.GetComponent<SanityMeter>().lowerOnce(SanityLowerOnceAmount);
			SpookeEm = true;
		}

	}

	private void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player" && Time.time > NextEvent) {
			NextEvent = Time.time + Duration;
			other.gameObject.GetComponent<cr_controller> ().CarShutdown (CarShutdownTime);
		}
	}
}
