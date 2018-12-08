using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyOptions : MonoBehaviour {
    public Slider difficultySlider;

	// Use this for initialization
	void Start () {
		difficultySlider.onValueChanged.AddListener(delegate { DifficultyChanged(); });
	}
	/*
	// Update is called once per frame
	void Update () {
		
	}
    */

    // take in difficulty change
    void DifficultyChanged()
    {
        Debug.Log(difficultySlider.value);
    }
}
