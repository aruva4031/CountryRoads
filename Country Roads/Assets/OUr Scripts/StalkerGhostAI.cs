using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StalkerGhostAI : MonoBehaviour {
    public float speed=2f;                  //walking speed of the ghost, can be adjusted
    public float originalSpeed;             //to store the original speed of the stalker ghost
    public GameObject target;               //to store where the ghost is moving to: the player
    public AudioSource deathSource;         //to get the audio source of the stalker ghost
    public AudioClip deathScream;           //to get a clip for the death screma
    public Animation anim;                  //to access the animator of the camera
    public float maxDistance;               //to determine the maximum distance to be slow
    public float closeDistance;             //to determine the close distance to be slow again
    public bool isKilled;                   //to determine if the player is basically being killed
    private bool coroutine_running;         //to determine if the coroutine is running
    public bool stopMovement;               //to determine if the movement has stopped
    public GameObject playerController;     //to access the playerController scriüt


    // Use this for initialization
    void Start () {
        //find the animator of the camera, the target ghost position and the player
        anim = GameObject.FindWithTag("Camera").GetComponent<Animation>();
        target = GameObject.Find("GhostPosition");
        playerController = GameObject.Find("Player");
        //get the audio source and play it, which will continue as a loop
        deathSource = GetComponent<AudioSource>();
        deathSource.Play();
        //set all booleans to false
        isKilled = false;
        coroutine_running = false;
        stopMovement = false;
        //set the original speed to the current speed
        originalSpeed = speed;
    }
	
	// Update is called once per frame
	void Update () {
        //create realtime step
        float step = speed * Time.deltaTime;
        //use moveTowards() function to follow/ move towards player
        this.transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z), step);
        //if the player is too far away or has stopped moving, the speed will increase to the original speed times 1.5
        if (Vector3.Distance(transform.position, target.transform.position) > maxDistance || (playerController.GetComponent<PlayerController>().carDamage == 0))
        {
            speed = originalSpeed*1.5f;
        }
        //if the stalker ghost is getting close to the player and the player hasn't stopped moving, the speed of the ghost will decrease to 2/3 of the original speed
        else if ((Vector3.Distance(transform.position, target.transform.position) < closeDistance) && (playerController.GetComponent<PlayerController>().carDamage != 0))
        {
            speed = originalSpeed*(2/3);
        }
        //if the killing coroutine didn't start yet and the stalker ghost basically arrived at the position next to the car
        if (!coroutine_running&&Vector3.Distance(transform.position, target.transform.position)<=0.5f)
        {
            //start the player losing coroutine
            StartCoroutine(playerLoses());
            //stop the player movement
            speed = 0;
        }
        //if the distance between player and stalker ghost is basically there and the speed is zero and the car is still running, reset the speed back to the original speed
        if (!(Vector3.Distance(transform.position, target.transform.position) <= 0.5f)&&speed==0 && (playerController.GetComponent<PlayerController>().carDamage != 0))
        {
            speed = originalSpeed;
        }
        //else if the distance between player and stalker ghost is basically there and the speed is zero and the car is still running, set the speed to the original speed times 1.5
        else if (!(Vector3.Distance(transform.position, target.transform.position) <= 0.5f) && speed == 0 && (playerController.GetComponent<PlayerController>().carDamage != 0))
        {
            speed = originalSpeed*1.5f;
        }
         //if the player is killed and the death source stopped playing
        if (isKilled==true & !deathSource.isPlaying)
        {
            //set the death source clip to the death scream and play it
            deathSource.clip = deathScream;
            deathSource.Play();
        }
    }

    //function to make the player lose
    IEnumerator playerLoses()
    {
        //the coroutine is running
        coroutine_running = true;
        //stop any movement
        stopMovement = true;
        //set the playerDies animator boolean of the camera animator to true, so that the camera will turn to the window
        GameObject.FindWithTag("Camera").GetComponent<Animator>().SetBool("playerDies",true);
        //stop the audio source of the stalker ghost
        deathSource.Stop();
        //set the looping of the stalker ghost audio source to false
        deathSource.loop = false;
        //get a random audio clip as clip for the audio source
        deathSource.clip = GetComponent<RandomAudioClip>().getRandomClip(GetComponent<RandomAudioClip>().soundClips);
        //play the audio source.
        deathSource.Play();
        //set the isKilled boolean to true: the player is killed
        isKilled = true;
        //wait until the death scream and the ghost wail have been played
        yield return new WaitForSeconds(deathScream.length+deathSource.clip.length);
        //restart the scene
        SceneManager.LoadScene("MVP");
    }
}
