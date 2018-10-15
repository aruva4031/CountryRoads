//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2015 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Camera/Fixed System/Main Camera")]
public class RCC_MainFixedCamera : MonoBehaviour {
	
	private Transform[] camPositions;
	private RCC_ChildFixedCamera[] childCams;
	private float[] childDistances;
	private RCC_Camera rccCamera;

	private float distance;
	internal Transform player;

	public float minimumFOV = 20f;
	public float maximumFOV = 60f;
	public bool canTrackNow = false;
	public bool drawGizmos = true;

	void Start(){

		StartCoroutine(GetVariables());

	}

	IEnumerator GetVariables () {

		yield return new WaitForSeconds(1);

		childCams = GetComponentsInChildren<RCC_ChildFixedCamera>();
		rccCamera = GameObject.FindObjectOfType<RCC_Camera>();

		foreach(RCC_ChildFixedCamera l in childCams){
			l.enabled = false;
			l.player = player;
		}

		camPositions = new Transform[childCams.Length];
		childDistances = new float[childCams.Length];

		for(int i = 0; i < camPositions.Length; i ++){
			camPositions[i] = childCams[i].transform;
			childDistances[i] = childCams[i].distance;
		}

	}

	void Act(){

		foreach(RCC_ChildFixedCamera l in childCams){
			l.enabled = false;
			l.player = player;
		}

	}

	void Update(){

		if(!player)
			return;

		if(canTrackNow)
			Tracking ();

		foreach(RCC_ChildFixedCamera l in childCams){
			if(l.player != player)
				l.player = player;
		}

	}

	void Tracking () {

		bool culling = false;
	
		for(int i = 0; i < camPositions.Length; i ++){

			distance = Vector3.Distance(camPositions[i].position, player.transform.position);
			
			if(distance <= childDistances[i]){

				culling = true;

				if(childCams[i].enabled != true)
					childCams[i].enabled = true;

				if(rccCamera.transform.parent != childCams[i].transform){
					rccCamera.transform.SetParent(childCams[i].transform);
					rccCamera.transform.localPosition = Vector3.zero;
					rccCamera.transform.localRotation = Quaternion.identity;
				}

				rccCamera.targetFieldOfView = Mathf.Lerp (rccCamera.targetFieldOfView, Mathf.Lerp (maximumFOV, minimumFOV, ((Vector3.Distance(rccCamera.transform.position, player.transform.position) * 2f) / childDistances[i])), Time.deltaTime * 3f);

			}else{
				
				if(childCams[i].enabled != false)
					childCams[i].enabled = false;

				if(rccCamera.transform.parent == childCams[i].transform){
					canTrackNow = false;
					rccCamera.transform.SetParent(null);
					childCams[i].transform.rotation = Quaternion.identity;
					rccCamera.cameraSwitchCount = 10;
					rccCamera.ChangeCamera();
				}

			}

		}

		if(!culling){

			if(rccCamera.cameraSwitchCount == 3){
				canTrackNow = false;
				rccCamera.transform.SetParent(null);
				rccCamera.cameraSwitchCount = 10;
				rccCamera.ChangeCamera();
			}

		}

	}

	void OnDrawGizmos() {

		if(drawGizmos){

			childCams = GetComponentsInChildren<RCC_ChildFixedCamera>();
			camPositions = new Transform[childCams.Length];
			childDistances = new float[childCams.Length];

			for(int i = 0; i < camPositions.Length; i ++){
				camPositions[i] = childCams[i].transform;
				childDistances[i] = childCams[i].distance;
				Gizmos.matrix = camPositions[i].transform.localToWorldMatrix;
				Gizmos.color = new Color(1f, 0f, 0f, .5f);
				Gizmos.DrawCube(Vector3.zero,new Vector3(childDistances[i] * 2f, childDistances[i] / 2f, childDistances[i] * 2f));
			}

		}

	}
	
}
