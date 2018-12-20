/**
 *  Title:       Radio.cs
 *  Description: Used to turn the radio on, listen to country music, and trigger both sanity and supernatural events.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : MonoBehaviour
{
    // game object of the game cameraS
    private GameObject camera;

    // audio source for radio; 
    // includes normal radio song,
    // insane radio hosts,
    // and optional songs
    public AudioSource radioSound;
    public AudioSource insaneRadio;
    public AudioClip SanityRadioHosts;
    public AudioClip Song1;
    public AudioClip Song2;

    // float for starting time of radio sanity event,
    // value of nextTrack(?)
    private float SanityStart;
    public float nextTrack;

    // bool to see if radio is on,
    // if car is one (initially set to true),
    // and if sanity event is playing (initially set to false)
    public bool radioOn;
    public bool carOn = true;
    private bool SanityEvent = false;

    // Use this for initializsation
    void Start()
    {
        // initally set radio to off
        this.radioOn = false;

        //start playing radio
        radioSound.clip = Song1;
        radioSound.Play();
        radioSound.volume = 0f;

        // find game object of game camera
        camera = GameObject.FindGameObjectWithTag("GameCamera");
    }

    // Update is called once per frame
    void Update()
    {
        // if the camera sees the radio and no sanity event is playing
        if (camera.GetComponent<CameraController>().isRadioSeen() && !SanityEvent)
        {
            // if player presses Xbox A button and the car is on
            if (Input.GetButtonDown("XboxA") && carOn)
            {
                // switch radio on
                this.switchRadio();
                Debug.Log("Radio has been switched");
            }
        }   // else if there is a sanity event playing and starting time is greater than length of the sanity event
        else if (SanityEvent && Time.time > SanityStart + insaneRadio.clip.length)
        {
            // there's no insaneRadio and no sanity event 
            insaneRadio.clip = null;
            SanityEvent = false;
        }   // else if the car is off
        else if (!carOn)
        {   // turn radio off
            offRadio();
        }
    }

    // turn radio on or off
    public void switchRadio()
    {
        // negate whatever starting point the radio was at
        this.radioOn = !this.radioOn;
        // if radio is on, volume is up
        if (radioOn)
        {
            radioSound.volume = 1f;
        }   // else volume is off
        else
        {
            radioSound.volume = 0f;
        }
    }

    // turn radio off
    private void offRadio()
    {
        radioOn = false;
        radioSound.volume = 0f;
    }

    // switch between tracks (unused)
    public void switchTrack()
    {

    }

    // if a sanity event is playing, and radio has been picked, use this
    public void SanityRadio()
    {
        // if radio on, turn radio off
        if (radioOn)
        {
            radioOn = !radioOn;
        }

        // play insane radio hosts clip for sanity event
        insaneRadio.clip = SanityRadioHosts;
        insaneRadio.Play();
        SanityEvent = true;
        SanityStart = Time.time;
        Debug.Log("Its SPOOKY TIME");
    }
}
