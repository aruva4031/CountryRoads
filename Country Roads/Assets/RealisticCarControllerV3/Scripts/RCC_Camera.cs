//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2015 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using System.Collections;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Camera/Main Camera")]
public class RCC_Camera : MonoBehaviour{
	
	// The target we are following
	public Transform playerCar;
	public Transform _playerCar{get{return playerCar;}set{playerCar = value;	GetPlayerCar();}}
	private Rigidbody playerRigid;

	private Camera cam;
	public GameObject pivot;
	private GameObject boundCenter;

	public CameraMode cameraMode;
	public enum CameraMode{TPS, FPS, WHEEL, FIXED}

	// The distance in the x-z plane to the target
	public float distance = 6f;
	
	// the height we want the camera to be above the target
	public float height = 2f;

	private float heightDamping = 5f;
	private float rotationDamping = 3f;

	public float targetFieldOfView = 60f;
	public float minimumFOV = 55f;
	public float maximumFOV = 70f;
	
	public float maximumTilt = 15f;
	private float tiltAngle = 0f;

	internal int cameraSwitchCount = 0;
	private RCCHoodCamera hoodCam;
	private RCCWheelCamera wheelCam;
	private RCC_MainFixedCamera fixedCam;

	private Vector3 targetPosition = Vector3.zero;
	private Vector3 pastFollowerPosition, pastTargetPosition = Vector3.zero;

	private float speed = 0f;

	public float maxShakeAmount = .01f;

	private Vector3 localVector;
	private Vector3 collisionPos;
	private Quaternion collisionRot;

	private float index = 0f;

	void Awake(){

		cam = GetComponentInChildren<Camera>();

	}
	
	void GetPlayerCar(){

		if(!playerCar)
			return;
		
		playerRigid = playerCar.GetComponent<Rigidbody>();
		hoodCam = playerCar.GetComponentInChildren<RCCHoodCamera>();
		wheelCam = playerCar.GetComponentInChildren<RCCWheelCamera>();
		fixedCam = GameObject.FindObjectOfType<RCC_MainFixedCamera>();

		transform.position = playerCar.transform.position;
		transform.rotation = playerCar.transform.rotation * Quaternion.Euler(10f, 0f, 0f);

		if(playerCar.GetComponent<RCC_CameraConfig>())
			playerCar.GetComponent<RCC_CameraConfig>().SetCameraSettings();

//		Quaternion orgRotation = playerCar.rotation;
//		playerCar.rotation = Quaternion.identity;
//
//		Bounds combinedBounds = playerCar.GetComponentInChildren<Renderer>().bounds;
//		Renderer[] renderers = playerCar.GetComponentsInChildren<Renderer>();
//
//		foreach (Renderer render in renderers) {
//			if (render != playerCar.GetComponent<Renderer>() && render.GetComponent<ParticleSystem>() == null)
//				combinedBounds.Encapsulate(render.bounds);
//		}
//
//		playerCar.rotation = orgRotation;
//
//		boundCenter = new GameObject("Bounds Center");
//		boundCenter.transform.position = combinedBounds.center;
//		boundCenter.transform.rotation = playerCar.rotation;
//		boundCenter.transform.SetParent(playerCar, true);

	}

	public void SetPlayerCar(GameObject player){

		_playerCar = player.transform;

	}
	
	void Update(){
		
		// Early out if we don't have a player
		if (!playerCar || !playerRigid){
			GetPlayerCar();
			return;
		}

		// Speed of the vehicle.
		speed = Mathf.Lerp(speed, playerRigid.velocity.magnitude * 3.6f, Time.deltaTime * .5f);

		if(index > 0)
			index -= Time.deltaTime * 5f;

		if(cameraMode == CameraMode.TPS){
			
		}

		cam.fieldOfView = targetFieldOfView;
			
	}
	
	void LateUpdate (){
		
		// Early out if we don't have a target
		if (!playerCar || !playerRigid)
			return;

		if (!playerCar.gameObject.activeSelf)
			return;

		if(Input.GetKeyDown(RCC_Settings.Instance.changeCameraKB)){
			ChangeCamera();
		}

		switch(cameraSwitchCount){
		case 0:
			cameraMode = CameraMode.TPS;
			break;
		case 1:
			cameraMode = CameraMode.FPS;
			break;
		case 2:
			cameraMode = CameraMode.WHEEL;
			break;
		case 3:
			cameraMode = CameraMode.FIXED;
			break;
		}

		pastFollowerPosition = transform.position;
		pastTargetPosition = targetPosition;

		switch(cameraMode){

		case CameraMode.TPS:
			TPS();
			break;

		case CameraMode.FPS:
			if(hoodCam){
				FPS();
			}else{
				cameraSwitchCount ++;
				ChangeCamera();
			}
			break;
		
		case CameraMode.WHEEL:
			if(wheelCam){
				WHEEL();
			}else{
				cameraSwitchCount ++;
				ChangeCamera();
			}
			break;

		case CameraMode.FIXED:
			if(fixedCam){
				FIXED();
			}else{
				cameraSwitchCount ++;
				ChangeCamera();
			}
			break;

		}

	}

	public void ChangeCamera(){

		cameraSwitchCount ++;
		if(cameraSwitchCount >= 4)
			cameraSwitchCount = 0;
		if(fixedCam)
			fixedCam.canTrackNow = false;

	}

	void FPS(){

		if(transform.parent != hoodCam){
			transform.SetParent(hoodCam.transform, false);
			transform.position = hoodCam.transform.position;
			transform.rotation = hoodCam.transform.rotation;
			targetFieldOfView = 70;
		}

	}

