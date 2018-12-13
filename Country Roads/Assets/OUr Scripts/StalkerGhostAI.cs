using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StalkerGhostAI : MonoBehaviour {
    public float speed=2f;             //walking speed of the ghost, can be adjusted
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
    public bool stopMovement;
    public GameObject playerController;


    // Use this for initialization
    void Start () {
        anim = GameObject.FindWithTag("Camera").GetComponent<Animation>();
        //set transparent color
        //gameObject.GetComponentInChildren<Renderer>().material.color
        //   = new Color(gameObject.GetComponentInChildren<Renderer>().material.color.r, gameObject.GetComponentInChildren<Renderer>().material.color.g, gameObject.GetComponentInChildren<Renderer>().material.color.b, 0.5f);
        target = GameObject.Find("GhostPosition");
        deathSource = GetComponent<AudioSource>();
        deathSource.Play();
        isKilled = false;
        coroutine_running = false;
        originalSpeed = speed;
        stopMovement = false;
        playerController = GameObject.Find("Player");
    }
	
	// Update is called once per frame
	void Update () {
        //create realtime step
        //Debug.Log(Vector3.Distance(transform.position, target.transform.position));
        float step = speed * Time.deltaTime;
        //Debug.Log("Step: " + Time.deltaTime);
        this.transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z), step);
        if (Vector3.Distance(transform.position, target.transform.position) > maxDistance || (playerController.GetComponent<PlayerController>().carDamage == 0))
        {
            speed = originalSpeed*1.5f;
        }
        else if ((Vector3.Distance(transform.position, target.transform.position) < closeDistance) && (playerController.GetComponent<PlayerController>().carDamage != 0))
        {
            speed = originalSpeed*(2/3);
        }
        //else if ((Vector3.Distance(transform.position, target.transform.position) < closeDistance))
        //{
        //    speed = originalSpeed * 1.5f;
        //}
        if (!coroutine_running&&Vector3.Distance(transform.position, target.transform.position)<=0.2f)
        {
            Debug.Log("Fail");
            StartCoroutine(playerLoses());
            speed = 0;
        }
        if (!(Vector3.Distance(transform.position, target.transform.position) <= 0.2f)&&speed==0 && (playerController.GetComponent<PlayerController>().carDamage != 0))
        {
            speed = originalSpeed;
        }
        else if (!(Vector3.Distance(transform.position, target.transform.position) <= 0.2f) && speed == 0 && (playerController.GetComponent<PlayerController>().carDamage != 0))
        {
            speed = originalSpeed*1.5f;
        }
            //use moveTowards() function to follow/ move towards player
            //this.transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.transform.position.x, transform.position.y,target.transform.position.z), step);
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
        stopMovement = true;
        //GameObject.FindWithTag("Camera").transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, -90, 0), Time.deltaTime * 3f);
        //yield return new WaitForSeconds(2f);
        //GameObject.FindWithTag("Camera").transform.rotation=Vector3.RotateTowards(new Vector3(GameObject.FindWithTag("Camera").transform.rotation.x, GameObject.FindWithTag("Camera").transform.rotation.y, GameObject.FindWithTag("Camera").transform.rotation.z),new Vector3(transform.rotation.x, transform.rotation.y-90f,transform.rotation.z),3f,0.0f);

        // Using Quaternion's rotation
        //GameObject.FindWithTag("Camera").transform.rotation = Quaternion.LookRotation(Quaternion.Euler(this.transform.rotation.x, this.transform.rotation.y, this.transform.rotation.z), Vector3.up);
        //GameObject.FindWithTag("GameCamera").transform.LookAt(new Vector3(target.transform.position.x, target.transform.position.y+5f, target.transform.position.z));
        
        //play audio source clip
        //yield return new WaitForSeconds(2.0f);
        GameObject.FindWithTag("Camera").GetComponent<Animator>().SetBool("playerDies",true);
        deathSource.Stop();
        deathSource.loop = false;
        deathSource.clip = GetComponent<RandomAudioClip>().getRandomClip(GetComponent<RandomAudioClip>().soundClips);
        deathSource.Play();
        isKilled = true;
        yield return new WaitForSeconds(deathScream.length+ghostWail.length);
        //restart scene
        SceneManager.LoadScene("MVP");
    }
}
