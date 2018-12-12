using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMusician : MonoBehaviour {

    public float speed;
    public float inRange;
    public AudioSource musicianSource;
    public AudioClip textClip;
    public AudioClip musicClip;
    public SanityMeter sm;
    public bool coroutine_running;
    public bool pose_switched;
    public GameObject pose2;
    public GameObject carPosition;
    public Radio radio;

    // Use this for initialization
    void Start () {
        sm = GameObject.FindWithTag("SanityHandler").GetComponentInChildren<SanityMeter>();
        coroutine_running = false;
        pose_switched = false;
        textClip = GetComponent<RandomAudioClip>().getRandomClip(GetComponent<RandomAudioClip>().soundClips);
        //radio = GameObject.FindGameObjectWithTag("Radio").GetComponent<Radio>();
	}

    void Awake()
    {
        sm = GameObject.FindWithTag("SanityHandler").GetComponentInChildren<SanityMeter>();
        coroutine_running = false;
        pose_switched = false;
        textClip = GetComponent<RandomAudioClip>().getRandomClip(GetComponent<RandomAudioClip>().soundClips);
        //radio = GameObject.FindGameObjectWithTag("Radio").GetComponent<Radio>();
    }

    // Update is called once per frame
    void Update () {
        Debug.Log("Distance: " + Vector3.Distance(transform.position, new Vector3(radio.transform.position.x, transform.position.y, radio.transform.position.z)));
		if ((Vector3.Distance(transform.position,new Vector3(radio.transform.position.x,transform.position.y, radio.transform.position.z)) <= inRange) && radio.GetComponent<Radio>().radioOn&&!pose_switched)
        {
            transform.parent.GetComponent<ObserveableManager>().musicianHeard = true;
            poseSwitch();
        }
        if (pose_switched&&!coroutine_running && musicianSource.clip == textClip)
        {
            StartCoroutine(musicianCarHaunt());
        }
        if (pose_switched && coroutine_running){
            pose2.transform.position = carPosition.transform.position;
            pose2.transform.rotation = carPosition.transform.rotation;
            //pose2.transform.Translate(0, 0, 0.01f);
        }
        if (coroutine_running && musicianSource.clip == textClip && !musicianSource.isPlaying)
        {
            musicianSource.clip = musicClip;
            musicianSource.loop = true;
            musicianSource.Play();
        }
	}
    void poseSwitch()
    {
        pose2.transform.position = carPosition.transform.position;
        pose2.transform.rotation = carPosition.transform.rotation;
        //pose2.transform.Translate(0, 0, 0.01f);
        pose2.SetActive(true);
        pose2.GetComponentInChildren<GhostMusician>().pose_switched = true;
        //pose2.GetComponent<ObserveableManager>().musicianHeard = true;
        this.gameObject.transform.parent.gameObject.SetActive(false);
    }

    IEnumerator musicianCarHaunt()
    {
        coroutine_running = true;
        sm.lowerSanity = true;
        musicianSource.clip = textClip;
        musicianSource.Play();
        for(int i = 0; i < 10; i++)
        {
            sm.lowerOnce(4.5f);
            yield return new WaitForSeconds(1f);
        }
        sm.lowerSanity = false;
        coroutine_running = false;
    }
}
