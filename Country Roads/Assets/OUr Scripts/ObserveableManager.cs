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

    // Use this for initialization
    void Start () {
        children = GetComponentsInChildren<Transform>();
        changeChildActivity(false);
        triggerDone = false;
        hikerDone = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (isHitchhiker && hikerDone && triggerDone)
        {
            changeChildActivity(false);
        }
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
        if (other.gameObject.tag == "Player" && this.gameObject.tag != "tree")
        {
            changeChildActivity(true);
            if (isGhost)
            {
                StartCoroutine(ghostDestruction());
            }
        }
        if (this.gameObject.tag == "tree"&& other.gameObject.tag == "Player")
        {
            this.gameObject.GetComponent<MeshRenderer>().enabled = true;
            this.gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player"&&this.gameObject.tag!="tree")
        {
            triggerDone = true;
            changeChildActivity(false);
            //if (!isHitchhiker)
            //{
            //    changeChildActivity(false);
            //}
        }
        if(this.gameObject.tag == "tree"&& other.gameObject.tag == "Player")
        {
            this.gameObject.GetComponent<MeshRenderer>().enabled = false;
            this.gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
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
