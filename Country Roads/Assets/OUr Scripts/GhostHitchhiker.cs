using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostHitchhiker : MonoBehaviour {

    public GameObject carPosition;
    public GameObject meshObject;
    public GameObject pose1;
    public GameObject pose2;
    public bool ghostInCar;
    public GameObject car;
    private float randomSteeringValue;
    public bool coroutine_running;
    public AudioClip[] ghostClips;
    public AudioSource ghostSource;
    public bool audio_running;

    // Use this for initialization
    void Start() {
        meshObject = gameObject.transform.Find("Men_4").gameObject;
        carPosition = GameObject.Find("CarPosition");
        //ghostInCar = false;
        car = GameObject.Find("Player");
        randomSteeringValue = 0;
        coroutine_running = false;
        ghostSource = GetComponent<AudioSource>();
        audio_running = false;
    }

    void Awake()
    {
        meshObject = gameObject.transform.Find("Men_4").gameObject;
        carPosition = GameObject.Find("CarPosition");
        //ghostInCar = false;
        car = GameObject.Find("Player");
        randomSteeringValue = 0;
        coroutine_running = false;
        ghostSource = GetComponent<AudioSource>();
        audio_running = false;
    }

    // Update is called once per frame
    void Update() {
        bool isSeen = lookForPlayer();
        if (isSeen&&!coroutine_running)
        {
            StartCoroutine(playerStops());
        }
        if (ghostInCar && !audio_running)
        {
            StartCoroutine(ghostStops());
        }
        if (ghostInCar&&GameObject.FindWithTag("GameCamera").GetComponent<CameraController>().isGhostSeen("HikerInCar"))
        {
            Debug.Log("Got in if statement");
            GameObject.Find("SanityMeter").GetComponentInChildren<SanityMeter>().sanityLowering(2);
        }
        else
        {
            GameObject.Find("SanityMeter").GetComponentInChildren<SanityMeter>().lowerSanity = false;
        }
        if (ghostInCar)
        {
            //transform.position = carPosition.transform.position;
            this.transform.position = carPosition.transform.position;
            this.transform.rotation = carPosition.transform.rotation;
        }
    }

    //shuffle events
    public ArrayList Shuffle(ArrayList a)
    {
        ArrayList res = new ArrayList();
        System.Random rand = new System.Random();
        int randIndex = 0;
        for (int i = 0; i < 4; i++)
        {
            randIndex = rand.Next(0, a.Count);
            Debug.Log("Adding: " + a[randIndex]);
            res.Add(a[randIndex]);
            a.RemoveAt(randIndex);
        }
        return res;
    }


    public void playRandomEvent(int eventType)
    {
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

    IEnumerator noBrakes()
    {
        car.GetComponent<RCC_CarControllerV3>().brakeInput = 0;
        car.GetComponent<RCC_CarControllerV3>().handbrakeInput = 0;
        car.GetComponent<cr_controller>().car_manipulated = true;
        yield return new WaitForSeconds(3f);
        car.GetComponent<cr_controller>().car_manipulated = false;
    }

    IEnumerator shiftSteering()
    {
        float randomAmount = Random.Range(-1, 1);
        //randomAmount = 1;
        car.GetComponent<cr_controller>().randomSteeringValue=randomAmount;
        yield return new WaitForSeconds(3f);
        car.GetComponent<cr_controller>().randomSteeringValue = 0;
    }

    IEnumerator carAcceleration()
    {
        float acceleration = Random.Range(0f,1f);
        //acceleration = 1;
        car.GetComponent<cr_controller>().accelerate = acceleration;
        yield return new WaitForSeconds(3f);
        car.GetComponent<cr_controller>().accelerate = 0;
    }

    IEnumerator lightsTurnOn()
    {
        car.GetComponent<RCC_CarControllerV3>().highBeamHeadLightsOn = true;
        yield return new WaitForSeconds(3f);
        car.GetComponent<RCC_CarControllerV3>().highBeamHeadLightsOn = false;
    }

    public bool lookForPlayer()
    {
        RaycastHit hit=new RaycastHit();
        if (Physics.Raycast(new Ray(transform.position,transform.forward), out hit, 100f)){
            Debug.DrawRay(transform.position,transform.forward*100f);
            if (hit.collider.gameObject.tag == "Player")
            {
                Debug.Log("I SEE YOU");
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    IEnumerator playerStops()
    {
        coroutine_running = true;
        yield return new WaitForSeconds(3f);
        if (lookForPlayer())
        {
            //get ghost in car
            pose2.transform.position = carPosition.transform.position;
            pose2.transform.rotation = carPosition.transform.rotation;
            pose2.transform.Translate(0, -0.21f, 0);
            pose2.gameObject.GetComponent<GhostHitchhiker>().coroutine_running = false;
            pose2.SetActive(true);
            pose2.gameObject.GetComponent<GhostHitchhiker>().ghostInCar = true;
            this.gameObject.SetActive(false);
            //pose2.transform.position = meshObject.transform.position;
            //pose2.transform.rotation = meshObject.transform.rotation;
            //pose2Instance=Instantiate(pose2, carPosition.transform.position, pose2.transform.rotation);
            //pose2Instance.SetActive(true);

            //necessary rotations
            //gameObject.transform.Find("mixamorig:LeftShoulder").gameObject.transform.Rotate(45, 0, 0);
            //gameObject.transform.Find("mixamorig:RightShoulder").gameObject.transform.Rotate(30, 66, -27);
            //gameObject.transform.Find("mixamorig:LeftUpLeg").gameObject.transform.Rotate(-90, 0, 0);
            //gameObject.transform.Find("mixamorig:RightUpLeg").gameObject.transform.Rotate(-90, 0, 0);
            //gameObject.transform.Find("mixamorig:LeftLeg").gameObject.transform.Rotate(-90, 0, 0);
            //gameObject.transform.Find("mixamorig:RightLeg").gameObject.transform.Rotate(-90, 0, 0);

            //meshObject.SetActive(false);
            //meshObject.GetComponent<SkinnedMeshRenderer>().sharedMesh = pose2.GetComponent<SkinnedMeshRenderer>().sharedMesh;
            ghostInCar = true;
        }
        else
        {
            StartCoroutine(randomizeEvents());
        }
        coroutine_running = false;
    }

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
        //ArrayList b = a;
        //play shuffled events with breaks in between
        for (int i = 0; i < 4; i++)
        {
            Debug.Log(b[i]);
            playRandomEvent((int)b[i]);
            yield return new WaitForSeconds(3f);
        }
        gameObject.transform.parent.GetComponent<ObserveableManager>().hikerDone = true;
    }

    IEnumerator ghostStops()
    {
        audio_running = true;
        ghostSource.clip = ghostClips[Random.Range(0, ghostClips.Length-1)];
        ghostSource.Play();
        yield return new WaitForSeconds(ghostSource.clip.length);
        this.gameObject.SetActive(false);
    }
}
