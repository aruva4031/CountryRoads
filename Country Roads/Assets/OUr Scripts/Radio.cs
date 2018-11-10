using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : MonoBehaviour
{
	// is radio switch on?
	public bool radioOn;
	public AudioSource radioSound;
	public AudioSource insaneRadio;
	private GameObject camera;

	public AudioClip SanityRadioHosts;
	private bool SanityEvent = false;
	private float SanityStart;
	public AudioClip Song1;
	public AudioClip Song2;
	public float nextTrack;

	// Use this for initializsation
	void Start()
	{
		this.radioOn = false;
		//start playing radio
		radioSound.Play();
		radioSound.volume = 0f;
		camera = GameObject.FindGameObjectWithTag("GameCamera");
	}

	// Update is called once per frame
	void Update()
	{
		/*
        if (radioOn)
        {
            Debug.Log("The radio is ON");
        }
        else if (!radioOn)
        {
            Debug.Log("The radio is OFF");
        }*/

		if (camera.GetComponent<CameraController>().isRadioSeen() && !SanityEvent)
		{
			if (Input.GetButtonDown("XboxA"))
			{
				this.switchRadio();
				//Debug.Log("Radio has been switched");
			}
		}
		else if(SanityEvent && Time.time > SanityStart + insaneRadio.clip.length) {
			insaneRadio.clip = null;
			SanityEvent = false;
		}
	}

	// turn radio on or off
	public void switchRadio()
	{
		// negate whatever starting point the radio was at
		this.radioOn = !this.radioOn;
		if (radioOn)
		{
			radioSound.volume = 1f;
		}
		else
		{
			radioSound.volume = 0f;
		}
	}
	public void switchTrack() {

	}
	public void SanityRadio() {
		if (radioOn) {
			radioOn = !radioOn;
		}
		insaneRadio.clip = SanityRadioHosts;
		insaneRadio.Play ();
		SanityEvent = true;
		SanityStart = Time.time;
		Debug.Log ("Its SPOOKY TIME");
	}
}
