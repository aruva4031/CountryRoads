using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMusician : MonoBehaviour {

    public float speed;
    public float inRange;
    public AudioSource musicianSource;
    public AudioClip textClip;
    public AudioClip musicClip;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if((Vector3.Distance(transform.position,new Vector3(GameObject.Find("Radio").transform.position.x,transform.position.y, GameObject.Find("Radio").transform.position.z)) < inRange) && GameObject.Find("Radio").GetComponent<Radio>().radioOn)
        {
            StartCoroutine(musicianCarHaunt()){

            }
        }
	}

    IEnumerator musicianCar
}
