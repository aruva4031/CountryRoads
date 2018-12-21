/**
 *  Title:       GhostChild.cs
 *  Description: Script for the ghost child AI, subtracting sanity from the player if it sees the ghost and haunt the player in the car if 
 *  player and ghost collide
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostChild : MonoBehaviour
{

    public Transform positionInCar;     //the position the ghost should have when getting in the car with the player
    public GameObject player;           //to hold the player for reference
    float speed;                        //to hold the speed of the ghost when moving towards the player
    private bool isHaunting;            //to determine whether the ghost is currently haunting the player
    public bool hasSeen;                //to determine whether the player has seen the ghost
    public AudioSource ghostSound;      //to store the audio source of the ghost
    public GameObject radio;            //to store the radio
    public int SanityLowerOnceAmount;   //to determine the amount of how much the sanity is lowered

    // Use this for initialization
    void Start()
    {
        //since ghost children should be cloned: find psoition in car and player on start
        positionInCar = GameObject.Find("PositionInCar").transform;
        //find the player
        player = GameObject.Find("Player");
        //player is not haunting the ghost yet; haunts him when he sits in the car
        isHaunting = false;
        hasSeen = false;
        //set the speed for moving towards player
        speed = 0.1f;
        //find the ghost's audio source
        ghostSound = GetComponent<AudioSource>();
        //find the radio
        radio = GameObject.Find("Radio");
    }

    void Awake()
    {
        //since ghost children should be cloned: find psoition in car and player on start
        positionInCar = GameObject.Find("PositionInCar").transform;
        //find the player
        player = GameObject.FindGameObjectWithTag("Player");
        //player is not haunting the ghost yet; haunts him when he sits in the car
        isHaunting = false;
        hasSeen = false;
        //set the speed for moving towards player
        speed = 0.1f;
        //find the ghost's audio source
        ghostSound = GetComponent<AudioSource>();
        //find the radio
        radio = GameObject.Find("Radio");
    }

    // Update is called once per frame
    void Update()
    {
        //if the player is close, move towards it
        if (Vector3.Distance(transform.position, player.transform.position) <= 10 && !hasSeen)
        {
            //create a step to move by
            float step = speed * Time.deltaTime;
            //use the Vector3.moveTowards function to move towards the player
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z), speed);
            //ghost has seen the player now
            hasSeen = true;
            //lower the sanity once by a small amount
            GameObject.Find("SanityMeter").GetComponentInChildren<SanityMeter>().lowerOnce(SanityLowerOnceAmount);
        }
        //if the player is far enough away from the ghost, the player gets out of the ghost's sight
        else if (Vector3.Distance(transform.position, player.transform.position) > 10 && hasSeen == true)
        {
            hasSeen = false;
        }
        //if the player is not hauting the ghost but is in sight, lower the sanity by a slow amount once
        else if (!isHaunting)
        {
            GameObject.Find("SanityMeter").GetComponentInChildren<SanityMeter>().lowerSanity = false;
        }
        //if the player is haunting the ghost
        else if (isHaunting)
        {
            //set the ghost position to the current position in the ghost car
            transform.position = positionInCar.position;
            transform.rotation = positionInCar.rotation;
            //if the radio is on, the ghost's volume is lower and he talks less loud
            if (radio.GetComponent<Radio>().radioOn)
            {
                ghostSound.volume = 0.2f;
            }
            //if the radio is off, set the ghost's volume to full volume
            else if (!radio.GetComponent<Radio>().radioOn)
            {
                ghostSound.volume = 1f;
            }
        }
    }

    //function to start haunting the player: start the coroutine to haunt the player in the car
    public void startHaunting()
    {
        StartCoroutine(hauntPlayerInCar());
    }

    //function to haunt the player in the car
    public IEnumerator hauntPlayerInCar()
    {
        //set the isHaunting boolean to true to determine that the coroutine is running
        isHaunting = true;
        //set the lowering sanity boolean of the sanity meter to true
        GameObject.Find("SanityMeter").GetComponentInChildren<SanityMeter>().lowerSanity = true;
        //play the ghost's dialogue
        ghostSound.Play();
        //set the ghost position to the current position in the ghost car
        transform.position = positionInCar.position;
        transform.rotation = positionInCar.rotation;
        //wait for 10 seconds
        yield return new WaitForSeconds(10);
        //set the lowering sanity boolean of the sanity meter to false
        GameObject.Find("SanityMeter").GetComponentInChildren<SanityMeter>().lowerSanity = false;
        //create a new event
        StartCoroutine(newEvent());
        //destroy the gameObject's parent to destroy the complete game object
        GameObject.Destroy(this.gameObject.transform.parent.gameObject);
    }

    //create a new event in place of the playe
    IEnumerator newEvent()
    {
        //wait for 15 seconds
        yield return new WaitForSeconds(15f);
        //generate a new event at the index of the ghost child, which the observeableManager component has stored
        gameObject.transform.parent.GetComponent<ObserveableManager>().eventGenerator.generateSingleEvent(gameObject.transform.parent.GetComponent<ObserveableManager>().index);
    }
}
