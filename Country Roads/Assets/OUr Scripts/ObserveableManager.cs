using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObserveableManager : MonoBehaviour {

    Transform[] children;
    public bool isGhost;
    public bool isHitchhiker;
    public bool hikerDone;
    public bool triggerDone;
    public GenerateEvents eventGenerator;
    public int index=-1;
    public bool musicianHeard;
    GameObject pos;

    // Use this for initialization
    void Start () {
        children = GetComponentsInChildren<Transform>();
        changeChildActivity(false);
        triggerDone = false;
        hikerDone = false;
        pos = GameObject.Find("PositionInCar");
    }

    // Update is called once per frame
    void Update () {
        if (isHitchhiker && hikerDone && triggerDone)
        {
            changeChildActivity(false);
            eventGenerator.generateSingleEvent(index);
        }
        if (lookForPlayer())
        {
            if (isGhost && !musicianHeard)
            {
                StartCoroutine(ghostDestruction());
            }
        }
	}

    public bool lookForPlayer()
    {
       
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(new Ray(new Vector3(transform.position.x,pos.transform.position.y,transform.position.z), transform.forward), out hit, 10f))
        {
            Debug.DrawRay(new Vector3(transform.position.x, pos.transform.position.y, transform.position.z), transform.forward * 100f);
            if (hit.collider.gameObject.tag == "Player")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    void changeChildActivity(bool isActive)
    {
        foreach (Transform child in children)
        {
            if (!(child.gameObject == this.gameObject))
            {
                child.gameObject.SetActive(isActive);
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "GenerationCollider" && this.gameObject.tag != "tree")
        {
            changeChildActivity(true);
        }
        if (this.gameObject.tag == "tree"&& other.gameObject.tag == "GenerationCollider")
        {
            this.gameObject.GetComponent<MeshRenderer>().enabled = true;
            if (this.gameObject.transform.childCount>0)
            {
                this.gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "GenerationCollider" && this.gameObject.tag!="tree")
        {
            triggerDone = true;
            changeChildActivity(false);
            if (!musicianHeard&&!isHitchhiker)
            {
                changeChildActivity(false);
            }
        }
        if(this.gameObject.tag == "tree"&& other.gameObject.tag == "GenerationCollider")
        {
            this.gameObject.GetComponent<MeshRenderer>().enabled = false;
            if (this.gameObject.transform.childCount>0)
            {
                this.gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }

    IEnumerator ghostDestruction()
    {
        yield return new WaitForSeconds(15f);
        if (eventGenerator != null&&index!=-1)
        {
            eventGenerator.generateSingleEvent(index);
        }
        Destroy(this.gameObject);
    }
}
