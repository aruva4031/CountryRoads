/**
 *  Title:       CameraController.cs
 *  Description: Used to control the player's camera via a GameObject.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // game objects for player and radio
    public GameObject player;
    public GameObject radio;

    // ray to use for looking for the radio or a ghost
    public Ray ray;

    // quaternion used for looking around
    public Quaternion look;

    // lerp value used to determine how fast the camera should rotate to default,
    // the horizontalRS and verticalRS for reading the analog sticks
    public float lerpValue;
    public float horizontalRS;
    public float verticalRS;

    // the official time in seconds, 
    // initial and final time;
    // when 5 seconds have passed since the initial time, do the wait then rotation
    private float timeInSec;
    private int initalTime;
    private int finalTime;

    // is player dead
    public bool seeRadio;
    public bool isKilled;

    // Use this for initialization
    void Start()
    {
        //this.player = GameObject.FindGameObjectWithTag("Player");
        this.radio = GameObject.FindGameObjectWithTag("Radio");
        this.ray = new Ray(transform.position, transform.forward);
        this.transform.rotation = player.transform.rotation;
        this.lerpValue = 0.02F;
        this.horizontalRS = Input.GetAxis("HorizontalRS");
        this.verticalRS = Input.GetAxis("VerticalRS");
        this.seeRadio = false;
        this.isKilled = false;

        // initialize the seconds to 0
        this.timeInSec = 0f;
        this.initalTime = 0;
        this.finalTime = 0;
    }

    // code to see the radio
    public bool isRadioSeen()
    {
        // raycast used to "hit" colliders of game objects
        RaycastHit hit;

        // if-statement to check if the Raycast is hitting a collider
        if (Physics.Raycast(new Ray(transform.position, transform.forward), out hit, 100f))
        {
            // if-else statement to check if Raycast sees a radio
            if (hit.collider.tag == "Radio") this.seeRadio = true;
            else this.seeRadio = false;
        }
        return this.seeRadio;
    }

    // Alex's code: to see the ghost
    public bool isGhostSeen(string tag)
    {
        // Raycast for "hitting" colliders
        RaycastHit hit;

        // bool to check if ghost is seen
        bool GhostSeen = false;

        // if-statement to see if raycast is hitting a collider
        if (Physics.Raycast(new Ray(transform.position, transform.forward), out hit, 100f))
        {
            // if the raycast is hitting
            if (hit.collider.tag == tag)
            {
                GhostSeen = true;
            }
            else
            {
                GhostSeen = false;
            }
        }
        return GhostSeen;
    }

    // Update is called once per frame
    void Update()
    {
        // set the timeInSec here
        this.timeInSec = timeInSec + Time.deltaTime;

        // set the limits of the player's/camera's rotation, based on which direction they're looking in
        horizontalRS = Input.GetAxis("HorizontalRS") * 80;
        verticalRS = Input.GetAxis("VerticalRS") * 30;

        // give the Euler angles of where to look at
        look = Quaternion.Euler(transform.localRotation.x + verticalRS, transform.localRotation.y + horizontalRS, transform.localRotation.z + 0);

        // if-statement for finding the StalkerGhost, which will force the camera to look towards him for the death scene
        if (GameObject.FindWithTag("StalkerGhost").GetComponent<StalkerGhostAI>().stopMovement == false)
        {
            // if the player is looking around
            if (this.horizontalRS >= 0.8 || this.horizontalRS <= -0.8 || this.verticalRS >= 0.8 || this.verticalRS <= -0.8)
            {
                // if the player is not looking around
                transform.localRotation = Quaternion.Slerp(transform.localRotation, look, Time.deltaTime * 3f);

                // set the initial time by the seconds
                this.timeInSec = this.timeInSec + Time.deltaTime;
                this.initalTime = (int)(this.timeInSec);
                // give finalTime the initalTime 5 seconds in advance
                this.finalTime = this.initalTime + 5;
            }
            else
            {
                this.initalTime = (int)(this.timeInSec);

                if (this.initalTime == this.finalTime)
                {
                    // if the player is not looking around
                    transform.rotation = Quaternion.Slerp(transform.rotation, player.transform.rotation, Time.deltaTime * 3f);
                }
            }
        }
        else
        {
            // the Stalker ghost has stopped moving
            transform.rotation = Quaternion.Slerp(transform.rotation, player.transform.rotation, Time.deltaTime * 3f);
        }
    }
}