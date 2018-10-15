//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2015 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using UnityEditor;
using System.Collections;

public class RCC_EditorWindows : Editor {
	
	[MenuItem("Tools/BoneCracker Games/Realistic Car Controller/Edit Asset Settings", false, -10)]
	public static void OpenAssetSettings(){
		Selection.activeObject =RCC_Settings.Instance;
	}

	[MenuItem("Tools/BoneCracker Games/Realistic Car Controller/Configure Ground Materials", false, -8)]
	public static void OpenGroundMaterialsSettings(){
		Selection.activeObject =RCC_GroundMaterials.Instance;
	}

	[MenuItem("Tools/BoneCracker Games/Realistic Car Controller/Misc/Add Hood Camera To Vehicle", false, 10)]
	public static void CreateHoodCamera(){

		if(!Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>()){

			EditorUtility.DisplayDialog("Select a vehicle controlled by Realistic Car Controller!", "Select a vehicle controlled by Realistic Car Controller!", "Ok");

		}else{

			if(Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>().gameObject.GetComponentInChildren<RCCHoodCamera>()){
				EditorUtility.DisplayDialog("Your Vehicle Has Hood Camera Already!", "Your vehicle has hood camera already!", "Ok");
				Selection.activeGameObject = Selection.activeGameObject.GetComponentInChildren<RCCHoodCamera>().gameObject;
				return;
			}

			GameObject hoodCam = (GameObject)Instantiate(Resources.Load("RCCAssets/Hood Camera", typeof(GameObject)), Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>().transform.position, Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>().transform.rotation);
			hoodCam.transform.SetParent(Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>().chassis.transform, true);
			hoodCam.GetComponent<ConfigurableJoint>().connectedBody = Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>().gameObject.GetComponent<Rigidbody>();
			RCC_LabelEditor.SetIcon(hoodCam, RCC_LabelEditor.LabelIcon.Purple);
			Selection.activeGameObject = hoodCam;

		}

	}

	[MenuItem("Tools/BoneCracker Games/Realistic Car Controller/Misc/Add Hood Camera To Vehicle", true)]
	public static bool CheckCreateHoodCamera() {
		if(Selection.gameObjects.Length > 1 || !Selection.activeTransform)
			return false;
		else
			return true;
	}

	[MenuItem("Tools/BoneCracker Games/Realistic Car Controller/Misc/Add Wheel Camera To Vehicle", false, 10)]
	public static void CreateWheelCamera(){

		if(!Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>()){

			EditorUtility.DisplayDialog("Select a vehicle controlled by Realistic Car Controller!", "Select a vehicle controlled by Realistic Car Controller!", "Ok");

		}else{

			if(Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>().gameObject.GetComponentInChildren<RCCWheelCamera>()){
				EditorUtility.DisplayDialog("Your Vehicle Has Wheel Camera Already!", "Your vehicle has wheel camera already!", "Ok");
				Selection.activeGameObject = Selection.activeGameObject.GetComponentInChildren<RCCWheelCamera>().gameObject;
				return;
			}

			GameObject wheelCam = new GameObject("Wheel Camera");
			wheelCam.transform.SetParent(Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>().transform, false);
			wheelCam.AddComponent<RCCWheelCamera>();
			RCC_LabelEditor.SetIcon(wheelCam, RCC_LabelEditor.LabelIcon.Purple);
			Selection.activeGameObject = wheelCam;

		}

	}

	[MenuItem("Tools/BoneCracker Games/Realistic Car Controller/Misc/Add Wheel Camera To Vehicle", true)]
	public static bool CheckCreateWheelCamera() {
		if(Selection.gameObjects.Length > 1 || !Selection.activeTransform)
			return false;
		else
			return true;
	}

