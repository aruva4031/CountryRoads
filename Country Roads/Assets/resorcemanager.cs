using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resorcemanager : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameObject[] roads = GameObject.FindGameObjectsWithTag("curvedroadleft");
        foreach(GameObject road in roads)
        {
            for (int i = 1; i < road.transform.childCount; i++)
            {
                GameObject child = road.transform.GetChild(i).gameObject;
                if (child != null)
                {
                    child.SetActive(true);
                }
            }
        }
        GameObject[] roads2 = GameObject.FindGameObjectsWithTag("curvedroadright");
        foreach (GameObject road in roads2)
        {
            for (int i = 1; i < road.transform.childCount; i++)
            {
                GameObject child = road.transform.GetChild(i).gameObject;
                if (child != null)
                {
                    child.SetActive(true);
                }
            }
        }
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
		if (other.tag == "croosmesh")
		{
			for (int i = 0; i < 1; i++)
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
		if (other.tag == "croosmesh")
		{
			for (int i = 0; i < 1; i++)
			{
				GameObject child = other.transform.GetChild(i).gameObject;
				if (child != null)
				{
					child.SetActive(false);
				}
			}
		}

    }
}
