using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAudioClip : MonoBehaviour {

    System.Random rand = new System.Random();
    public AudioSource soundSource;
    public AudioClip[] soundClips;
	// Use this for initialization
	void Start () {
        if (soundSource)
        {
            soundSource.clip = getRandomClip(soundClips);
        }
	}

    public AudioClip getRandomClip(AudioClip[] clips)
    {
        int clipNumber = rand.Next(0, soundClips.Length);
        return clips[clipNumber];
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
