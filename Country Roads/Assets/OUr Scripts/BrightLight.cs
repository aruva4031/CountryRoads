using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrightLight : MonoBehaviour {
    // rotation towards the camera
    // gradually grows brighter if player is in a certain range
    // comes closer to player?
    // vanishes as soon as everything's white
    public GameObject player;
    public GameObject brightLight;
    public Light actualLight;
    public Vector3 playerPos;
    public Vector3 lightEndPt;

    // intensity of the light
    //public float range;

    // distance from player
    //public float distance;

    public float lightSpeed;
    public float step;

    // trigger the light
    public bool triggerLight;

	// Use this for initialization
	void Start () {
        this.player = GameObject.FindGameObjectWithTag("Player");
        this.brightLight = GameObject.FindGameObjectWithTag("BrightLight");
        this.actualLight = GetComponent<Light>();

        /** remember that this is a Vector3 */
        this.playerPos = player.transform.position;
        this.lightEndPt = new Vector3(this.playerPos.x, this.playerPos.y + 3.0f, this.playerPos.z + 10.0f);

        this.lightSpeed = 10f;
        this.step = 0f;
        this.triggerLight = false;
    }
	
	// Update is called once per frame
	void Update () {
        step = lightSpeed * Time.deltaTime;
        this.playerPos = player.transform.position;
        this.lightEndPt = new Vector3(this.playerPos.x, this.playerPos.y + 1.5f, this.playerPos.z + 5.0f);


        if (!triggerLight)
        {
            // move this sphere's position towards the player
            this.transform.position = Vector3.MoveTowards(this.transform.position, lightEndPt, step);

            Debug.Log("distance is: " + (this.transform.position.magnitude - this.playerPos.magnitude));

            // if the light is close enough to the player to start getting brighter
            if ((this.transform.position.magnitude - this.playerPos.magnitude) <= 5)
            {
                Debug.Log("Near player's position");
                triggerLight = true;
            }
        }
        else
        {
            Debug.Log("Entered else");

            this.transform.position = Vector3.MoveTowards(this.transform.position, lightEndPt, step);

            // code to trigger brighter light
            Debug.Log("AHHH IT'S TOO BRIGHT");
            Debug.Log("Light's pos: " + this.transform.position);
            Debug.Log("Player's pos: " + this.playerPos);

            actualLight.range += 4f;
            actualLight.intensity += 4f;

            GameObject.DestroyObject(brightLight, 5f);
        }
    }
}