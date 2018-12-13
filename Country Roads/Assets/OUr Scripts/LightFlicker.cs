using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour {

	private Light lightfix;

	// Use this for initialization
	void Start () {
		lightfix = this.GetComponent<Light>();

		StartCoroutine ("stopInvoke");

	}
	
	// Update is called once per frame
	void Update () {



	}

	public void switchLight()
	{
		if (lightfix.enabled == true) {
			lightfix.enabled = false;
		} else {
			lightfix.enabled = true;
		}
	}


	public IEnumerator stopInvoke()
	{
		InvokeRepeating ("switchLight", 1.0f, 0.5f);
		yield return new WaitForSeconds (5.0f);
		if (lightfix.enabled == true) {
			switchLight ();
		}
		CancelInvoke ();
	}
}
