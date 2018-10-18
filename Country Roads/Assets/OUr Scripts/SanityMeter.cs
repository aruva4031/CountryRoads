using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanityMeter : MonoBehaviour {

    public int sanity = 100;
    public bool lowerSanity;
    public bool coroutine_running;
    public GameObject bloodEffects;
    public GameObject gameRadio;

    // Use this for initialization
    void Start () {
        lowerSanity = false;
        coroutine_running = false;
        InvokeRepeating("sanityLowering", 0.0f, 1.0f);
        InvokeRepeating("sanityIncreasing", 0.0f, 1.0f);
        bloodEffects = GameObject.FindWithTag("BloodEffect");
        bloodEffects.SetActive(false);
        gameRadio = GameObject.FindGameObjectWithTag("Radio");
    }
	
	// Update is called once per frame
	void Update () {
        GetComponent<UnityEngine.UI.Text>().text="Sanity: "+sanity;
        if (sanity <= 45&&!coroutine_running)
        {
            StartCoroutine(sanityEvent());
            coroutine_running = true;
        }
        else if (sanity > 45 && coroutine_running)
        {
            StopCoroutine(sanityEvent());
            coroutine_running = false;
        }
	}

    public void sanityLowering()
    {
        if (lowerSanity && sanity > 0 && (gameRadio.GetComponent<Radio>().radioOn==false))
        {
            sanity -= 5;
        }
    }

    public void sanityIncreasing()
    {
        if ((lowerSanity==false) && (sanity < 100)&&!coroutine_running)
        {
            sanity++;
        }
    }

    public void lowerOnce()
    {
        sanity -= 5;
    }

    public void sanityLowerCall()
    {
        lowerSanity = true;
    }

    public IEnumerator sanityEvent()
    {
        bloodEffects.SetActive(true);
        yield return new WaitForSeconds(10);
        bloodEffects.SetActive(false);
        sanity += 10;
    }
}
