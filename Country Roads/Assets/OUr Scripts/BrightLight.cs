using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrightLight : MonoBehaviour
{
    // rotation towards the camera
    // gradually grows brighter if player is in a certain range
    // comes closer to player?
    // vanishes as soon as everything's white

    // GameObject for Sanity
    public GameObject player;
    public GameObject brightLight;
    public Light actualLight;

    public AudioSource ringingNoise;
    public AudioClip Ringing_Noise_Clip;

    public Vector3 playerPos;
    public Vector3 lightEndPt;
    public Vector3 startPos;

    public float lightSpeed;
    public float step;
    public float startRange;
    public float startIntensity;

    // the official time
    public float timeInSec;

    // need initial and final time; when 5 seconds have passed since the initial time, do the wait then rotation
    public int initalTime;
    public int finalTime;

    // trigger the light
    public bool sanityIsLow;
    public bool isDone;
    public bool isPlaying;

    // Use this for initialization
    void Start()
    {
        this.player = GameObject.FindGameObjectWithTag("Player");
        this.brightLight = GameObject.FindGameObjectWithTag("BrightLight");
        this.actualLight = GetComponent<Light>();

        this.ringingNoise = GetComponent<AudioSource>();
        this.Ringing_Noise_Clip = GetComponent<AudioSource>().clip;

        /** remember that this is a Vector3 */
        this.playerPos = player.transform.position;

        // taken out because the light's starting position is above the player
        //this.lightEndPt = new Vector3(this.playerPos.x, this.playerPos.y + 3.0f, this.playerPos.z + 10.0f);
        this.startPos = new Vector3(this.playerPos.x, this.playerPos.y + 10, this.playerPos.z + 10);
        this.transform.position = new Vector3(this.playerPos.x, this.playerPos.y + 10, this.playerPos.z + 10);

        this.lightSpeed = 10f;
        this.step = 0f;

        this.sanityIsLow = false;
        this.isDone = false;
        this.isPlaying = false;

        this.startRange = 5;
        this.startIntensity = 0;
    }

    public void resetAll()
    {
        this.actualLight.range = 5;
        this.actualLight.intensity = 0;
        this.GetComponent<Renderer>().enabled = false;
        this.sanityIsLow = false;
        this.startPos = new Vector3(this.playerPos.x, this.playerPos.y + 10, this.playerPos.z + 10);
        this.lightEndPt = new Vector3(this.playerPos.x, this.playerPos.y + 1.5f, this.playerPos.z + 5.0f);
        this.transform.position = this.startPos;
        this.GetComponent<Renderer>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        step = lightSpeed * Time.deltaTime;
        this.playerPos = player.transform.position;
        this.lightEndPt = new Vector3(this.playerPos.x, this.playerPos.y + 1.5f, this.playerPos.z + 5.0f);
        this.transform.localRotation = this.player.transform.rotation;

        if (this.sanityIsLow)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, lightEndPt, step);

            // code to trigger brighter light
            actualLight.range += 2f;
            actualLight.intensity += 2f;

            if (isPlaying)
            {
                ringingNoise.PlayOneShot(this.Ringing_Noise_Clip, 0.5f);
                Debug.Log("is ringing noise playing?" + ringingNoise.isPlaying);
                this.isPlaying = false;
            }

            if (isDone)
            {
                // after the length of the audio
                // reset the Light
                ringingNoise.Stop();
                resetAll();
            }
        }
    }
}