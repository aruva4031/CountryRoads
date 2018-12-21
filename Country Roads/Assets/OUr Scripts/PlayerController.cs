/**
 *  Title:       PlayerController.cs
 *  Description: Used to manage and decrease the car damage
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public int carDamage;                           //to hold the amount of car damage
    public RCC_CarControllerV3 carController;       //to access the car controller

	// Use this for initialization
	void Start () {
        //set the car damage on 10
        carDamage = 10;
        //get the car controller script component
        carController = GetComponent<RCC_CarControllerV3>();
        //start a function to enable the trigger collider after 0.5 seconds
        StartCoroutine(TriggerDelay());
	}

    //functiont to activate the trigger collider after a delay
    IEnumerator TriggerDelay()
    {
        //wait for 0.5 seconds
        yield return new WaitForSeconds(0.5f);
        //activate the trigger collider
        GameObject.FindWithTag("GenerationCollider").GetComponent<SphereCollider>().enabled = true;
    }
	
    //function to decrease the car damage based on the magnitude of the car crash
    public int decreaseBy(float magnitude)
    {
        //return the amount to decrease based on the magnitude input
        if (magnitude > 15)
        {
            return 3;
        }
        else if (magnitude > 5)
        {
            return 2;
        }
        return 1;
    }

    //function to decrease the car damage
    public void decreaseCarDamage(float magnitude)
    {
        //find the amount to decrease the car damage by
        int decreasingAmount = decreaseBy(magnitude);
        //decrease the car damage by the decrease amount if it is greater than zero
        if (carDamage > 0)
        {
            carDamage-=decreasingAmount;
        }
        //if the car damage is less than 0, reset it to 0
        if (carDamage < 0)
        {
            carDamage = 0;
        }
        //if the car damage is zero
        if (carDamage == 0)
        {
            //speed is zero
            carController.maxspeed = 0f;
        }
        //if the car damage is half
        else if (carDamage == 5)
        {
            //speed is half
            carController.maxspeed *= 0.5f;
        }
        //if the car damage is 7
        else if (carDamage == 7)
        {
            //speed is 3/4
            carController.maxspeed *= 0.75f;
        }
    }
}
