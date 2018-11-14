using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public GameObject radio;
    public Ray ray;
    public Quaternion look;
    public float lerpValue;
    public float horizontalRS;
    public float verticalRS;
    public bool seeRadio;

    // is player dead
    public bool isKilled;

    // the official time
    private float timeInSec;

    // need initial and final time; when 5 seconds have passed since the initial time, do the wait then rotation
    private int initalTime;
    private int finalTime;

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
        RaycastHit hit;
        if (Physics.Raycast(new Ray(transform.position, transform.forward), out hit, 100f))
        {
            if (hit.collider.tag == "Radio")
            {
                Debug.Log("Player sees Radio");
                this.seeRadio = true;
            }
            else
            {
                this.seeRadio = false;
            }
        }

        return this.seeRadio;
    }

    // Alex's code: to see the ghost
    public bool isGhostSeen(string tag)
    {
        RaycastHit hit;
        bool GhostSeen = false;
        if (Physics.Raycast(new Ray(transform.position, transform.forward), out hit, 100f))
        {
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

        horizontalRS = Input.GetAxis("HorizontalRS") * 90;
        verticalRS = Input.GetAxis("VerticalRS") * 45;

        //look = Quaternion.Euler(transform.rotation.x + verticalRS, transform.rotation.y + horizontalRS, transform.rotation.z + 0);
        look = Quaternion.Euler(transform.localRotation.x + verticalRS, transform.localRotation.y + horizontalRS, transform.localRotation.z + 0);
        if (GameObject.FindWithTag("StalkerGhost").GetComponent<StalkerGhostAI>().stopMovement == false)
        {
            // if the player is looking around
            if (this.horizontalRS >= 0.8 || this.horizontalRS <= -0.8 || this.verticalRS >= 0.8 || this.verticalRS <= -0.8)
            {
                // if the player is not looking around
                //  transform.rotation = Quaternion.Slerp(transform.rotation, look, Time.deltaTime * 3f);
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

                // Debug.Log("## Initial Time is: " + this.initalTime);
                // Debug.Log("## Final Time is: " + this.finalTime);

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