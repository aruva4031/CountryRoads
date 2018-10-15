using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostChild : MonoBehaviour {

    public Transform positionInCar;
    public GameObject player;
    float speed;
    private bool isHaunting;
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
        //set the speed for moving towards player
        speed = 0.01f;
    }

    // Update is called once per frame
    void Update()
    {
        //if the player is close, move towards it
        if (Vector3.Distance(transform.position, player.transform.position) < 10)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z), speed);
            //cast raycast towards player: if it sees the player, reduce sanity?
            if (Physics.Raycast(transform.position, player.transform.position, 10))
            {
                GameObject.Find("SanityMeter").GetComponentInChildren<SanityMeter>().sanityLowerCall();
            }
        }
        else if(!isHaunting)
        {
            GameObject.Find("SanityMeter").GetComponentInChildren<SanityMeter>().lowerSanity = false;
        }
    }

    public void startHaunting()
    {
        StartCoroutine(hauntPlayerInCar());
    }

    public IEnumerator hauntPlayerInCar()
    {
        isHaunting = true;
        transform.position = positionInCar.position;
        yield return new WaitForSeconds(10);
        GameObject.Find("SanityMeter").GetComponentInChildren<SanityMeter>().lowerSanity = false;
        GameObject.Destroy(this);
    }
}
