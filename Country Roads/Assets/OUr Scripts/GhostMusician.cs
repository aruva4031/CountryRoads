/**
 *  Title:       GhostMusician.cs
 *  Description: This script is used to model the ghost musician. If you have the radio on, he will get into the car and play his own music
 *  as well as decreasing your sanity by a large amount
 *  Outcome addressed: Model behaviour of non-player characters/machine learning
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMusician : MonoBehaviour {

    public float inRange;                   //to determine if the player is close enough so the radio can be heard
    public AudioSource musicianSource;      //get the audio source of the ghost musician
    public AudioClip textClip;              //to store the text clip for the musician
    public AudioClip musicClip;             //to store the music clip for the musician
    public SanityMeter sm;                  //to store the sanity meter, used for sanity lowering
    public bool coroutine_running;          //to determine if the coroutine is running
    public bool pose_switched=false;        //to determine if the pose has been switched and the player is in the car
    public GameObject pose2;                //to store the pose for the musician in the car
    public GameObject carPosition;          //to store the position the ghost musician will have in the car
    public Radio radio;                     //to access the radio

    // Use this for initialization
    void Start () {
        //the coroutine is not currently running
        coroutine_running = false;
        //find the game objects needed in the scene and the PublicObjects script
        sm = GameObject.FindWithTag("SanityHandler").GetComponent<SanityMeter>();
        textClip = GetComponent<RandomAudioClip>().getRandomClip(GetComponent<RandomAudioClip>().soundClips);
        pose2 = GameObject.Find("PublicObjects").GetComponent<PublicObjects>().musicianPose2;
        carPosition = GameObject.Find("PublicObjects").GetComponent<PublicObjects>().carPosition2;
        radio = GameObject.Find("PublicObjects").GetComponent<PublicObjects>().radio.GetComponent<Radio>();
    }

    void Awake()
    {
        //do the same things as in Start() to apply it when the objects get activated
        sm = GameObject.FindWithTag("SanityHandler").GetComponent<SanityMeter>();
        coroutine_running = false;
        textClip = GetComponent<RandomAudioClip>().getRandomClip(GetComponent<RandomAudioClip>().soundClips);
        radio = GameObject.FindGameObjectWithTag("Radio").GetComponent<Radio>();
        pose2 = GameObject.Find("PublicObjects").GetComponent<PublicObjects>().musicianPose2;
        carPosition = GameObject.Find("PublicObjects").GetComponent<PublicObjects>().carPosition2;
    }

    // Update is called once per frame
    void Update () {
        //if the player is in range and the radio is on, but the musician is not in the car yet
		if ((Vector3.Distance(transform.position,new Vector3(radio.transform.position.x,transform.position.y, radio.transform.position.z)) <= inRange) && radio.GetComponent<Radio>().radioOn&&!pose_switched)
        {
            //change for the observeable manager that the musician has been heard
            transform.parent.GetComponent<ObserveableManager>().musicianHeard = true;
            //switch the pose
            poseSwitch();
        }
        //if the misician is in the car and the coroutine is not running and the musician clip is the text clip
        if (pose_switched&&!coroutine_running && musicianSource.clip == textClip)
        {
            //change the position to the current car position of the ghost musician
            this.transform.parent.transform.position = carPosition.transform.position;
            this.transform.parent.transform.rotation = carPosition.transform.rotation;
            //start the car haunt of the player
            StartCoroutine(musicianCarHaunt());
        }
        //if the misician is in the car and the coroutine is running
        if (pose_switched && coroutine_running){
            //change the position to the current car position of the ghost musician
            this.transform.parent.transform.position = carPosition.transform.position;
            this.transform.parent.transform.rotation = carPosition.transform.rotation;
        }
        //if the coroutine is running and the text clip has been played
        if (coroutine_running && musicianSource.clip == textClip && !musicianSource.isPlaying)
        {
            //change the aduio source clip to the music clip and play the music clip in a loop
            musicianSource.clip = musicClip;
            musicianSource.loop = true;
            //turn off the radio
            radio.radioOn = false;
            musicianSource.Play();
        }
	}

    //function to switch the pose of the musician to the pose in the car
    void poseSwitch()
    {
        //change the pose 2 position to the current car position of the ghost musician
        pose2.transform.position = carPosition.transform.position;
        pose2.transform.rotation = carPosition.transform.rotation;
        //set the pose2 of the musician to active
        pose2.SetActive(true);
        //set the pose switched to the pose 2 of the musician to true
        pose2.GetComponentInChildren<GhostMusician>().pose_switched = true;
        //change the pose 2 position to the current car position of the ghost musician again to make sure
        pose2.transform.position = carPosition.transform.position;
        pose2.transform.rotation = carPosition.transform.rotation;
        //set the current musician object to false
        this.gameObject.transform.parent.gameObject.SetActive(false);
    }

    //function for the musician to haunt the player while in the car
    IEnumerator musicianCarHaunt()
    {
        //the coroutine is running
        coroutine_running = true;
        //the sanity of the player is lowering
        sm.lowerSanity = true;
        //set the clip of the musician source to the text clip
        musicianSource.clip = textClip;
        //play the text clip
        musicianSource.Play();
        //lower the sanity 10 times by 4.5 per second: 45 in total
        for(int i = 0; i < 10; i++)
        {
            sm.lowerOnce(4.5f);
            yield return new WaitForSeconds(1f);
        }
        //the sanity of the player stopped lowering
        sm.lowerSanity = false;
        //the coroutine is running
        coroutine_running = false;
        //turn the radio back on
        radio.radioOn = true;
        //set the parent gameObject inactive
        this.gameObject.transform.parent.gameObject.SetActive(false);
    }
}
