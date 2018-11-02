using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEventCollision : MonoBehaviour {

    //if the player collides with a trigger collider
    private void OnTriggerEnter(Collider collider)
    {
        //if the player collides with the ghost child
        if (collider.gameObject.tag == "GhostChild")
        {
            //if the ghost child has the GhostChild script
            if (collider.gameObject.GetComponent<GhostChild>())
            {
                //make the ghost child start haunting the player in the car by calling the startHaunting() function
                collider.gameObject.GetComponent<GhostChild>().startHaunting();
            }
        }
        //if (collider.gameObject.tag == "StalkerGhost")
        //{
        //    Debug.Log(collider.transform.tag);
        //    collider.gameObject.GetComponentInParent<StalkerGhostAI>().speed = 0;
        //    collider.gameObject.GetComponentInParent<StalkerGhostAI>().killPlayer();
        //}
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if the player collides with the ghost child
        if (collision.transform.tag == "Deer")
        {
            Debug.Log(collision.transform.tag);
            collision.gameObject.GetComponentInParent<Animator>().SetBool("deerFalls", true);
            GetComponentInParent<PlayerController>().decreaseCarDamage();
        }
    }

}
