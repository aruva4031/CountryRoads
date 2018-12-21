/**
 *  Title:       BrightLight.cs
 *  Description: Sanity Event that includes a bright orb of light appearing in front of the player. Only occurs if the sanity value is below 45.
 *               BrightLight is used in the SanityMeter script, where its values are also changed.
 */
/**
*	Outcome addressed: Physics-based lighting, where the bright light gradually brightens up all the area around it.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrightLight : MonoBehaviour
{
    // GameObjects for Player and brightlight itself
    public GameObject player;

    // the actual light object attached to the game object
    public Light actualLight;

    // audio attached to BrightLight game object
    public AudioSource ringingNoise;
    public AudioClip Ringing_Noise_Clip;

    // vector positions of player, where the light will appear, and where it starts from
    public Vector3 playerPos;
    public Vector3 lightEndPt;
    public Vector3 startPos;

    // floats for speed of the light orb,
    // steps in time based on lightSpeed,
    // starting range and intensity of light
    public float lightSpeed;
    public float step;
    public float startRange;
    public float startIntensity;

    // bools to trigger the light, say when bright light event is done, and when ringing noise audio is playing
    public bool sanityIsLow;
    public bool isDone;
    public bool isPlaying;

    // Use this for initialization
    void Start()
    {
        // find player game object
        this.player = GameObject.FindGameObjectWithTag("Player");

        // get actual light
        this.actualLight = GetComponent<Light>();

        // the ringing noise playing
        this.ringingNoise = GetComponent<AudioSource>();
        this.Ringing_Noise_Clip = GetComponent<AudioSource>().clip;

        // Vector3 position of the player
        this.playerPos = player.transform.position;

        // set starting and current position of light above player
        this.startPos = new Vector3(this.playerPos.x, this.playerPos.y + 10, this.playerPos.z + 10);
        this.transform.position = new Vector3(this.playerPos.x, this.playerPos.y + 10, this.playerPos.z + 10);

        // set the initial speed of the light
        this.lightSpeed = 10f;
        this.step = 0f;

        // initialize bools as false
        this.sanityIsLow = false;
        this.isDone = false;
        this.isPlaying = false;

        // set starting values of the light
        this.startRange = 5;
        this.startIntensity = 0;
    }

    // Function to reset the needed positions and values of the light
    public void resetAll()
    {
        this.actualLight.range = 5;
        this.actualLight.intensity = 0;
        this.GetComponent<Renderer>().enabled = false;
        this.sanityIsLow = false;
        this.startPos = new Vector3(this.playerPos.x, this.playerPos.y + 10, this.playerPos.z + 10);
        this.lightEndPt = new Vector3(this.playerPos.x, this.playerPos.y + 1.5f, this.playerPos.z + 5.0f);
        this.transform.position = this.startPos;

        // make light mesh reappear
        this.GetComponent<Renderer>().enabled = true;
    }

    void Update()
    {
        // set speed of light based on time
        step = lightSpeed * Time.deltaTime;

        // update player and light end positions; also rotate light w/ player
        this.playerPos = player.transform.position;
        this.lightEndPt = new Vector3(this.playerPos.x, this.playerPos.y + 1.5f, this.playerPos.z + 5.0f);
        //this.transform.localRotation = this.player.transform.rotation;

        // if sanity is low, enter if statement
        if (this.sanityIsLow)
        {
            // start moving towards set light end point
            this.transform.position = Vector3.MoveTowards(this.transform.position, lightEndPt, step);

            // brighten light by increasing light values
            actualLight.range += 2f;
            actualLight.intensity += 2f;

            // if the ringing noise is playing, enter if-statement
            if (isPlaying)
            {
                ringingNoise.PlayOneShot(this.Ringing_Noise_Clip, 0.5f);
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