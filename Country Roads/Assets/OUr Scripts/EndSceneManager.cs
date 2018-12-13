using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndSceneManager : MonoBehaviour {

    public float delay = 20;

	// Use this for initialization
	void Start () {
        StartCoroutine(LoadMenuScene(delay));
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator LoadMenuScene(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("Start");
    }
}
