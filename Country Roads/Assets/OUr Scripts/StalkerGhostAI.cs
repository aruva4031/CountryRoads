using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StalkerGhostAI : MonoBehaviour {
    public float speed;             //walking speed of the ghost, can be adjusted
    public float originalSpeed;
    public GameObject target;      //to store where the ghost is moving to: the player
    public AudioSource deathSource;
    public AudioClip deathScream;
    public AudioClip ghostWail;
    public Animation anim;
    public float maxDistance;
    public float closeDistance;
    public bool isKilled;
    private bool coroutine_running;

    // Use this for initialization
    void Start () {
        anim = GameObject.FindWithTag("Camera").GetComponent<Animation>();
        //set transparent color
        Renderer[] meshes = GetComponentsInChildren<Renderer>();
        foreach(Renderer model in meshes)
        {
            model.material.color
           = new Color(gameObject.GetComponentInChildren<Renderer>().material.color.r, gameObject.GetComponentInChildren<Renderer>().material.color.g, gameObject.GetComponentInChildren<Renderer>().material.color.b, 0.5f);
        }
        //gameObject.GetComponentInChildren<Renderer>().material.color
        //   = new Color(gameObject.GetComponentInChildren<Renderer>().material.color.r, gameObject.GetComponentInChildren<Renderer>().material.color.g, gameObject.GetComponentInChildren<Renderer>().material.color.b, 0.5f);
        target = GameObject.Find("GhostPosition");
        deathSource = GetComponent<AudioSource>();
        deathSource.Play();
        isKilled = false;
        coroutine_running = false;
        originalSpeed = speed;
    }
	
	// Update is called once per frame
	void Update () {
        //create realtime step
        Debug.Log(Vector3.Distance(transform.position, target.transform.position));

        float step = speed * Time.deltaTime;
        if (Vector3.Distance(transform.position, target.transform.position) > maxDistance)
        {
            speed = originalSpeed*1.5f;
        }
        else if (Vector3.Distance(transform.position, target.transform.position) < closeDistance)
        {
            speed = originalSpeed*2/3;
        }
        if (!coroutine_running&&Vector3.Distance(transform.position, target.transform.position) <= 0.2f)
        {
            StartCoroutine(playerLoses());
            speed = 0;
        }
        //use moveTowards() function to follow/ move towards player
        this.transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.transform.position.x, transform.position.y,target.transform.position.z), step);
        if (isKilled==true & !deathSource.isPlaying)
        {
            deathSource.clip = deathScream;
            deathSource.Play();
        }
        //if (coroutine_running)
        //{
        //    transform.rotation = GameObject.FindWithTag("Camera").GetComponent<Animation>().transform.rotation;
        //}
    }

    //public void killPlayer()
    //{
    //    StartCoroutine(playerLoses());
    //}

    IEnumerator playerLoses()
    {
        coroutine_running = true;
        transform.Rotate(new Vector3(0, -90, 0));
        //play audio source clip
        GameObject.FindWithTag("Camera").GetComponent<Animator>().SetBool("playerDies",true);
        //transform.rotation = GameObject.FindWithTag("Camera").GetComponent<Animation>().transform.rotation;
        deathSource.Stop();
        deathSource.loop = false;
        deathSource.clip = ghostWail;
        deathSource.Play();
        isKilled = true;
        yield return new WaitForSeconds(deathScream.length+ghostWail.length);
        //restart scene
        SceneManager.LoadScene("MVP");
    }
}
