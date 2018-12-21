/**
 *  Title:       EndSceneManager.cs
 *  Description: This script is used to get back to the start menu automatically after the player spending 20 seconds in the ending scene.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndSceneManager : MonoBehaviour {

    public float delay = 20;        //to hold the delay that starting the menu should have

	// Use this for initialization
	void Start () {
        //start the coroutine for starting the start menu
        StartCoroutine(LoadMenuScene(delay));
	}

    //function to load the starting scene after a delay
    IEnumerator LoadMenuScene(float delay)
    {
        //wait for the specified delay amount
        yield return new WaitForSeconds(delay);
        //load the starting scene
        SceneManager.LoadScene("Start");
    }
}
