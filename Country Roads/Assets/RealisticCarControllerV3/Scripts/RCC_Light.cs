using UnityEngine;
using System.Collections;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Light/Light")]
public class RCC_Light : MonoBehaviour {

	private RCC_CarControllerV3 carController;
	private Light _light;
	private Projector projector;

	public LightType lightType;
	public enum LightType{HeadLight, BrakeLight, ReverseLight, Indicator};

	// For Indicators.
	private RCC_CarControllerV3.IndicatorsOn indicatorsOn;
	private AudioSource indicatorSound;
	public AudioClip indicatorClip{get{return RCC_Settings.Instance.indicatorClip;}}

	void Start () {
		
		carController = GetComponentInParent<RCC_CarControllerV3>();
		_light = GetComponent<Light>();
		_light.enabled = true;

		if(RCC_Settings.Instance.useLightProjectorForLightingEffect){
			
			projector = GetComponent<Projector>();
			if(projector == null){
				projector = ((GameObject)Instantiate(RCC_Settings.Instance.projector, transform.position, transform.rotation)).GetComponent<Projector>();
				projector.transform.SetParent(transform, true);
			}
			projector.ignoreLayers = RCC_Settings.Instance.projectorIgnoreLayer;
			if(lightType != LightType.HeadLight)
				projector.transform.localRotation = Quaternion.Euler(20f, transform.localPosition.z > 0f ? 0f : 180f, 0f);
			Material newMaterial = new Material(projector.material);
			projector.material = newMaterial ;

		}

		if(RCC_Settings.Instance.useLightsAsVertexLights){
			_light.renderMode = LightRenderMode.ForceVertex;
			_light.cullingMask = 0;
		}else{
			_light.renderMode = LightRenderMode.ForcePixel;
		}

		if(lightType == LightType.Indicator){
			
			if(!carController.transform.Find("All Audio Sources/Indicator Sound AudioSource"))
				indicatorSound = RCC_CreateAudioSource.NewAudioSource(carController.gameObject, "Indicator Sound AudioSource", 3, 10, 1, indicatorClip, false, false, false);
			else
				indicatorSound = carController.transform.Find("All Audio Sources/Indicator Sound AudioSource").GetComponent<AudioSource>();
			
		}

	}

	void Update () {

		if(RCC_Settings.Instance.useLightProjectorForLightingEffect)
			Projectors();

		switch(lightType){

		case LightType.HeadLight:
			if(!carController.lowBeamHeadLightsOn && !carController.highBeamHeadLightsOn)
				Lighting(0f);
			if(carController.lowBeamHeadLightsOn && !carController.highBeamHeadLightsOn){
				Lighting(.6f, 50f, 90f);
				transform.localEulerAngles = new Vector3(10f, 0f, 0f);
			}else if(carController.highBeamHeadLightsOn){
				Lighting(1f, 200f, 45f);
				transform.localEulerAngles = new Vector3(0f, 0f, 0f);
			}
			break;

		case LightType.BrakeLight:
			Lighting((!carController.lowBeamHeadLightsOn ? Mathf.Clamp01(carController._brakeInput * 10f)  : Mathf.Clamp(carController._brakeInput * 10f, .3f, 1f)));
			break;

		case LightType.ReverseLight:
			Lighting(carController.direction == -1 ? 1f : 0f);
			break;

		case LightType.Indicator:
			indicatorsOn = carController.indicatorsOn;
			Indicators();
			break;

		}
		
	}

	void Lighting(float input){

		_light.intensity = input;

	}

	void Lighting(float input, float range, float spotAngle){

		_light.intensity = input;
		_light.range = range;
		_light.spotAngle = spotAngle;

	}

	void Indicators(){

		switch(indicatorsOn){

		case RCC_CarControllerV3.IndicatorsOn.Left:

			if(transform.localPosition.x > 0f){
				_light.intensity = 0f;
				break;
			}

			if(carController.indicatorTimer >= .5f){
				_light.intensity = 0f;
				if(indicatorSound.isPlaying)
					indicatorSound.Stop();
			}else{
				_light.intensity = 1f; 
				if(!indicatorSound.isPlaying && carController.indicatorTimer <= .05f)
					indicatorSound.Play();
			}
			if(carController.indicatorTimer >= 1f)
				carController.indicatorTimer = 0f;
			break;

		case RCC_CarControllerV3.IndicatorsOn.Right:

			if(transform.localPosition.x < 0f){
				_light.intensity = 0f;
				break;
			}

			if(carController.indicatorTimer >= .5f){
				_light.intensity = 0f;
			if(indicatorSound.isPlaying)
				indicatorSound.Stop();
			}else{
				_light.intensity = 1f;
				if(!indicatorSound.isPlaying && carController.indicatorTimer <= .05f)
					indicatorSound.Play();
			}
			if(carController.indicatorTimer >= 1f)
				carController.indicatorTimer = 0f;
			break;

		case RCC_CarControllerV3.IndicatorsOn.All:
			
			if(carController.indicatorTimer >= .5f){
				_light.intensity = 0f;
				if(indicatorSound.isPlaying)
					indicatorSound.Stop();
			}else{
				_light.intensity = 1f;
				if(!indicatorSound.isPlaying && carController.indicatorTimer <= .05f)
					indicatorSound.Play();
			}
			if(carController.indicatorTimer >= 1f)
				carController.indicatorTimer = 0f;
			break;

		case RCC_CarControllerV3.IndicatorsOn.Off:
			
			_light.intensity = 0f;
			carController.indicatorTimer = 0f;
			break;
			
		}

	}

	private void Projectors(){

		if(!_light.enabled){
			projector.enabled = false;
			return;
		}else{
			projector.enabled = true;
		}

		projector.material.color = _light.color * _light.intensity;

		projector.farClipPlane = Mathf.Lerp(10f, 40f, (_light.range - 50) / 150f);
		projector.fieldOfView = Mathf.Lerp(40f, 30f, (_light.range - 50) / 150f);

	}
		
}
