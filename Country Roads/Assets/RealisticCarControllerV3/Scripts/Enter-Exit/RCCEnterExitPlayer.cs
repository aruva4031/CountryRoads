//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2015 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using System.Collections;

public class RCCEnterExitPlayer : MonoBehaviour {
	
	public float maxRayDistance= 2.0f;
	private bool showGui = false;

	void Start(){

		GameObject carCamera = GameObject.FindObjectOfType<RCC_Camera>().gameObject;
		carCamera.SetActive(false);

	}
	
	void Update (){
		
		Vector3 direction= transform.TransformDirection(Vector3.forward);
		RaycastHit hit;

		if(Physics.Raycast(transform.position, direction, out hit, maxRayDistance)){

			if(hit.transform.GetComponentInParent<RCC_CarControllerV3>()){

				showGui = true;

				if(Input.GetKeyDown(RCC_Settings.Instance.enterExitVehicleKB))
					hit.transform.GetComponentInParent<RCC_CarControllerV3>().SendMessage("Act", GetComponentInParent<CharacterController>().gameObject, SendMessageOptions.DontRequireReceiver);

			}else{

				showGui = false;

			}
			
		}else{

			showGui = false;

		}
		
	}
	
	void OnGUI (){
		
		if(showGui){
			GUI.Label( new Rect(Screen.width - (Screen.width/1.7f),Screen.height - (Screen.height/1.2f),800,100),"Press ''" + RCC_Settings.Instance.enterExitVehicleKB.ToString() + "'' key to Get In");
		}
		
	}
	
}