	[MenuItem("Tools/BoneCracker Games/Realistic Car Controller/Misc/Add Exhaust To Vehicle", false, 10)]
	public static void CreateExhaust(){

		if(!Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>()){

			EditorUtility.DisplayDialog("Select a vehicle controlled by Realistic Car Controller!", "Select a vehicle controlled by Realistic Car Controller!", "Ok");

		}else{

			GameObject exhaustsMain;

			if(!Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>().transform.Find(Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>().chassis.name+"/Exhausts")){
				exhaustsMain = new GameObject("Exhausts");
				exhaustsMain.transform.SetParent(Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>().chassis.transform, false);
			}else{
				exhaustsMain = Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>().transform.Find(Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>().chassis.name+"/Exhausts").gameObject;
			}

			GameObject exhaust = (GameObject)Instantiate(RCC_Settings.Instance.exhaustGas, Selection.activeGameObject.transform.position, Selection.activeGameObject.transform.rotation * Quaternion.Euler(0f, 180f, 0f));
			exhaust.transform.SetParent(exhaustsMain.transform);
			exhaust.transform.localPosition = new Vector3(1f, 0f, -2f);
			RCC_LabelEditor.SetIcon(exhaust, RCC_LabelEditor.Icon.DiamondGray);
			Selection.activeGameObject = exhaust;

		}

	}

	[MenuItem("Tools/BoneCracker Games/Realistic Car Controller/Misc/Add Exhaust To Vehicle", true)]
	public static bool CheckCreateExhaust() {
		if(Selection.gameObjects.Length > 1 || !Selection.activeTransform)
			return false;
		else
			return true;
	}

	[MenuItem("Tools/BoneCracker Games/Realistic Car Controller/Misc/Add Lights To Vehicle/Brake", false, 10)]
	public static void CreateBrakeLight(){

		if(!Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>()){

			EditorUtility.DisplayDialog("Select a vehicle controlled by Realistic Car Controller!", "Select a vehicle controlled by Realistic Car Controller!", "Ok");

		}else{

			GameObject lightsMain;

			if(!Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>().transform.Find(Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>().chassis.name+"/Lights")){
				lightsMain = new GameObject("Lights");
				lightsMain.transform.SetParent(Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>().chassis.transform, false);
			}else{
				lightsMain = Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>().transform.Find(Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>().chassis.name+"/Lights").gameObject;
			}

			GameObject brakeLight = new GameObject("Brake Light");
			brakeLight.AddComponent<Light>();
			brakeLight.GetComponent<Light>().color = Color.red;
			brakeLight.GetComponent<Light>().range = 2f;
			brakeLight.AddComponent<RCC_Light>();
			brakeLight.GetComponent<RCC_Light>().lightType = RCC_Light.LightType.BrakeLight;
			brakeLight.transform.SetParent(lightsMain.transform);
			brakeLight.transform.localRotation = Quaternion.identity;
			brakeLight.transform.localPosition = new Vector3(0f, 0f, -2f);
			RCC_LabelEditor.SetIcon(brakeLight, RCC_LabelEditor.Icon.CircleRed);
			Selection.activeGameObject = brakeLight;

		}

	}

	[MenuItem("Tools/BoneCracker Games/Realistic Car Controller/Misc/Add Lights To Vehicle/Brake", true)]
	public static bool CheckBrakeLight() {
		if(Selection.gameObjects.Length > 1 || !Selection.activeTransform)
			return false;
		else
			return true;
	}

	[MenuItem("Tools/BoneCracker Games/Realistic Car Controller/Misc/Add Lights To Vehicle/Reverse", false, 10)]
	public static void CreateReverseLight(){

		if(!Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>()){

			EditorUtility.DisplayDialog("Select a vehicle controlled by Realistic Car Controller!", "Select a vehicle controlled by Realistic Car Controller!", "Ok");

		}else{

			GameObject lightsMain;

			if(!Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>().transform.Find(Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>().chassis.name+"/Lights")){
				lightsMain = new GameObject("Lights");
				lightsMain.transform.SetParent(Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>().chassis.transform, false);
			}else{
				lightsMain = Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>().transform.Find(Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>().chassis.name+"/Lights").gameObject;
			}

			GameObject reverseLight = new GameObject("Reverse Light");
			reverseLight.AddComponent<Light>();
			reverseLight.GetComponent<Light>().color = Color.white;
			reverseLight.GetComponent<Light>().range = 2f;
			reverseLight.AddComponent<RCC_Light>();
			reverseLight.GetComponent<RCC_Light>().lightType = RCC_Light.LightType.ReverseLight;
			reverseLight.transform.SetParent(lightsMain.transform);
			reverseLight.transform.localRotation = Quaternion.identity;
			reverseLight.transform.localPosition = new Vector3(0f, 0f, -2f);
			RCC_LabelEditor.SetIcon(reverseLight, RCC_LabelEditor.Icon.CircleGray);
			Selection.activeGameObject = reverseLight;

		}

	}

