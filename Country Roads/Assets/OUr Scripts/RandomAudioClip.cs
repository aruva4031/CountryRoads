using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAudioClip : MonoBehaviour {
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
        int clipNumber = Random.Range(0, clips.Length);
        return clips[clipNumber];
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
