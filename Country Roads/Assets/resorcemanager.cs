using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resorcemanager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "straightRoad" || other.tag == "curvedroadleft" || other.tag == "curvedroadright")
        {
            for (int i = 1; i < other.transform.childCount; i++)
            {
                GameObject child = other.transform.GetChild(i).gameObject;
                if (child != null)
                {
                    child.SetActive(true);
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "straightRoad" || other.tag == "curvedroadleft" || other.tag == "curvedroadright")
        {
            for (int i = 1; i < other.transform.childCount; i++)
            {
                GameObject child = other.transform.GetChild(i).gameObject;
                if (child != null)
                {
                    child.SetActive(true);
                }
            }
        }

    }
}
