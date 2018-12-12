using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanityMeter : MonoBehaviour
{
    public float sanity = 100;
    public bool lowerSanity;
    public bool coroutine_running;
    public GameObject bloodEffects;
    public GameObject gameRadio;
    public GameObject fireEffects;
    public GameObject brightLight;
    public bool sanityIncreasing;

    private int selection = 1;
    private int lastSelection = 1;

    // Use this for initialization
    void Start()
    {
        lowerSanity = false;
        coroutine_running = false;
        InvokeRepeating("sanityLowering", 0.0f, 1.0f);
        bloodEffects = GameObject.FindWithTag("BloodEffect");
        bloodEffects.SetActive(false);
        gameRadio = GameObject.FindGameObjectWithTag("Radio");

        // added BrightLight
        brightLight = GameObject.FindGameObjectWithTag("BrightLight");

        sanityIncreasing = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (sanity > 100)
        {
            sanity = 100;
        }
        if (sanity < 0)
        {
            sanity = 0;
        }
        //************************CHANGED!!!!!!!!!!!*****************************************************************************
        GetComponent<UnityEngine.UI.Text>().text = "Sanity: " + (int)sanity;
        //************************CHANGED END!!!!!!!!!!!*************************************************************************
        if (sanity <= 45 && !coroutine_running)
        {
            selection = (int)(Random.Range(1, 4));

            if (selection != lastSelection)
            {
                sanityEvent(selection);
                coroutine_running = true;
            }
            else
            {
                while (selection == lastSelection)
                {
                    selection = (int)(Random.Range(1, 4));
                }

                sanityEvent(selection);
                coroutine_running = true;
            }

            lastSelection = selection;
        }
    }

    public void FixedUpdate()
    {
        //************************CHANGED!!!!!!!!!!!*************************************************************************
        if (sanity > 45 && sanity < 100 && !coroutine_running && !lowerSanity && sanityIncreasing)
        {
            if (GameObject.Find("Radio").GetComponent<Radio>().radioOn)
            {
                sanity += Time.deltaTime * 1f;
            }
            else
            {
                sanity += Time.deltaTime * 0.5f;
            }
        }
        else if (sanityIncreasing)
        {
            sanityIncreasing = false;
        }
        if (sanity > 45 && sanity < 100 && !coroutine_running && !lowerSanity && !sanityIncreasing)
        {
            StartCoroutine(increaseSanity());
        }
        //************************CHANGED END!!!!!!!!!!!*************************************************************************
    }

    IEnumerator increaseSanity()
    {
        sanityIncreasing = false;
        yield return new WaitForSeconds(5f);
        if (sanity > 45 && sanity < 100 && !coroutine_running && !lowerSanity)
        {
            sanityIncreasing = true;
        }
    }

    public void sanityLowering()
    {
        if (lowerSanity && sanity > 0 && (gameRadio.GetComponent<Radio>().radioOn == false))
        {
            sanity -= 5;
        }
    }

    //************************CHANGED!!!!!!!!!!!*************************************************************************
    public void lowerOnce(float ammount)
    //************************CHANGED END!!!!!!!!!!!*************************************************************************
    {
        lowerSanity = true;
        sanity -= ammount;
    }

    //************************CHANGED!!!!!!!!!!!*************************************************************************
    public void sanityLowering(float constantAmount)
    {
        //************************CHANGED END!!!!!!!!!!!*************************************************************************
        lowerSanity = true;
        sanity -= constantAmount;
    }

    public void sanityLowerCall()
    {
        lowerSanity = true;
    }

    private void sanityEvent(int selector)
    {
        if (selector == 1)
        {
            StartCoroutine(BloodyWindows());
        }
        else if (selector == 2)
        {
            StartCoroutine(TalkativeRadio());
        }
        else if (selector == 3)
        {
            StartCoroutine(BrightLight());
        }
    }
    //selector must be 1
    public IEnumerator BloodyWindows()
    {
        Debug.Log("BloodyWindows running");
        bloodEffects.SetActive(true);
        yield return new WaitForSeconds(10);
        bloodEffects.SetActive(false);
        sanity += 10;
        coroutine_running = false;
    }
    //selector must be 2
    public IEnumerator TalkativeRadio()
    {
        Debug.Log("TalkativeRadio running");
        gameRadio.GetComponent<Radio>().SanityRadio();
        GetComponent<RandomAudioClip>().getRandomClip(GetComponent<RandomAudioClip>().soundClips);
        yield return new WaitForSeconds(gameRadio.GetComponent<Radio>().insaneRadio.clip.length);
        sanity += 10;
        coroutine_running = false;
    }

    //selector must be 3
    public IEnumerator BrightLight()
    {
        Debug.Log("BrightLight running");
        brightLight.GetComponent<BrightLight>().sanityIsLow = true;
        brightLight.GetComponent<BrightLight>().isPlaying = true;
        yield return new WaitForSeconds(brightLight.GetComponent<BrightLight>().ringingNoise.clip.length);
        sanity += 10;
        brightLight.GetComponent<BrightLight>().isDone = true;
        yield return new WaitForSeconds(2);
        brightLight.GetComponent<BrightLight>().isDone = false;
        coroutine_running = false;
    }
}

