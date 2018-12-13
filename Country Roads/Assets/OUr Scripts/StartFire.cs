using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartFire : MonoBehaviour {

	//public Transform point1;
	//public Transform point2;
	//public Transform point3;
	//public Transform point4;

	public GameObject house;
	public bool isfire = false;

	//private bool ispoint1 = true;
	//private bool ispoint2= false;
	//private bool ispoint3 = false;
	//private bool ispoint4 = false;


	// Use this for initialization
	void Start () {
		StartCoroutine ("startFire");
	}
	
	// Update is called once per frame
	void Update () {

		if (isfire) {
			this.transform.RotateAround (house.transform.position, Vector3.up, 45 * Time.deltaTime);

		}

	}


	public IEnumerator startFire()
	{
		yield return new WaitForSeconds (6.0f);

		isfire = true;
	}
}