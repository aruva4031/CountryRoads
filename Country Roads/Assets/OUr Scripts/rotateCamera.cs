using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateCamera : MonoBehaviour {

	public GameObject snickers;
	private bool islookingforsnickers = false;

	// Use this for initialization
	void Start () {
		StartCoroutine ("lookAtSnickers");
	}
	
	// Update is called once per frame
	void Update () {
		if (islookingforsnickers) {
			this.transform.rotation = Quaternion.RotateTowards (this.transform.rotation, snickers.transform.rotation, 45 * Time.deltaTime);
		}
	}

	public IEnumerator lookAtSnickers()
	{
		yield return new WaitForSeconds (16.0f);
		islookingforsnickers = true;
	}
}
