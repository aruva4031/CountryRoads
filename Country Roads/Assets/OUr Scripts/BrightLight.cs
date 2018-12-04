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
    public bool triggerLight;
    public bool sanityIsLow;
    public bool isDone;

    // Use this for initialization
    void Start()
    {
        this.player = GameObject.FindGameObjectWithTag("Player");
        this.brightLight = GameObject.FindGameObjectWithTag("BrightLight");
        this.actualLight = GetComponent<Light>();

        /** remember that this is a Vector3 */
        this.playerPos = player.transform.position;

        // taken out because the light's starting position is above the player
        //this.lightEndPt = new Vector3(this.playerPos.x, this.playerPos.y + 3.0f, this.playerPos.z + 10.0f);
        this.startPos = new Vector3(this.playerPos.x, this.playerPos.y + 10, this.playerPos.z + -10);
        this.transform.position = new Vector3(this.playerPos.x, this.playerPos.y + 10, this.playerPos.z + -10);

        this.lightSpeed = 10f;
        this.step = 0f;
        this.triggerLight = false;
        this.sanityIsLow = false;
        this.isDone = false;

        this.startRange = 5;
        this.startIntensity = 0;
    }

    public void resetPos()
    {
        this.startPos = new Vector3(this.playerPos.x, this.playerPos.y + 10, this.playerPos.z + -10);
        this.transform.position = this.startPos;
        this.GetComponent<Renderer>().enabled = true;
        this.triggerLight = false;
    }

    // Update is called once per frame
    void Update()
    {
        step = lightSpeed * Time.deltaTime;
        this.playerPos = player.transform.position;
        this.lightEndPt = new Vector3(this.playerPos.x, this.playerPos.y + 1.5f, this.playerPos.z + 5.0f);

        if (!triggerLight)
        {
            //Debug.Log("distance is: " + (this.transform.position.magnitude - this.playerPos.magnitude));

            /*
            // if the light is close enough to the player to start getting brighter
            if ((this.transform.position.magnitude - this.playerPos.magnitude) <= 5)
            {
                Debug.Log("Near player's position");
                triggerLight = true;
            }*/
            // check for sanity level


            if (sanityIsLow)
            {
                this.triggerLight = true;
            }
        }
        else
        {
            Debug.Log("Entered else");
            //+ Vector3.RotateTowards(this.transform.position, this.playerPos, 1, 1)
            this.transform.position = Vector3.MoveTowards(this.transform.position, lightEndPt, step);

            // code to trigger brighter light
            Debug.Log("AHHH IT'S TOO BRIGHT");
            Debug.Log("Light's pos: " + this.transform.position);
            Debug.Log("Player's pos: " + this.playerPos);

            actualLight.range += 2f;
            actualLight.intensity += 2f;

            if (isDone)
            {
                // after the length of the audio
                this.GetComponent<Renderer>().enabled = false;
                triggerLight = false;
                sanityIsLow = false;
                this.actualLight.range = 5;
                this.actualLight.intensity = 0;
                // reset position
                resetPos();
            }

        }
    }
}