	void WHEEL(){

		if(transform.parent != wheelCam){
			transform.SetParent(wheelCam.transform, false);
			transform.position = wheelCam.transform.position;
			transform.rotation = wheelCam.transform.rotation;
			targetFieldOfView = 60;
		}

	}

	void TPS(){

		if(transform.parent != null)
			transform.SetParent(null);

		if(targetPosition == Vector3.zero){
			targetPosition = _playerCar.position;
			targetPosition -= transform.rotation * Vector3.forward * distance;
			transform.position = targetPosition;
			pastFollowerPosition = transform.position;
			pastTargetPosition = targetPosition;
		}

		targetFieldOfView = Mathf.Lerp(cam.fieldOfView , Mathf.Lerp(minimumFOV, maximumFOV, speed / 150f) + (5f * Mathf.Cos (1f * index)), Time.deltaTime * 10f);
		tiltAngle = Mathf.Lerp(0f, maximumTilt * (int)Mathf.Clamp(-playerCar.InverseTransformDirection(playerRigid.velocity).x, -1, 1), Mathf.Abs(playerCar.InverseTransformDirection(playerRigid.velocity).x) / 50f);

		// Calculate the current rotation angles.
		float wantedRotationAngle = playerCar.eulerAngles.y + Mathf.Clamp((playerRigid.transform.InverseTransformDirection(playerRigid.velocity).z), -10f, 0f) * 18f;
		float wantedHeight = playerCar.position.y + height;
		float currentHeight = targetPosition.y;
		float currentRotationAngle = transform.eulerAngles.y;

		rotationDamping = Mathf.Lerp(1f, 5f, (playerRigid.velocity.magnitude * 3.6f) / 10f);

		currentRotationAngle = Mathf.LerpAngle (currentRotationAngle, wantedRotationAngle, Time.deltaTime * rotationDamping);
		Quaternion currentRotation = Quaternion.Euler (0, currentRotationAngle, tiltAngle);

		// Damp the height
		currentHeight = Mathf.Lerp (currentHeight, wantedHeight, heightDamping * Time.deltaTime);

		targetPosition = playerCar.position;

		if(RCC_Settings.Instance.behaviorType != RCC_Settings.BehaviorType.Drift){
			targetPosition -= currentRotation * Vector3.forward * distance;
			targetPosition = new Vector3(targetPosition.x, currentHeight, targetPosition.z);
		}else{
			targetPosition -= transform.rotation * Vector3.forward * distance;
			targetPosition = new Vector3(targetPosition.x, currentHeight, targetPosition.z);
		}

		transform.position = SmoothApproach(pastFollowerPosition, pastTargetPosition, targetPosition, Mathf.Clamp(.1f, speed, Mathf.Infinity));

		pastFollowerPosition = transform.position;
		pastTargetPosition = targetPosition;

		transform.LookAt (new Vector3(playerCar.position.x, playerCar.position.y + 1f, playerCar.position.z));

		pivot.transform.localPosition = Vector3.Lerp(pivot.transform.localPosition, (new Vector3(Random.insideUnitSphere.x / 2f, Random.insideUnitSphere.y, Random.insideUnitSphere.z) * speed * maxShakeAmount), Time.deltaTime * 1f);
		collisionPos = Vector3.Lerp(collisionPos, Vector3.zero, Time.deltaTime * 5f);
		collisionRot = Quaternion.Lerp(collisionRot, Quaternion.identity, Time.deltaTime * 5f);
		pivot.transform.localPosition = Vector3.Lerp(pivot.transform.localPosition, collisionPos, Time.deltaTime * 5f);
		pivot.transform.localRotation = Quaternion.Lerp(pivot.transform.localRotation, collisionRot, Time.deltaTime * 5f);

	}

	void FIXED(){

		if(transform.parent != null)
			transform.SetParent(null);

		fixedCam.canTrackNow = true;
		fixedCam.player = playerCar;

	}

	private Vector3 SmoothApproach( Vector3 pastPosition, Vector3 pastTargetPosition, Vector3 targetPosition, float delta){

		if(float.IsNaN(delta) || float.IsInfinity(delta) || pastPosition == Vector3.zero || pastTargetPosition == Vector3.zero || targetPosition == Vector3.zero)
			return transform.position;

		float t = Time.deltaTime * delta;
		Vector3 v = ( targetPosition - pastTargetPosition ) / t;
		Vector3 f = pastPosition - pastTargetPosition + v;
		return targetPosition - v + f * Mathf.Exp( -t );

	}

	public void Collision(Collision collision){

		if(!enabled || cameraMode != CameraMode.TPS)
			return;
		
		Vector3 colRelVel = collision.relativeVelocity;
		colRelVel *= 1f - Mathf.Abs(Vector3.Dot(transform.up,collision.contacts[0].normal));

		float cos = Mathf.Abs(Vector3.Dot(collision.contacts[0].normal, colRelVel.normalized));

		if (colRelVel.magnitude * cos >= 5f){

			localVector = transform.InverseTransformDirection(colRelVel) / (50f);

			collisionPos -= localVector * 5f;
			collisionRot = Quaternion.Euler(new Vector3(-localVector.z * 5f, 0f, -localVector.x * 50f));
			cam.fieldOfView = cam.fieldOfView - Mathf.Clamp(collision.relativeVelocity.magnitude, 0f, 30f);
			index = Mathf.Clamp(collision.relativeVelocity.magnitude / 5f, 0f, 10f);

		}

	}

}