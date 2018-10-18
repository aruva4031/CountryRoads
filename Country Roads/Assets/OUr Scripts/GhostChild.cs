using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostChild : MonoBehaviour {

    public Transform positionInCar;
    public GameObject player;
    float speed;
    private bool isHaunting;
    public bool hasSeen;
    public AudioSource ghostSound;
	// Use this for initialization
	void Start () {
        //make ghost transparent
        gameObject.GetComponentInChildren<Renderer>().material.color
            =new Color(gameObject.GetComponentInChildren<Renderer>().material.color.r, gameObject.GetComponentInChildren<Renderer>().material.color.g, gameObject.GetComponentInChildren<Renderer>().material.color.b,0.5f);
        //since ghost children should be cloned: find psoition in car and player on start
        positionInCar = GameObject.Find("PositionInCar").transform;
        player = GameObject.Find("Player");
        //player is not haunting the ghost yet; haunts him when he sits in the car
        isHaunting = false;
        hasSeen = false;
        //set the speed for moving towards player
        speed = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        //if the player is close, move towards it
        if (Vector3.Distance(transform.position, player.transform.position) <= 10&&!hasSeen)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z), speed);
            //cast raycast towards player: if it sees the player, reduce sanity?
            hasSeen = true;
            GameObject.Find("SanityMeter").GetComponentInChildren<SanityMeter>().lowerOnce();
            /*if (Physics.Raycast(transform.position, player.transform.position, 10))
            {
                GameObject.Find("SanityMeter").GetComponentInChildren<SanityMeter>().lowerOnce();
            }*/
        }
        else if(Vector3.Distance(transform.position, player.transform.position) > 10&&hasSeen==true)
        {
            hasSeen = false;
        }
        else if (!isHaunting)
        {
            GameObject.Find("SanityMeter").GetComponentInChildren<SanityMeter>().lowerSanity = false;
        }
        else if (isHaunting)
        {
            transform.position = positionInCar.position;
            if (GameObject.FindWithTag("Radio").GetComponent<Radio>().radioOn)
            {
                ghostSound.volume = 0.5f;
            }
            else
            {
                ghostSound.volume = 1f;
            }
        }
    }

    public void startHaunting()
    {
        StartCoroutine(hauntPlayerInCar());
    }


    public IEnumerator hauntPlayerInCar()
    {
        isHaunting = true;
        ghostSound.Play();
        GameObject.Find("SanityMeter").GetComponentInChildren<SanityMeter>().lowerSanity = true;
        transform.position = positionInCar.position;
        transform.LookAt(GameObject.Find("Player").transform);
        yield return new WaitForSeconds(10);
        GameObject.Find("SanityMeter").GetComponentInChildren<SanityMeter>().lowerSanity = false;
        GameObject.Destroy(this.gameObject);
    }
}
