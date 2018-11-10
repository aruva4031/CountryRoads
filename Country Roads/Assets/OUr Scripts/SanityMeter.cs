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
		bloodEffects = GameObject.FindWithTag("BloodEffect");
		bloodEffects.SetActive(false);
		gameRadio = GameObject.FindGameObjectWithTag("Radio");
	}

	// Update is called once per frame
	void Update () {
		GetComponent<UnityEngine.UI.Text>().text="Sanity: "+sanity;
		if (sanity <= 45&&!coroutine_running)
		{
			sanityEvent (2);
			coroutine_running = true;
		}
		else if (sanity > 45 && coroutine_running)
		{
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

	public void lowerOnce(int ammount)
	{
		sanity -= ammount;
	}

	public void sanityLowering(int constantAmount) {
		sanity -= constantAmount;
	}

	public void sanityLowerCall()
	{
		lowerSanity = true;
	}
	private void sanityEvent(int selector)
	{
		if (selector == 1) {
			StartCoroutine (BloodyWindows ());
		} 
		else if (selector == 2) {
			StartCoroutine (TalkitiveRadio ());
		}
	}
	//selector must be 1
	public IEnumerator BloodyWindows()
	{
		bloodEffects.SetActive(true);
		yield return new WaitForSeconds(10);
		bloodEffects.SetActive(false);
		sanity += 10;
	}
	//selector must be 2
	public IEnumerator TalkitiveRadio()
	{
		gameRadio.GetComponent<Radio> ().SanityRadio ();
		yield return new WaitForSeconds(gameRadio.GetComponent<Radio>().insaneRadio.clip.length);
		sanity += 10;
	}
}
