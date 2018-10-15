//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2015 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using System.Collections;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Camera/Camera Config")]
public class RCC_CameraConfig : MonoBehaviour {

	public bool automatic = true;
	private Bounds combinedBounds;

	public float distance = 10f;
	public float height = 5f;

	void Awake(){

		if(automatic){

			Quaternion orgRotation = transform.rotation;
			transform.rotation = Quaternion.identity;

			combinedBounds = GetComponentInChildren<Renderer>().bounds;
			Renderer[] renderers = GetComponentsInChildren<Renderer>();

			foreach (Renderer render in renderers) {
				if (render != GetComponent<Renderer>() && render.GetComponent<ParticleSystem>() == null)
					combinedBounds.Encapsulate(render.bounds);
			}

			transform.rotation = orgRotation;

			distance = combinedBounds.size.z * 1.1f;
			height = combinedBounds.size.z * .35f;

		}

	}

	public void SetCameraSettings () {

		RCC_Camera cam = GameObject.FindObjectOfType<RCC_Camera>();
		 
		if(!cam)
			return;
			
		cam.distance = distance;
		cam.height = height;

	}

//	void OnDrawGizmos(){
//
//		Gizmos.matrix = transform.localToWorldMatrix;
//		Gizmos.color = new Color(1f, 0f, 0f, .5f);
//		Gizmos.DrawCube(Vector3.zero,new Vector3(combinedBounds.extents.x * 2f, combinedBounds.extents.y * 2f, combinedBounds.extents.z * 2f));
//
//	}

}
