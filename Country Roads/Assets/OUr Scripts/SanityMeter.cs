using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanityMeter : MonoBehaviour {

    public int sanity = 100;
    public bool lowerSanity;
    public bool coroutine_running;

    // Use this for initialization
    void Start () {
        lowerSanity = false;
        coroutine_running = false;
        InvokeRepeating("sanityLowering", 0.0f, 1.0f);
    }
	
	// Update is called once per frame
	void Update () {
        GetComponent<UnityEngine.UI.Text>().text="Sanity: "+sanity;
	}

    public void sanityLowering()
    {
        if (lowerSanity && sanity > 0)
        {
            sanity -= 1;
        }
    }

    public void sanityLowerCall()
    {
        StartCoroutine(sanityLoweringStart());
    }

    public IEnumerator sanityLoweringStart()
    {
        yield return new WaitForSeconds(15);
        lowerSanity = true;
    }
}