	[MenuItem("Tools/BoneCracker Games/Realistic Car Controller/Misc/Add Lights To Vehicle/Reverse", true)]
	public static bool CheckReverseLight() {
		if(Selection.gameObjects.Length > 1 || !Selection.activeTransform)
			return false;
		else
			return true;
	}

	[MenuItem("Tools/BoneCracker Games/Realistic Car Controller/Misc/Add Lights To Vehicle/HeadLight", false, 10)]
	public static void CreateHeadLight(){

		if(!Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>()){

			EditorUtility.DisplayDialog("Select a vehicle controlled by Realistic Car Controller!", "Select a vehicle controlled by Realistic Car Controller!", "Ok");

		}else{

			GameObject lightsMain;

			if(!Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>().transform.Find(Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>().chassis.name+"/Lights")){
				lightsMain = new GameObject("Lights");
				lightsMain.transform.SetParent(Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>().chassis.transform, false);
			}else{
				lightsMain = Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>().transform.Find(Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>().chassis.name+"/Lights").gameObject;
			}

			GameObject headLight = new GameObject("Head Light");
			headLight.AddComponent<Light>();
			headLight.GetComponent<Light>().color = Color.white;
			headLight.GetComponent<Light>().type = LightType.Spot;
			headLight.GetComponent<Light>().range = 50f;
			headLight.GetComponent<Light>().spotAngle = 90f;
			headLight.AddComponent<RCC_Light>();
			headLight.GetComponent<RCC_Light>().lightType = RCC_Light.LightType.HeadLight;
			headLight.transform.SetParent(lightsMain.transform);
			headLight.transform.localRotation = Quaternion.identity;
			headLight.transform.localPosition = new Vector3(0f, 0f, 2f);
			RCC_LabelEditor.SetIcon(headLight, RCC_LabelEditor.Icon.CircleTeal);
			Selection.activeGameObject = headLight;

		}

	}

	[MenuItem("Tools/BoneCracker Games/Realistic Car Controller/Misc/Add Lights To Vehicle/HeadLight", true)]
	public static bool CheckHeadLight() {
		if(Selection.gameObjects.Length > 1 || !Selection.activeTransform)
			return false;
		else
			return true;
	}

	[MenuItem("Tools/BoneCracker Games/Realistic Car Controller/Misc/Add Lights To Vehicle/Indicator", false, 10)]
	public static void CreateIndicatorLight(){

		if(!Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>()){

			EditorUtility.DisplayDialog("Select a vehicle controlled by Realistic Car Controller!", "Select a vehicle controlled by Realistic Car Controller!", "Ok");

		}else{

			GameObject lightsMain;

			if(!Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>().transform.Find(Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>().chassis.name+"/Lights")){
				lightsMain = new GameObject("Lights");
				lightsMain.transform.SetParent(Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>().chassis.transform, false);
			}else{
				lightsMain = Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>().transform.Find(Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>().chassis.name+"/Lights").gameObject;
			}

			GameObject indicatorLight = new GameObject("Indicator Light");
			indicatorLight.AddComponent<Light>();
			indicatorLight.GetComponent<Light>().color = new Color(1f, .5f, 0f);
			indicatorLight.GetComponent<Light>().range = 2f;
			indicatorLight.AddComponent<RCC_Light>();
			indicatorLight.GetComponent<RCC_Light>().lightType = RCC_Light.LightType.Indicator;
			indicatorLight.transform.SetParent(lightsMain.transform);
			indicatorLight.transform.localRotation = Quaternion.identity;
			indicatorLight.transform.localPosition = new Vector3(0f, 0f, -2f);
			RCC_LabelEditor.SetIcon(indicatorLight, RCC_LabelEditor.Icon.CircleOrange);
			Selection.activeGameObject = indicatorLight;

		}

	}

	[MenuItem("Tools/BoneCracker Games/Realistic Car Controller/Misc/Add Lights To Vehicle/Indicator", true)]
	public static bool CheckIndicatorLight() {
		if(Selection.gameObjects.Length > 1 || !Selection.activeTransform)
			return false;
		else
			return true;
	}
	
}
