using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : MonoBehaviour
{
    // is radio switch on?
    public bool radioOn;
    public AudioSource radioSound;
    public GameObject camera;

    // Use this for initialization
    void Start()
    {
        this.radioOn = false;
        //start playing radio
        radioSound = GetComponent<AudioSource>();
        radioSound.Play();
        radioSound.volume = 0f;
        camera = GameObject.FindGameObjectWithTag("Camera");
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

        if (camera.GetComponent<CameraController>().isRadioSeen())
        {
            if (Input.GetButtonDown("XboxA"))
            {
                this.switchRadio();
                Debug.Log("Radio has been switched");
            }
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
}
