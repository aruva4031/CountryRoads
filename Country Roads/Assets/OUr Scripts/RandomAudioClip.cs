/**
 *  Title:       RandomAudioClip.cs
 *  Description: Used to choose and set a random audio clip out of multiple clips
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAudioClip : MonoBehaviour {

    System.Random rand = new System.Random();   //to generate a random number in the range of clips to find a random clip
    public AudioSource soundSource;             //the audio source that should play the clip
    public AudioClip[] soundClips;              //an array of possible clips

	// Use this for initialization
	void Start () {
        //if the audio source is initialized, assign a random sound clip to it and set the clip of the audio source to the assigned clip
        if (soundSource)
        {
            soundSource.clip = getRandomClip(soundClips);
        }
	}

    //function to assign a random audio clip to an audio source, given an array of clips
    public AudioClip getRandomClip(AudioClip[] clips)
    {
        //generate a random index of the range of the sound clips array
        int clipNumber = rand.Next(0, soundClips.Length);
        //return the clip stored at the index
        return clips[clipNumber];
    }
}
