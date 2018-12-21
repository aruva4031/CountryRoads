using System.Collections;
/**
 *  Title:       PlayerEventCollision.cs
 *  Description: This script handles collision of the player with trees and events and determines, what should happen depending in the collision.
 */
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerEventCollision : MonoBehaviour {

    //if the player collides with a trigger collider
    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log(this.GetComponent<Collider>().GetType());
        //if the player collides with the ghost child
        if (collider.transform.tag == "GhostChild"|| collider.gameObject.tag == "GhostChild")
        {
            //if the ghost child has the GhostChild script
            if (collider.gameObject.GetComponent<GhostChild>())
            {
                //make the ghost child start haunting the player in the car by calling the startHaunting() function
                collider.gameObject.GetComponent<GhostChild>().startHaunting();
            }
        }
        //if the player collides with the "home" tagged trigger collider, meaning he reached the end, start the ending scene
        if (collider.gameObject.tag == "home")
        {
            SceneManager.LoadScene("EndingScene");
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        //if the player collides with the deer
        if (collision.transform.tag == "Deer")
        {
            //trigger a falling animation for the deer
            collision.gameObject.GetComponent<Animator>().SetBool("deerFalls", true);
            //decrease the car damage accessing the PlayerController script
            GetComponentInParent<PlayerController>().decreaseCarDamage(collision.relativeVelocity.magnitude);
        }
        //if the player collides with a tree
        if (collision.transform.tag == "Tree")
        {
            //decrease the car damage accessing the PlayerController script
            GetComponentInParent<PlayerController>().decreaseCarDamage(collision.relativeVelocity.magnitude);
        }
        //if the player collides with a fallen tree
        if (collision.transform.tag == "FallenTree")
        {
            //decrease the car damage accessing the PlayerController script
            GetComponentInParent<PlayerController>().decreaseCarDamage(collision.relativeVelocity.magnitude);
        }
    }

}
