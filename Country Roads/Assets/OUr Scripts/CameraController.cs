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
    bool isKilled;

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
    }

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

    // Update is called once per frame
    void Update()
    {
        horizontalRS = Input.GetAxis("HorizontalRS") * 90;
        verticalRS = Input.GetAxis("VerticalRS") * 45;

        look = Quaternion.Euler(transform.rotation.x + verticalRS, transform.rotation.y + horizontalRS, transform.rotation.z + 0);
        if (GameObject.FindWithTag("StalkerGhost").GetComponent<StalkerGhostAI>().stopMovement==false)
        {
            // if the player is looking around
            if (this.horizontalRS >= 0.8 || this.horizontalRS <= -0.8 || this.verticalRS >= 0.8 || this.verticalRS <= -0.8)
            {
                // if the player is not looking around
                transform.rotation = Quaternion.Slerp(transform.rotation, look, Time.deltaTime * 3f);
            }
            else
            {
                // if the player is not looking around
                transform.rotation = Quaternion.Slerp(transform.rotation, player.transform.rotation, Time.deltaTime * 3f);
            }
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, player.transform.rotation, Time.deltaTime * 3f);
        }


        //TODO: fixupdate method
        /*
        if (this.horizontalRS >= 0.8)
        {
            this.transform.Rotate(Vector3.down * Time.deltaTime * -60);
            Debug.Log("Go into horizontalRS if");
        }
        else if (this.horizontalRS <= -0.8)
        {
            this.transform.Rotate(Vector3.down * Time.deltaTime * 60);
        }
        else if (this.horizontalRS < 0.8 || this.horizontalRS > -0.8)
        {
            if (this.horizontalRS > 0.15)
            {
                this.transform.Rotate(Vector3.down * Time.deltaTime * -15);
            }
            else if (this.horizontalRS < -0.15)
            {
                this.transform.Rotate(Vector3.down * Time.deltaTime * 15);
            }
            else
            {
               // rotate the camera back to the original position
               this.transform.rotation = Quaternion.Lerp(this.transform.rotation, player.transform.rotation, Time.time * lerpValue);
            }
        }*/
    }
}
