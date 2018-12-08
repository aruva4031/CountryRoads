using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyOptions : MonoBehaviour {
    public Slider difficultySlider;
    public int difficultyLevel;

	// Use this for initialization
	void Start () {
        this.difficultyLevel = 0;
		difficultySlider.onValueChanged.AddListener(delegate { DifficultyChanged(); });
	}

    // take in difficulty change
    void DifficultyChanged()
    {
        Debug.Log(difficultySlider.value);
        this.difficultyLevel = (int)(difficultySlider.value);

        // call the static method to set the level
        DifficultyChosen.setDifficultyLevel(this.difficultyLevel);
    }
}
