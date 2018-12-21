/**
 *  Title:       GhostHitchhiker.cs
 *  Description: This script is used to model the ghost hitchhiker. If you pick him up, he will substract sanity when being looked at, if he
 *  is not picked up, he will mess with the car controls
 *  Outcome addressed: Model behaviour of non-player characters/machine learning
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostHitchhiker : MonoBehaviour {

    public GameObject carPosition;          //to hold the position of the ghost hitchhiker in the car
    public GameObject pose2;                //to hold the pose for the hitchhiker in the car
    public bool ghostInCar;                 //to determine if the ghost is in the car
    public GameObject car;                  //to access the car 
    private float randomSteeringValue;      //to hold a random value to mess with the steering
    public bool coroutine_running;          //to determine if the coroutine is running
    public AudioClip[] ghostClips;          //to hold the audio clips available for the hitchhiker
    public AudioSource ghostSource;         //to access the audio source of the ghost
    public bool audio_running;              //to determine if the audio is running

    // Use this for initialization
    void Start() {
        //find the car position of the player
        carPosition = GameObject.Find("CarPosition");
        car = GameObject.Find("Player");
        //set the random steering value to 0
        randomSteeringValue = 0;
        //the coroutine is not running yet, neither is the audio
        coroutine_running = false;
        audio_running = false;
        //get the audio source
        ghostSource = GetComponent<AudioSource>();
        //find the pose and car position
        pose2 = GameObject.Find("PublicObjects").GetComponent<PublicObjects>().hikerPose2;
        carPosition = GameObject.Find("PublicObjects").GetComponent<PublicObjects>().carPosition;
    }

    void Awake()
    {
        //do the same things as in Start() to apply it when the objects get activated
        carPosition = GameObject.Find("CarPosition");
        car = GameObject.Find("Player");
        randomSteeringValue = 0;
        coroutine_running = false;
        ghostSource = GetComponent<AudioSource>();
        audio_running = false;
    }

    // Update is called once per frame
    void Update() {
        //look if the player is seen
        bool isSeen = lookForPlayer();
        //if the player is seen and the coroutine is running, start a coroutine to see if the player stops
        if (isSeen&&!coroutine_running)
        {
            StartCoroutine(playerStops());
        }
        //if the ghost is in the car and the audio has stopped, start a coroutine to stop the ghost
        if (ghostInCar && !audio_running)
        {
            StartCoroutine(ghostStops());
        }
        //if the ghost is in the car and the player sees the ghost, lower the sanity by 2
        if (ghostInCar&&GameObject.FindWithTag("GameCamera").GetComponent<CameraController>().isGhostSeen("HikerInCar"))
        {
            GameObject.Find("SanityMeter").GetComponentInChildren<SanityMeter>().sanityLowering(2);
        }
        //otherwise, set the sanity lowering to false
        else
        {
            GameObject.Find("SanityMeter").GetComponentInChildren<SanityMeter>().lowerSanity = false;
        }
        //if the ghost is in the car, set the hitchhiker to the car position
        if (ghostInCar)
        {
            this.transform.position = carPosition.transform.position;
            this.transform.rotation = carPosition.transform.rotation;
        }
    }

    //shuffle events
    public ArrayList Shuffle(ArrayList a)
    {
        //create a new array list
        ArrayList res = new ArrayList();
        //create a new random object
        System.Random rand = new System.Random();
        //create a variable for a random index
        int randIndex = 0;
        //do the following four times
        for (int i = 0; i < 4; i++)
        {
            //set the random index to a random index between 0 and the length of a, the input array(has elements 1,2,3,4)
            randIndex = rand.Next(0, a.Count);
            //add the element at that index to the result array
            res.Add(a[randIndex]);
            //remove the index from the original array
            a.RemoveAt(randIndex);
        }
        //return the resulting array
        return res;
    }

    //function to play a random event
    public void playRandomEvent(int eventType)
    {
        //based on the input of the eventType number, start the right coroutine
        switch (eventType)
        {
            case 1:
                StartCoroutine(noBrakes());
                break;
            case 2:
                StartCoroutine(shiftSteering());
                break;
            case 3:
                StartCoroutine(carAcceleration());
                break;
            case 4:
                StartCoroutine(lightsTurnOn());
                break;
        }
    }

    //function to deactivate the brak
    IEnumerator noBrakes()
    {
        //set the brakeInput and handBrake input to 0 and set the car_manipulated variable to false
        //the brakes are now deactivated
        car.GetComponent<RCC_CarControllerV3>().brakeInput = 0;
        car.GetComponent<RCC_CarControllerV3>().handbrakeInput = 0;
        car.GetComponent<cr_controller>().car_manipulated = true;
        //wait for 3 seconds
        yield return new WaitForSeconds(3f);
        //reactivate the brakes
        car.GetComponent<cr_controller>().car_manipulated = false;
    }

    //function to shift the steering
    IEnumerator shiftSteering()
    {
        //get a random value between -1 and 1, which is the steering range
        float randomAmount = Random.Range(-1, 1);
        //set the randomSteeringValue of the car controller to the random amount: it will add it to the steering input
        car.GetComponent<cr_controller>().randomSteeringValue=randomAmount;
        //wait for 3 seconds
        yield return new WaitForSeconds(3f);
        //set the randomSteeringValue back to 0: steering should be normal again
        car.GetComponent<cr_controller>().randomSteeringValue = 0;
    }

    //function to change the car speed
    IEnumerator carAcceleration()
    {
        //get a random value between 0 and 1, which is the acceleration range
        float acceleration = Random.Range(0f,1f);
        //change the accelerate value in the car controller: it will be added to the normal acceleration
        car.GetComponent<cr_controller>().accelerate = acceleration;
        //wait for 3 seconds
        yield return new WaitForSeconds(3f);
        //change the accelerate value in the car controller back to 0: acceleration should be back to normal
        car.GetComponent<cr_controller>().accelerate = 0;
    }

    //function to turn on the lights
    IEnumerator lightsTurnOn()
    {
        //turn the lights on
        car.GetComponent<RCC_CarControllerV3>().highBeamHeadLightsOn = true;
        //wait for 3 seconds
        yield return new WaitForSeconds(3f);
        //turn the lights off
        car.GetComponent<RCC_CarControllerV3>().highBeamHeadLightsOn = false;
    }

    //function to look for the player
    public bool lookForPlayer()
    {
        //get the appropriate position for a raycast looking for the player
        Vector3 pos = new Vector3(transform.position.x, car.transform.position.y+1, transform.position.z);
        //create a new RaycastHit to use for the raycast output
        RaycastHit hit=new RaycastHit();
        //create a raycast forward from the ghost but higher
        if (Physics.Raycast(new Ray(pos,transform.forward), out hit, 100f)){
            //draw a ray for debugging purposes
            Debug.DrawRay(pos,transform.forward*100f);
            //if the hit collider object is the player, return true: player is seen
            if (hit.collider.gameObject.tag == "Player")
            {
                return true;
            }
            //otherwise, the player is not seen
            else
            {
                return false;
            }
        }
        //if the raycast fails, the player is not seen either
        return false;
    }

    //coroutine to see if the player stops
    IEnumerator playerStops()
    {
        //determine that the coroutine is running
        coroutine_running = true;
        //wait for 3 seconds
        yield return new WaitForSeconds(3f);
        //if the player is still there
        if (lookForPlayer())
        {
            //get the ghost hitchhiker of the sitting pose in the car
            pose2.transform.position = carPosition.transform.position;
            pose2.transform.rotation = carPosition.transform.rotation;
            pose2.transform.Translate(0, -0.21f, 0);
            //change the coroutine_running boolean of the pose 2 hitchhiker to false
            pose2.gameObject.GetComponent<GhostHitchhiker>().coroutine_running = false;
            //set the pose 2 ghost hitchhiker to true
            pose2.SetActive(true);
            //set the pose 2 ghostInCar component to true: the ghost is in the car
            pose2.gameObject.GetComponent<GhostHitchhiker>().ghostInCar = true;
            //set the current hitchhiker to false
            this.gameObject.SetActive(false);
            //set the current ghostInCar to true
            ghostInCar = true;
        }
        //otherwise: start the randomized events messing with car controls using a coroutine
        else
        {
            StartCoroutine(randomizeEvents());
        }
        //set coroutine_running to false
        coroutine_running = false;
    }

    //function to play the randomized events to mess with the car controls
    IEnumerator randomizeEvents()
    {
        //create list for all 4 events
        ArrayList a = new ArrayList();
        a.Add(1);
        a.Add(2);
        a.Add(3);
        a.Add(4);
        //shuffle events
        ArrayList b = Shuffle(a);
        //play shuffled events with breaks in between
        for (int i = 0; i < 4; i++)
        {
            //play event of current array value
            playRandomEvent((int)b[i]);
            //wait for 3 seconds
            yield return new WaitForSeconds(3f);
        }
        //set the hikerDone variable to true
        gameObject.transform.parent.GetComponent<ObserveableManager>().hikerDone = true;
    }

    //function to make the ghost stops
    IEnumerator ghostStops()
    {
        //set audio_running to true and play a random audio clip for the ghost hitchhiker
        audio_running = true;
        ghostSource.clip = ghostClips[Random.Range(0, ghostClips.Length-1)];
        ghostSource.Play();
        //wait for the length of the audio clip
        yield return new WaitForSeconds(ghostSource.clip.length);
        //set the game object to false
        this.gameObject.SetActive(false);
    }
}
