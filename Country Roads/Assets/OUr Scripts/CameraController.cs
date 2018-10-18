using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraController : MonoBehaviour
{
    public GameObject player;
    public GameObject radio;
    public Ray ray;
    public float lerpValue;
    public float horizontalRS;
    public bool seeRadio;

    // Use this for initialization
    void Start()
    {  
        //this.player = GameObject.FindGameObjectWithTag("Player");
        this.radio = GameObject.FindGameObjectWithTag("Radio");
        this.ray = new Ray(transform.position, transform.forward);
        this.transform.rotation = player.transform.rotation;
        this.lerpValue = 0.04F;
        this.horizontalRS = Input.GetAxis("HorizontalRS");
        this.seeRadio = false;
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
        this.horizontalRS = Input.GetAxis("HorizontalRS");

        Debug.Log(this.horizontalRS);

        //TODO: fixupdate method

        if (this.horizontalRS >= 0.8)
        {
            this.transform.Rotate(Vector3.down * Time.deltaTime * -60);
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
               //this.transform.rotation = Quaternion.Lerp(this.transform.rotation, player.transform.rotation, Time.time * lerpValue);
            }
        }
    }
}
