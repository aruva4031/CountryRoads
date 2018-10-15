using UnityEngine;
using System.Collections;

public class RCC_LockRot : MonoBehaviour {

	Quaternion orgRotation;

	void Awake () {

		orgRotation = transform.localRotation;
	
	}
	
	// Update is called once per frame
	void Update () {

		transform.localRotation = orgRotation;
	
	}

}
